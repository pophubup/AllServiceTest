using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using zFireBaseRepository.IRepositories;
using zFireBaseRepository.Repositories;

namespace zFireBaseRepository
{
    public static class FireBaseService
    {
         public static IServiceCollection AddFireBaseService(this IServiceCollection service)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(@"C:\Users\Yohoo\OneDrive\桌面\myDotnet123\haha\zFireBaseRepository\key.json", false);
            //service.AddScoped<IFireBase, FireBaseRepository>();
            //var builder = service.BuildServiceProvider();
            //var result = builder.GetService<IFireBase>().InitalClient;
            return service;
        }
    }
}
