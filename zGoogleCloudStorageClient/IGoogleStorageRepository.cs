using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using zModelLayer;

namespace zGoogleCloudStorageClient
{
    public interface IGoogleStorageRepository
    {
        StorageClient Client { get; }
        public bool CreateFolder(string labelName);
        public bool DeleteFolder(string labelName);
        public bool CreateFiles(List<ImageContainer> localpath, string labelName);
        public bool DeleteFile(string bucketid, string labelName);


    }
}
