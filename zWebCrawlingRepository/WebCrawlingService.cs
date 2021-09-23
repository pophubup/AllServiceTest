using Microsoft.Extensions.DependencyInjection;
using zModelLayer.ViewModels;

namespace zWebCrawlingRepository
{
    public static class WebCrawlingService
    {
        public static IServiceCollection AddWebCrawlingService(this IServiceCollection service)
        {
            service.AddTransient<IWebCrawling<EveryPage>, DATOWebCrawlingRepossitory>();
            var builder = service.BuildServiceProvider();
            //var result = builder.GetService<IWebCrawling<EveryPage>>().GetDataFromWebElement("c.y", 4);
            return service;
        }
    }
}
