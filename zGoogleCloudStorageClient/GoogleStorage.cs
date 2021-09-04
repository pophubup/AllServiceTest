using Microsoft.Extensions.DependencyInjection;
using System;

namespace zGoogleCloudStorageClient
{
    public static class GoogleStorage
    {
        public static IServiceCollection AddGoogleStorageService(this IServiceCollection service)
        {
            service.AddScoped<IGoogleStorageRepository, GoogleStorageRepository>();
           //var builder = service.BuildServiceProvider();
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
            return service;
        }
    }
}
