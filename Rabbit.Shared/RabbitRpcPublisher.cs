using System;
using System.Threading.Tasks;
using MassTransit;
using Rabbit.Shared.Models;

namespace Rabbit.Shared
{
    public class RabbitRpcPublisher
    {
        public static async Task<StringIntResultModel> SendToFrameworkRpcQueue(StringIntRequestModel stringIntRequestModel)
            => await SendRpcCall(stringIntRequestModel, RabbitConstants.FrameWorkRpcQueue);

        public static async Task<StringIntResultModel> SendToCoreRpcQueue(StringIntRequestModel stringIntRequestModel)
            => await SendRpcCall(stringIntRequestModel, RabbitConstants.CoreRpcQueue);

        private static async Task<StringIntResultModel> SendRpcCall(StringIntRequestModel stringIntRequestModel, string queueName)
        {
            try
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(RabbitMqBusFactory.ConfigureRabbitHost);

                await bus.StartAsync();

                var client =
                    bus.CreateRequestClient<StringIntRequestModel>(new Uri($"rabbitmq://{RabbitConstants.RabbitHost}/{queueName}"));

                var response = await client.GetResponse<StringIntResultModel>(stringIntRequestModel);

                await bus.StopAsync();

                return response.Message;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}