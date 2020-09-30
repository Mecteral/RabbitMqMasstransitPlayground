using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rabbit.Shared;

namespace Rabbit.Core.Web.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            var coreBus = RabbitMqBusFactory.CreateCoreBusWithReceiveEndpoint();
            var coreRpcBus = RabbitMqBusFactory.CreateCoreRpcEndpoint();

            Task.Factory.StartNew(async () =>
            {
                await coreBus.StartAsync();
                await coreRpcBus.StartAsync();

                await RabbitQueuePublisher.SendToFrameworkQueue(new StringIntRequestModel
                {
                    IntValue = 13, StringValue = "FrameworkCall"
                });

                var response = await RabbitRpcPublisher.SendToFrameworkRpcQueue(new StringIntRequestModel
                {
                    IntValue = 13,
                    StringValue = "FromFrameworkRpcCall"
                });

                Console.WriteLine($"int: {response.IntValue}, string: {response.StringValue}, time: {response.DateTime}");
            });
        }
    }
}
