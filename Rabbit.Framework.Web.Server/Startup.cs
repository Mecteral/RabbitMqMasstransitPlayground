﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Shared;
using Rabbit.Shared.Models;

namespace Rabbit.Framework.Web.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            var frameworkBus = RabbitMqBusFactory.CreateFrameworkBusWithReceiveEndpoint();

            Task.Factory.StartNew(async () =>
            {
                await frameworkBus.StartAsync();

                await RabbitQueuePublisher.SendToCoreQueue(new StringIntRequestModel
                {
                    IntValue = 17,
                    StringValue = "CoreCall"
                });

                var response = await RabbitRpcPublisher.SendToCoreRpcQueue(new StringIntRequestModel
                {
                    IntValue = 13, StringValue = "FromFrameworkRpcCall"
                });

                Console.WriteLine($"int: {response.IntValue}, string: {response.StringValue}, time: {response.DateTime}");
            });
        }
    }
}
