using Azure.Storage.Blobs;
using System;
using System.IO;
using zAzureClientRepository.IRepositories;
using zModelLayer.Blobs;

namespace zAzureClientRepository.Repositories
{
    public class AzureStroageRepository : IAzureStorage<ProductsBlob>
    {
        public ProductsBlob TargetDetil => new ProductsBlob();

        public BlobContainerClient GetContainer()
        {
            return new BlobServiceClient(TargetDetil.endpoint).GetBlobContainerClient(TargetDetil.BlobName);
        }

        public BlobContainerClient CreateContainer()
        {
            return new BlobServiceClient(TargetDetil.endpoint).CreateBlobContainer(TargetDetil.BlobName);
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
