using Microsoft.Extensions.DependencyInjection;

namespace zLineBotRepository
{
    public static class LineBotService
    {
        
        public static IServiceCollection AddLineBotService(this IServiceCollection service)
        {

            service.AddTransient<LazyLineBotService>();
            service.AddTransient<OneToAllBot>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Lazy":
                        return serviceProvider.GetService<LazyLineBotService>();
                    default:
                        return null;
                }
            });
            return service;
        }
    }
}
