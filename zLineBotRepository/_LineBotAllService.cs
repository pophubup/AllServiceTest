using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using zLineBotRepository.Interface;

namespace zLineBotRepository
{
   
    public static class _LineBotAllService
    {
        public static IServiceCollection AddLineBotAllService(this IServiceCollection service)
        {

            service.AddScoped<LazyLineBotService>();
            service.AddScoped<Func<string, ILineBot>>(provider => name =>
            {
                var type = Assembly.GetAssembly(typeof(ILineBot)).GetType($"zLineBotRepository.{name}LineBotService");
                var instance = provider.GetService(type);

                return instance as ILineBot;
            });
           
            return service;
        }
    }
}
