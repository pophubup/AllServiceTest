using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using zModelLayer;
namespace MongodbClientRepository
{
    public class MongoProductService
    {
        private IMongoCollection<MongoProduct> _MongoProduct;
        public MongoProductService(IMongoCollections settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _MongoProduct = database.GetCollection<MongoProduct>(settings.Collections.FirstOrDefault());
        }

        public List<MongoProduct> Get() =>
            _MongoProduct.AsQueryable().ToList();
        public MongoProduct Get(string ProductID) =>
            _MongoProduct.Find(g => g.ProductID == ProductID).FirstOrDefault();
        public bool Create(List<MongoProduct> data)
        {
            try
            {
                _MongoProduct.InsertMany(data);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
         
        }
        public bool Create(MongoProduct product)
        {
            try
            {
                _MongoProduct.InsertOne(product);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
         
        }
        public bool Update(string ProductID, MongoProduct updateobj)
        {
            try
            {
                ReplaceOneResult result = _MongoProduct.ReplaceOne(g => g.ProductID == ProductID, updateobj);
                if (result.ModifiedCount == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
         
        }
        public void Remove(string ProductID) =>
            _MongoProduct.DeleteOne(g => g.ProductID == ProductID);
        
    }
}
