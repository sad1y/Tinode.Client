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
                client.OnServerResponse += (msg => Console.WriteLine(msg.MessageCase));

                var connectResponse = await client.ConnectAsync();
                Console.WriteLine("connected successfully!");
                Console.WriteLine($"ver: {connectResponse.Version}, build: {connectResponse.Build}");

                // var createUser = client.CreateAccountAsync("zoth", "qwerty1");
//                var loginResult = await client.LoginAsync("zoth", "qwerty1");
                var loginResult = await client.LoginAsync("alice", "alice123");
                Console.WriteLine("logged as {0}", loginResult.User);

                // var createTopicResponse = await client.CreateTopicAsync("zoth_topic");
//                
                var topics = await client.GetTopicsAsync();
                foreach (var topic in topics)
                {
                    Console.WriteLine("[{1}] [{3}] {0}, {2}", topic.Topic, topic.Online, topic.Public.FormattedName, topic.Acs);
                }

                // await client.CreatePerToPerTopicAsync("usrCXKp4ZUszEk");

//                 await client.PublishContentAsync("usrCXKp4ZUszEk", DraftyMessage.Create("test message"));

                // await client.LeaveAsync("grpZwqX8J9Y1a8", unsub: true);

//                await client.InviteUserAsync("grpZwqX8J9Y1a8", "usrCXKp4ZUszEk",
//                    AccessPermission.Join | AccessPermission.Read | AccessPermission.Write | AccessPermission.Sharing);
            }

//            await Test("alice", "alice123");
//            await Task.Delay(5000);
        }
    }
}