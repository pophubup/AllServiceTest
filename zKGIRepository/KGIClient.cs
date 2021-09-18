using Microsoft.Extensions.DependencyInjection;
using Smart;
using System;

namespace zKGIRepository
{
    public static class KGIClient
    {
        public static IServiceCollection AddKGITradComService(this IServiceCollection service)
        {
        

            service.AddScoped<IKGITradeCom, KGITradComRepository>();
            return service;
        }
    }
}
