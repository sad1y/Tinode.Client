using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Tinode.Client.Exceptions;
using Pbx;
using Tinode.Client.Extensions;

namespace Tinode.Client
{
    public class TinodeClient : IDisposable
    {
        private int _cmdId;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly string _serverAddress;

        private readonly ConcurrentDictionary<string, Action<ServerMsg>>
            _internalSubscriber = new ConcurrentDictionary<string, Action<ServerMsg>>();

        private readonly ConcurrentBag<string> _topicSubscribtions = new ConcurrentBag<string>();
        private readonly object _sync = new object();
        private AsyncDuplexStreamingCall<ClientMsg, ServerMsg> _loop;
        private bool _isSubscribedToMe;


        public event Action<ServerMsg> OnServerResponse;


        public TinodeClient(string serverAddress)
        {
            if (string.IsNullOrEmpty(serverAddress)) throw new ArgumentNullException(nameof(serverAddress));

            _serverAddress = serverAddress;
            _cmdId = Environment.TickCount;
        }

        public void Disconnect()
        {
            lock (_sync)
            {
                _cts?.Cancel();
                _cts = null;
            }
        }

        public Task<ConnectResponse> ConnectAsync()
        {
            CancellationToken token;

            lock (_sync)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                token = _cts.Token;

                var channel = new Channel(_serverAddress, ChannelCredentials.Insecure);
                var client = new Node.NodeClient(channel);
                _loop = client.MessageLoop();
            }

            Task.Factory.StartNew(
                async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        var hasNewMsg = await _loop.ResponseStream.MoveNext(CancellationToken.None);
                        if (!hasNewMsg) break;

                        var msg = _loop.ResponseStream.Current;

                        if (_internalSubscriber.TryGetValue(msg.GetId(), out var action))
                        {
                            action(msg);
                        }

                        var onOnServerResponse = OnServerResponse;
                        onOnServerResponse?.Invoke(msg);
                    }
                }, TaskCreationOptions.LongRunning);

            return SayHi();
        }

        private string GenerateMessageId() => Interlocked.Increment(ref _cmdId).ToString("x8");

        private async Task<ConnectResponse> SayHi()
        {
            var id = GenerateMessageId();
            var message = new ClientMsg
            {
                Hi = new ClientHi
                {
                    Id = id,
                    Ver = "0.15.6-rc5",
                    Lang = "EN",
                    DeviceId = id,
                    UserAgent = "tinode-dotnet-core-client"
                }
            };

            var rvcMsg = await SendMessageAsync(message, message.Hi.Id);

            rvcMsg.Ctrl.Params.TryGetValue("ver", out var version);
            rvcMsg.Ctrl.Params.TryGetValue("build", out var build);

            return new ConnectResponse(version?.ToStringUtf8(), build?.ToStringUtf8());
        }

        public async Task<LoginResponse> LoginAsync(string login, string password)
        {
            var message = new ClientMsg
            {
                Login = new ClientLogin
                {
                    Id = GenerateMessageId(),
                    Scheme = "basic",
                    Secret = ByteString.CopyFromUtf8(login + ":" + password),
                }
            };

            var rcvMsg = await SendMessageAsync(message, message.Login.Id);

            rcvMsg.Ctrl.Params.TryGetValue("user", out var user);
            rcvMsg.Ctrl.Params.TryGetValue("authlvl", out var authlvl);
            rcvMsg.Ctrl.Params.TryGetValue("token", out var token);
            rcvMsg.Ctrl.Params.TryGetValue("expires", out var expires);

            await SubscribeToMeTopicAsync();

            return new LoginResponse(authlvl?.ToStringUtf8(), user?.ToStringUtf8(), token?.ToStringUtf8(), expires?.ToStringUtf8());
        }

        public async Task<CreateAccountResponse> CreateAccountAsync(string login, string password, string[] tags = null, bool authorize = false)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentException("Value cannot be null or empty.", nameof(login));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", nameof(password));

            if (login.IndexOf(":", StringComparison.Ordinal) > -1 || password.IndexOf(":", StringComparison.Ordinal) > -1)
                throw new ArgumentException("neither the login nor the password should contain ':' symbol");

            var message = new ClientMsg
            {
                Acc = new ClientAcc
                {
                    Id = GenerateMessageId(),
                    UserId = "new",
                    Scheme = "basic",
                    Secret = ByteString.CopyFromUtf8(login + ":" + password),
                    Login = authorize,
                }
            };

            if (tags?.Any() == true)
                message.Acc.Tags.AddRange(tags);

            var rcvMsg = await SendMessageAsync(message, message.Acc.Id);

            rcvMsg.Ctrl.Params.TryGetValue("desc", out var desc);
            rcvMsg.Ctrl.Params.TryGetValue("user", out var user);
            rcvMsg.Ctrl.Params.TryGetValue("token", out var token);
            rcvMsg.Ctrl.Params.TryGetValue("expires", out var expires);

            return new CreateAccountResponse(desc?.ToStringUtf8(), user?.ToStringUtf8(), token?.ToStringUtf8(), expires?.ToStringUtf8());
        }

        public async Task<CreateTopicResponse> CreateTopicAsync(string name, string[] tags = null, bool allowAnon = false)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

            var vCardData = (new VCard
            {
                FormattedName = name
            }).AsJson();

            var message = new ClientMsg
            {
                Sub = new ClientSub
                {
                    Id = GenerateMessageId(),
                    Topic = "new",
                    SetQuery = new SetQuery
                    {
                        Desc = new SetDesc
                        {
                            Public = ByteString.CopyFrom(vCardData)
                        }
                    }
                }
            };

            if (tags?.Any() == true)
                message.Sub.SetQuery.Tags.AddRange(tags);

//            message.Sub.SetQuery.Tags.Add("name:" + name);


            var rcvMsg = await SendMessageAsync(message, message.Sub.Id);

            var topicId = rcvMsg.Ctrl.Topic;

            return new CreateTopicResponse(topicId);
        }

        public Task SubscribeAsync(string topicName)
        {
            if (_topicSubscribtions.TryPeek(out var topic) && topicName == topic)
                return Task.CompletedTask;

            var message = new ClientMsg
            {
                Sub = new ClientSub
                {
                    Id = GenerateMessageId(),
                    Topic = topicName,
                }
            };

            return SendMessageAsync(message, message.Sub.Id)
                .ContinueWith(task => { _topicSubscribtions.Add(topicName); }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public Task SubscribeToMeTopicAsync() => SubscribeAsync("me");

        public async Task<IEnumerable<TopicSubscribtion>> GetTopicsAsync()
        {
            var message = new ClientMsg
            {
                Get = new ClientGet
                {
                    Id = GenerateMessageId(),
                    Topic = "me",
                    Query = new GetQuery
                    {
                        What = "sub"
                    }
                }
            };

            var rcvMessages = await SendMessageAsync(message, message.Get.Id);

            return rcvMessages.Meta == null ? Enumerable.Empty<TopicSubscribtion>() : rcvMessages.Meta.Sub.Select(TopicSubscribtion.FromTopicSub);
        }

        public async Task InviteUserAsync(string topic, string user, AccessPermission acs)
        {
            await SubscribeAsync(topic);
            
            var message = new ClientMsg
            {
                Set = new ClientSet
                {
                    Id = GenerateMessageId(),
                    Topic = topic,
                    Query = new SetQuery
                    {
                        Sub = new SetSub
                        {
                            UserId = user,
                            Mode = acs.AsString()
                        }
                    }
                }
            };

            await SendMessageAsync(message, message.Set.Id);
        }

        private Task SendMessageAsync(ClientMsg msg) => _loop.RequestStream.WriteAsync(msg);

        private Task<ServerMsg> SendMessageAsync(ClientMsg message, string operationId)
        {
            var tsc = new TaskCompletionSource<ServerMsg>();

            var counter = 0;

            while (true)
            {
                if (counter > 3)
                    throw new TimeoutException("failed to registering operation callback");

                if (_internalSubscriber.TryAdd(operationId, ServerAnswerHandler))
                    break;

                counter++;
            }

            void ServerAnswerHandler(ServerMsg answer)
            {
                var answerCode = answer.GetCode();
                if (answerCode < 400)
                {
                    tsc.TrySetResult(answer);
                    return;
                }

                switch (answerCode)
                {
                    case 401:
                    {
                        tsc.TrySetException(new TinodeUnauthorizedExcpetion(answer));
                        return;
                    }
                    default:
                    {
                        tsc.TrySetException(new TinodeSeverExcpetion(answer));
                        return;
                    }
                }
            }

            SendMessageAsync(message).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
#if DEBUG
                    Console.WriteLine(message);
#endif
                    return;
                }

                _internalSubscriber.TryRemove(operationId, out _);

                if (task.IsFaulted)
                {
                    tsc.TrySetException(task.Exception.InnerException);
                }
            });

            return tsc.Task;
        }

        public void Dispose()
        {
            Disconnect();
            var loop = _loop;
            loop.RequestStream.CompleteAsync().ContinueWith(task => loop.Dispose());
        }
    }
}