using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabbit.Shared;

namespace Rabbit.Core.ConsoleApp.Client
{
    class Program
    {
        private static int _intValue;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Press r to send requests");
            Console.WriteLine("Press x to exit console");

            char currentKey;

            do
            {
                currentKey = Console.ReadKey().KeyChar;

                if (currentKey == 'r')
                {
                    IList<Task> tasks = new List<Task>();
                    for (var i = 0; i < 5; i++)
                    {
                        tasks.Add(SendRequest());
                    }

                    await Task.WhenAll(tasks);
                }
            } while (currentKey != 'x');
        }

        private static async Task SendRequest()
        {
            var result = await RabbitRpcPublisher.SendToCoreRpcQueue(new StringIntRequestModel
            {
                IntValue = _intValue++,
                StringValue = "FrameworkRpcCall"
            });

            Console.WriteLine($"{Environment.NewLine}string: {result.StringValue}, int: {result.IntValue}, date: {result.DateTime}{Environment.NewLine}");
        }
    }
}
