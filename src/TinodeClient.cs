using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Tinode.Client.Exceptions;
using Tinode.Client.Response;
using Pbx;

namespace Tinode.Client
{
    public class TinodeClient : IDisposable
    {
        private int _cmdId;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly string _serverAddress;

        private readonly ConcurrentDictionary<string, Action<ServerMsg>>
            _internalSubscriber = new ConcurrentDictionary<string, Action<ServerMsg>>();

        private static readonly object Sync = new object();
        private AsyncDuplexStreamingCall<ClientMsg, ServerMsg> _loop;

        private event Action<ServerMsg> OnServerResponse;

        public TinodeClient(string serverAddress)
        {
            if (string.IsNullOrEmpty(serverAddress)) throw new ArgumentNullException(nameof(serverAddress));

            _serverAddress = serverAddress;
            _cmdId = Environment.TickCount;
            OnServerResponse = msg => { }; // eliminate null validation
        }

        public void Disconnect()
        {
            lock (Sync)
            {
                _cts?.Cancel();
                _cts = null;
            }
        }

        public Task<ConnectResponse> ConnectAsync()
        {
            CancellationToken token;

            lock (Sync)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource(30_000);
                token = _cts.Token;

                var channel = new Channel(_serverAddress, ChannelCredentials.Insecure);
                var client = new Node.NodeClient(channel);
                _loop = client.MessageLoop();
            }

            Task.Factory.StartNew(
                async () =>
                {
                    while (await _loop.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var msg = _loop.ResponseStream.Current;

                        if (_internalSubscriber.TryGetValue(msg.Ctrl.Id, out var action))
                        {
                            action(msg);
                        }
                    }
                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

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
                    Ver = "0.15",
                    Lang = "EN",
                    DeviceId = id,
                    UserAgent = "dotnet-core-client"
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
                if (answer.Ctrl.Code < 400)
                {
                    tsc.TrySetResult(answer);
                    return;
                }

                switch (answer.Ctrl.Code)
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
                if (task.IsCompletedSuccessfully) return;

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
            loop.RequestStream.CompleteAsync();
        }
    }
}