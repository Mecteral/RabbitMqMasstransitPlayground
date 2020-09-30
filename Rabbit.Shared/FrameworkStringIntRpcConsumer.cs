﻿using System;
using System.Threading.Tasks;
using MassTransit;

namespace Rabbit.Shared
{
    public class FrameworkStringIntRpcConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
        {
            await context.RespondAsync<StringIntResultModel>(new StringIntResultModel
            {
                IntValue = context.Message.IntValue,
                StringValue = context.Message.StringValue,
                DateTime = DateTime.Now
            });
        }
    }

    public class CoreStringIntRpcConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
        {
            await context.RespondAsync<StringIntResultModel>(new StringIntResultModel
            {
                IntValue = context.Message.IntValue,
                StringValue = context.Message.StringValue,
                DateTime = DateTime.Now
            });
        }
    }
}