using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace zFireBaseRepository.IRepositories
{
    public interface IFireBase
    {
        public FirestoreDb InitalClient { get; }
        public ResponseModel BulkInsert(List<CategoryViewModel> objs);
        public List<CategoryViewModel> GetData();
    }
}
