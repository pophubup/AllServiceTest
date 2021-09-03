using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using zAzureClientRepository.IRepositories;
using zModelLayer.Blobs;

namespace zAzureClientRepository.Repositories
{
    public class AzureStroageRepository : IAzureStorage<ProductsBlob>
    {
        IConfiguration _Configuration;
        public AzureStroageRepository(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public ProductsBlob TargetDetil => new ProductsBlob("products", _Configuration["Azure:connectionstring"]);

        public BlobContainerClient GetContainer()
        {
            return new BlobServiceClient(TargetDetil._endpoint).GetBlobContainerClient(TargetDetil._blobName);
        }

        public BlobContainerClient CreateContainer()
        {
            return new BlobServiceClient(TargetDetil._endpoint).CreateBlobContainer(TargetDetil._blobName);
        }

        public string DownloadLoadPicturesAsBase64(string fileName)
        {
            
            BlobClient blobClient = GetContainer().GetBlobClient(fileName);
            using (var memoryStream = new MemoryStream())
            {
                blobClient.DownloadTo(memoryStream);
                string base64 = Convert.ToBase64String(memoryStream.ToArray());

                return base64;
            }
        
        }

        public Stream DownloadLoadPicturesAsStream(string FileName)
        {
            BlobClient blobClient = GetContainer().GetBlobClient(FileName);
            using (var memoryStream = new MemoryStream())
            {
                blobClient.DownloadTo(memoryStream);
                string base64 = Convert.ToBase64String(memoryStream.ToArray());

                return memoryStream;
            }
        }

      
    }
}
