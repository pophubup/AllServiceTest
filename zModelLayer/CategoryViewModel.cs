using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer
{
    [FirestoreData]
    public class CategoryViewModel
    {
        [FirestoreProperty]
        public int CategoryID { get; set; }
        [FirestoreProperty]
        public string CategoryName { get; set; }
    }

}
