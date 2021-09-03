using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using zFireBaseRepository.IRepositories;
using zModelLayer;
using Grpc.Core;

namespace zFireBaseRepository.Repositories
{
    public class FireBaseRepository : IFireBase
    {
        private IConfiguration _configuration;
        public FireBaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public FirestoreDb InitalClient 
        { 
            get 
            {
                FireBaseAuth fireBaseAuth = _configuration.GetSection("Firebase").Get<FireBaseAuth>();
               
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(fireBaseAuth);
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
            string msg = string.Empty;
            WriteBatch batch = InitalClient.StartBatch();
            DocumentReference nycRef = InitalClient.Collection("category").Document(DateTime.Now.ToString("yyyyMMddHHmmss"));
            objs.ToList().ForEach(x =>
            {
                batch.Set(nycRef, x);
                msg += $"{x.CategoryName},";
            });
            var result = batch.CommitAsync().GetAwaiter().GetResult();
            return new ResponseModel()
            {
                isSuccess = result.Count() == objs.Count(),
                Message = result.Count() == objs.Count() ? msg + " Bulk Insert Success" : msg + "something went wrong"
            };
        }

        public List<CategoryViewModel> GetData()
        {

            Query allCitiesQuery = InitalClient.Collection("products");
            QuerySnapshot allCitiesQuerySnapshot = allCitiesQuery.GetSnapshotAsync().GetAwaiter().GetResult();
            List<CategoryViewModel> data = new List<CategoryViewModel>();
            foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
            {
                CategoryViewModel categoryView = documentSnapshot.ConvertTo<CategoryViewModel>();
                data.Add(categoryView);
            }
      

            return data;
        }
    }
}
