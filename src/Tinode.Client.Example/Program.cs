using System;
using System.Threading.Tasks;
using Pbx;

namespace Tinode.Client.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new TinodeClient("127.0.0.1:6061");
            {
                client.OnServerResponse += Console.WriteLine;

                var connectResponse = await client.ConnectAsync();
                Console.WriteLine("connected successfully!");
                Console.WriteLine($"ver: {connectResponse.Version}, build: {connectResponse.Build}");

                var loginResult = await client.LoginAsync("zoth", "qwerty1");
//                var loginResult = await client.LoginAsync("alice", "alice123");
                Console.WriteLine("logged as {0}", loginResult.User);

//                var createTopicResponse = await client.CreateTopicAsync("zoth_topic");
//                
//                var topics = await client.GetTopicsAsync();
//                foreach (var topic in topics)
//                {
//                    Console.WriteLine("[{1}] [{3}] {0}, {2}", topic.Topic, topic.Online, topic.Public.FormattedName, topic.Acs);
//                }

                // topic: "grpZwqX8J9Y1a8"
                // alice: "usrCXKp4ZUszEk"
                // zoth: "usr18E4Fe82wrw"

                await client.CreatePerToPerTopicAsync("usrCXKp4ZUszEk");
                

                // await client.LeaveAsync("grpZwqX8J9Y1a8", unsub: true);

//                await client.InviteUserAsync("grpZwqX8J9Y1a8", "usrCXKp4ZUszEk",
//                    AccessPermission.Join | AccessPermission.Read | AccessPermission.Write | AccessPermission.Sharing);
            }

//            await Test("alice", "alice123");
//            await Task.Delay(5000);
        }

//        static async Task Test(string login, string password)
//        {
//            var channel = new Channel("127.0.0.1:6061", ChannelCredentials.Insecure);
//            var client = new Node.NodeClient(channel);
//            var loop = client.MessageLoop();
//
//
//            await loop.RequestStream.WriteAsync(new ClientMsg
//            {
//                Hi = new ClientHi
//                {
//                    Id = "101",
//                    Ver = "0.15.6-rc5",
//                    Lang = "EN",
//                    DeviceId = "101",
//                    UserAgent = "tinode-dotnet-core-client"
//                }
//            });
//
//            await loop.RequestStream.WriteAsync(new ClientMsg
//            {
//                Login = new ClientLogin
//                {
//                    Id = "102",
//                    Scheme = "basic",
//                    Secret = ByteString.CopyFromUtf8(login + ":" + password),
//                }
//            });
//
//            await loop.RequestStream.WriteAsync(new ClientMsg
//            {
//                Sub = new ClientSub
//                {
//                    Id = "103",
//                    Topic = "me",
//                }
//            });
//
//            await Task.Delay(1000);
//
//            await loop.RequestStream.WriteAsync(new ClientMsg
//            {
//                Get = new ClientGet
//                {
//                    Id = "104",
//                    Topic = "me",
//                    Query = new GetQuery
//                    {
//                        What = "sub"
//                    }
//                }
//            });
//
//            while (true)
//            {
//                var hasNewMsg = await loop.ResponseStream.MoveNext(CancellationToken.None);
//                if (!hasNewMsg) break;
//
//                var msg = loop.ResponseStream.Current;
//                Console.WriteLine(msg);
//            }
//        }
    }
}