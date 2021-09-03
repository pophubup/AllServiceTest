using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System;
using System.Linq;
using zFireBaseRepository.IRepositories;
using zModelLayer;
using Grpc.Core;
namespace zFireBaseRepository.Repositories
{
    public class FireBaseRepository : IFireBase
    {
        private IConfiguration _config;
        public FireBaseRepository(IConfiguration configuration)
        {
            _config = configuration;
        }
        public FirestoreDb InitalClient 
        { 
            get 
            {
             
                var data = _config.GetSection("Firebase");
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                GoogleCredential cred = GoogleCredential.FromJson(json);
                Channel channel = new Channel(FirestoreClient.DefaultEndpoint.Host,
                             FirestoreClient.DefaultEndpoint.Port,
                               cred.ToChannelCredentials());
                FirestoreClient client = FirestoreClient.Create(channel);
                FirestoreDb db = FirestoreDb.Create("getproducts-92bee", client);
                return db;
            }
        }
        public ResponseModel BulkInsert(List<CategoryViewModel> objs)
        {
            throw new NotImplementedException();
        }

        public List<CategoryViewModel> GetData()
        {
            throw new NotImplementedException();
        }
    }
}
