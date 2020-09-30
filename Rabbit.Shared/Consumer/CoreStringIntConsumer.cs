using System;
using System.Threading.Tasks;
using MassTransit;
using Rabbit.Shared.Models;

namespace Rabbit.Shared.Consumer
{
    public class CoreStringIntConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
            => Console.WriteLine($"Core consumer called, string: {context.Message.StringValue}, int: {context.Message.IntValue}");
    }
}