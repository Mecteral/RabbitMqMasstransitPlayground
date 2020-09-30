using System;
using System.Threading.Tasks;
using MassTransit;
using Rabbit.Shared.Models;

namespace Rabbit.Shared
{
    public class RabbitQueuePublisher
    {
        public static async Task SendToFrameworkQueue(StringIntRequestModel stringIntRequestModel)
        {
            await SendToEndpoint(stringIntRequestModel, RabbitConstants.FrameWorkQueue);
        }

        public static async Task SendToCoreQueue(StringIntRequestModel stringIntRequestModel)
        {
            await SendToEndpoint(stringIntRequestModel, RabbitConstants.CoreWorkQueue);
        }

        private static async Task SendToEndpoint(object obj, string queueName)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(RabbitConstants.RabbitHost);
            });

            await bus.StartAsync();

            var sendEndpoint = await bus.GetSendEndpoint(new Uri($"rabbitmq://{RabbitConstants.RabbitHost}/{queueName}"));

            await sendEndpoint.Send(obj);

            await bus.StopAsync();
        }
    }
}