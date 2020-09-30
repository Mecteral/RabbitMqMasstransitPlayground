using MassTransit;
using MassTransit.RabbitMqTransport;
using Rabbit.Shared.Consumer;

namespace Rabbit.Shared
{
    public class RabbitMqBusFactory
    {
        public static IBusControl CreateFrameworkBusWithReceiveEndpoint()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                ConfigureRabbitHost(cfg);

                cfg.ReceiveEndpoint(RabbitConstants.FrameWorkQueue, e =>
                {
                    e.Consumer<FrameworkStringIntConsumer>();
                });

                cfg.ReceiveEndpoint(RabbitConstants.FrameWorkRpcQueue, e =>
                {
                    e.Consumer<FrameworkStringIntRpcConsumer>();
                });
            });
        }

        public static IBusControl CreateCoreBusWithReceiveEndpoint()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                ConfigureRabbitHost(cfg);

                cfg.ReceiveEndpoint(RabbitConstants.CoreWorkQueue, e =>
                {
                    e.Consumer<CoreStringIntConsumer>();
                });
            });
        }

        public static void ConfigureRabbitHost(IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.Host(RabbitConstants.RabbitHost, h =>
            {
                h.Username(RabbitConstants.RabbitUserName);
                h.Password(RabbitConstants.RabbitPassword);
            });
        }

        public static IBusControl CreateCoreRpcEndpoint()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                ConfigureRabbitHost(cfg);

                cfg.ReceiveEndpoint(RabbitConstants.CoreRpcQueue, e =>
                {
                    e.Consumer<CoreStringIntRpcConsumer>();
                });
            });
        }
    }
}
