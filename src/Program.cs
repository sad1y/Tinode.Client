using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Pbx;

namespace Tinode.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var client = new TinodeClient("127.0.0.1:6061"))
            {
                client.OnServerResponse += (msg) =>
                {
                    Console.WriteLine(msg);
                };

                var connectResponse = await client.ConnectAsync();
                Console.WriteLine("connected successfully!");
                Console.WriteLine($"ver: {connectResponse.Version}, build: {connectResponse.Build}");

                var loginResponse = await client.LoginAsync("alice", "alice123");

                // var createTopicResponse = await client.CreateTopicAsync("zoth_topic");
                await client.GetTopicsAsync();

                while (true)
                {
                    await Task.Delay(300);
                }
            }
        }
    }
}