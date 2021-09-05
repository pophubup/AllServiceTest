using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using zModelLayer;

namespace zGoogleCloudStorageClient
{
    public class GoogleStorageRepository : IGoogleStorageRepository
    {
        private IConfiguration _configuration;
        public GoogleStorageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public StorageClient Client { 
            
            get {
                FireBaseAuth fireBaseAuth = _configuration.GetSection("Firebase").Get<FireBaseAuth>();

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(fireBaseAuth);
                GoogleCredential cred = GoogleCredential.FromJson(json);
           
               return  StorageClient.Create(cred);
            } 
        
        
        }

        public bool CreateFiles(List<ImageContainer> localpath, string labelName)
        {
           
            var storage = Client;
            localpath.ForEach(g =>
            {
                storage.UploadObject(labelName, g.objName ,g.FileExtension, g.stream, new UploadObjectOptions() { PredefinedAcl = PredefinedObjectAcl.PublicRead }) ;
            });
            return true;
          
        }
       public bool DeleteFile(string bucketid,string labelName)
        {
            Client.DeleteObject(bucketid, labelName);
            return true;
        }

        public bool CreateFolder(string labelName)
        {
           
            Client.CreateBucket(_configuration["Firebase:project_id"], labelName);
            return true;
        }
        public bool DeleteFolder(string labelName)
        {
            Client.DeleteBucket(labelName);
            return true;
        }
    }
}
