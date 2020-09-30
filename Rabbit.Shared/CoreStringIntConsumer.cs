using System;
using System.Threading.Tasks;
using MassTransit;

namespace Rabbit.Shared
{
    public class CoreStringIntConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
            => Console.WriteLine($"Core consumer called, string: {context.Message.StringValue}, int: {context.Message.IntValue}");
    }
}