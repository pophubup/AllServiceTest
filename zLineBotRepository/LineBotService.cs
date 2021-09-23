using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace zLineBotRepository
{
   
    public static class LineBotService
    {
        private delegate ILineBot OneToAllBot(string name);
        public static IServiceCollection AddLineBotService(this IServiceCollection service)
        {

            service.AddScoped<LazyLineBotService>();
            service.AddScoped<Func<string, ILineBot>>(provider => name =>
            {
                var type = Assembly.GetAssembly(typeof(OneToAllBot)).GetType($"zLineBotRepository.{name}LineBotService");
                var instance = provider.GetService(type);

                return instance as ILineBot;
            });
           
            return service;
        }
    }
}
