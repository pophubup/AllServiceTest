using System;


namespace ImagekitCllient
{
    public static class Imagekit
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
