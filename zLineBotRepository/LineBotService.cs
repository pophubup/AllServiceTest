using Microsoft.Extensions.DependencyInjection;
using System;
using isRock.LineBot;
using Microsoft.Extensions.Configuration;

namespace zLineBotRepository
{
    public static class LineBotService
    {
        public static IServiceCollection AddLineBotService(this IServiceCollection service)
        {

            service.AddTransient<ILineBot, LazyLineBotService>();
            return service;
        }
    }
}
