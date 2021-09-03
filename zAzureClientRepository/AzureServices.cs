using Microsoft.Extensions.DependencyInjection;
using System;
using zAzureClientRepository.IRepositories;
using zAzureClientRepository.Repositories;
using zModelLayer;
using zModelLayer.Blobs;

namespace zAzureClientRepository
{
    public static class AzureServices
    {
        public static IServiceCollection AddAzureServices(this IServiceCollection service)
        {
     
            service.AddScoped<IAzureStorage<ProductsBlob>, AzureStroageRepository>();

            //var builder = service.BuildServiceProvider();
            //var result = builder.GetService<IAzureStorage<ProductsBlob>>().DownloadLoadPicturesAsBase64("123kitten.png");
            return service;
        }
    }
}
