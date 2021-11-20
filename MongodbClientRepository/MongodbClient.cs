using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;
namespace MongodbClientRepository
{
    public static class MongodbClient
    {
        public static IServiceCollection AddMongoDBCleint(this IServiceCollection service, IConfiguration Configuration)
        {
            var data = Configuration.GetSection("MongoDB:Collections").Get<List<string>>();
            service.Configure<MongoDBSetting>(configure => {

                configure.Collections = Configuration.GetSection("MongoDB:Collections").Get<List<string>>();
                configure.ConnectionString = Configuration.GetSection("MongoDB:ConnectionString").Value.ToString();
                configure.DatabaseName = Configuration.GetSection("MongoDB:DatabaseName").Value.ToString();
            });
            service.AddSingleton<IMongoCollections>(sp => sp.GetRequiredService<IOptions<MongoDBSetting>>().Value);
            service.AddSingleton<MongoProductService>();
            //var buidler = service.BuildServiceProvider();
            //var vvvv = buidler.GetService<MongoProductService>();
            ////vvvv.Create(new List<MongoProduct>()
            ////{
            ////    new MongoProduct()
            ////    {
            ////        ProductID = "M456",
            ////        ProductName ="豬肉"
            ////    },
            ////    new MongoProduct()
            ////    {
            ////        ProductID = "M789",
            ////        ProductName ="雞肉"
            ////    },
            ////    new MongoProduct()
            ////    {
            ////        ProductID ="M012",
            ////        ProductName = "兔肉"
            ////    }
            ////});
            //var finall = vvvv.Get();
            //var findone = vvvv.Get("M789");
            //findone.ProductName = "肌肉雞肉";
            //vvvv.Update("M789", findone);
            return service;
        }
    }
}
