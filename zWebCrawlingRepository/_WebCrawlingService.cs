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
            service.AddTransient<CommonhealthCrawlingRepository>();
           var builder = service.BuildServiceProvider();
            //DateTime result = Convert.ToDateTime("2021/01/01 08:00");
            // var see = ((DateTimeOffset)result).ToUnixTimeSeconds();
            //var result = builder.GetService<IDATOWebCrawling>().GetDataFromWebElement("c.y", 4);
            //var start = DateTime.Now.AddDays(-30);
            //var end = DateTime.Now.AddDays(30);

            //var check =   builder.GetService<IAnua>().GetPastRecordBasedOnBuyersByTimeSpan("1303", start, end);
            builder.GetService<CommonhealthCrawlingRepository>().GetBullshitContent();
            return service;
        }
    }
}
