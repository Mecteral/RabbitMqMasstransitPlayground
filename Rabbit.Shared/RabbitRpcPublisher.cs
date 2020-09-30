using System;
using System.Threading.Tasks;
using MassTransit;

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
                var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("localhost");
                });

                await bus.StartAsync();

                var client =
                    bus.CreateRequestClient<StringIntRequestModel>(new Uri($"rabbitmq://localhost/{queueName}"));

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