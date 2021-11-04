using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongodbClientRepository
{
    public class MongoDBSetting: IMongoCollections
    {
        public List<string> Collections { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IMongoCollections
    {
       List< string> Collections { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
