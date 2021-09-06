using Google.Cloud.Storage.V1;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace zGoogleCloudStorageClient
{
    public static class GoogleStorage
    {
        public static IServiceCollection AddGoogleStorageService(this IServiceCollection service)
        {
            service.AddScoped<IGoogleStorageRepository, GoogleStorageRepository>();
            var builder = service.BuildServiceProvider();
            //builder.GetService<IGoogleStorageRepository>().CreateFolder("66666");
            //System.IO.FileStream fileStream = System.IO.File.OpenRead(@"C:\Users\Yohoo\Desktop\螢幕擷取畫面 2021-09-04 164537.png");
            // builder.GetService<IGoogleStorageRepository>().CreateFiles(new System.Collections.Generic.List<zModelLayer.ImageContainer>()
            // {

            //     new zModelLayer.ImageContainer()
            //     {
            //         objName = "螢幕擷取畫面 2021-09-04 164537.png",
            //         stream = fileStream
            //     }
            // }, "66666");
            //var storageObjects = builder.GetService<IGoogleStorageRepository>().Client.ListObjects("632a6dc7-dc7c-4a3f-9a3a-a80feba9ea33");
            //foreach (var storageObject in storageObjects)
            //{
            //    Console.WriteLine(storageObject.Name);

            //}
           //string url =  builder.GetService<IGoogleStorageRepository>().GetPublicUrl("95d9d95c-b376-4d01-bc61-de911426790a", "test.jpg");
            return service;
        }
    }
}
