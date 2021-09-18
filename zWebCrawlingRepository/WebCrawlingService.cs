using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using zModelLayer.ViewModels;

namespace zWebCrawlingRepository
{
    public static class WebCrawlingService
    {
        public static IServiceCollection AddWebCrawlingService(this IServiceCollection service)
        {
            service.AddTransient<IWebCrawling<EveryPage>, DATOWebCrawlingRepossitory>();
          
            return service;
        }
    }
}
