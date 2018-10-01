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
                var connectResponse = await client.ConnectAsync();
                Console.WriteLine("connected successfully!");
                Console.WriteLine($"ver: {connectResponse.Version}, build: {connectResponse.Build}");

                var loginResponse = await client.LoginAsync("zoth1", "qwerty");

                var createTopicResponse = await client.CreateTopicAsync("zoth_topic");
            }
        }
    }
}