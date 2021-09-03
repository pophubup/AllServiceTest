using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.Drawing;
using System.IO;

namespace zAzureClientRepository.IRepositories
{
    
    public interface IAzureStorage<T> 
    {
        T TargetDetil { get; }
        BlobContainerClient CreateContainer();
        BlobContainerClient GetContainer();
        Stream DownloadLoadPicturesAsStream(string FileName);
        string DownloadLoadPicturesAsBase64(string FileName);

    }
}
