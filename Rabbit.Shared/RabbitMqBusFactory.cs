using System;
using System.Threading.Tasks;
using MassTransit;

namespace Rabbit.Shared
{
    public class RabbitMqBusFactory
    {
        private const string FrameWorkQueue = "FrameworkQueueEndpoint";
        private const string CoreWorkQueue = "CoreQueueEndpoint";
        private const string FrameWorkRpcQueue = "FrameworkRpcQueueEndpoint";
        private const string CoreRpcQueue = "CoreRpcQueueEndpoint";

        public static IBusControl CreateFrameworkBusWithReceiveEndpoint()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost");

                cfg.ReceiveEndpoint(FrameWorkQueue, e =>
                {
                    e.Consumer<FrameworkStringIntConsumer>();
                });

                cfg.ReceiveEndpoint(FrameWorkRpcQueue, e =>
                {
                    e.Consumer<FrameworkStringIntRpcConsumer>();
                });
            });
        }

        public static async Task SendToFrameworkQueue(StringIntRequestModel stringIntRequestModel)
        {
            await SendToEndpoint(stringIntRequestModel, FrameWorkQueue);
        }

        public static async Task<StringIntResultModel> SendToFrameworkRpcQueue(StringIntRequestModel stringIntRequestModel)
        {
            return await SendRpcCall(stringIntRequestModel, FrameWorkRpcQueue);
        }

        public static async Task<StringIntResultModel> SendToCoreRpcQueue(StringIntRequestModel stringIntRequestModel)
        {
            return await SendRpcCall(stringIntRequestModel, CoreRpcQueue);
        }

        public static async Task SendToCoreQueue(StringIntRequestModel stringIntRequestModel)
        {
            await SendToEndpoint(stringIntRequestModel, CoreWorkQueue);
        }

        public static IBusControl CreateCoreBusWithReceiveEndpoint()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost");

                cfg.ReceiveEndpoint(CoreWorkQueue, e =>
                {
                    e.Consumer<CoreStringIntConsumer>();
                });
            });
        }

        public static IBusControl CreateCoreRpcEndpoint()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost");

                cfg.ReceiveEndpoint(CoreRpcQueue, e =>
                {
                    e.Consumer<CoreStringIntRpcConsumer>();
                });
            });
        }

        private static async Task SendToEndpoint(object obj, string queueName)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost");
            });

            var sendEndpoint = await bus.GetSendEndpoint(new Uri($"rabbitmq://localhost/{queueName}"));

            await sendEndpoint.Send(obj);
        }

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
