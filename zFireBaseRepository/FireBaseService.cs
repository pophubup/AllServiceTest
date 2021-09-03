using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using zFireBaseRepository.IRepositories;
using zFireBaseRepository.Repositories;
using zModelLayer;

namespace zFireBaseRepository
{
    public static class FireBaseService
    {
         public static IServiceCollection AddFireBaseService(this IServiceCollection service)
        {
          
            service.AddScoped<IFireBase, FireBaseRepository>();
            //var builder = service.BuildServiceProvider();
            //var result = builder.GetService<IFireBase>().BulkInsert(new System.Collections.Generic.List<CategoryViewModel>() { 
            
            //   new CategoryViewModel()
            //   {
            //       CategoryID = 1,
            //       CategoryName ="aaaaa"
            //   },
            //      new CategoryViewModel()
            //   {
            //       CategoryID = 2,
            //       CategoryName ="bbbb"
            //   },
            //          new CategoryViewModel()
            //   {
            //       CategoryID = 3,
            //       CategoryName ="cccc"
            //   }
            //});
            return service;
        }
    }
}
