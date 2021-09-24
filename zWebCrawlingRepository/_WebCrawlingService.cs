using Microsoft.Extensions.DependencyInjection;
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
            //var result = builder.GetService<IWebCrawling<EveryPage>>().GetDataFromWebElement("c.y", 4);
            return service;
        }
    }
}
