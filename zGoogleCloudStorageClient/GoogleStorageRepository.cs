using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                storage.UploadObject(labelName, g.objName,g.FileExtension, g.stream);
            });
            return true;
          
        }

        public bool CreateFolder(string labelName)
        {
            Client.CreateBucket("getproducts-92bee", labelName);
            return true;
        }
    }
}
