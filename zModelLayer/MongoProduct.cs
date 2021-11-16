using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer
{

    public class MongoProduct
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
    }
  
}
