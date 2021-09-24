using Microsoft.Extensions.DependencyInjection;
using System;
using zModelLayer.ViewModels;

namespace zWebCrawlingRepository
{
    public static class _WebCrawlingService
    {
        public static IServiceCollection AddWebCrawlingService(this IServiceCollection service)
        {
            service.AddTransient<IDATOWebCrawling, DATOWebCrawlingRepossitory>();
            service.AddTransient<IAnua, AnuaWebCrawlingRepository>();
            var builder = service.BuildServiceProvider();
            DateTime result = Convert.ToDateTime("2021/01/01 08:00");
            var see = ((DateTimeOffset)result).ToUnixTimeSeconds();
            //var result = builder.GetService<IWebCrawling<EveryPage>>().GetDataFromWebElement("c.y", 4);
            return service;
        }
    }
}
