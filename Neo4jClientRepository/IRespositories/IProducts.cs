using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace Neo4jClientRepository.IRespositories
{
    public interface IProducts
    {
        public List<ProductViewModel> GetAllData();
        public bool InsertBulkData(List<ProductViewModel> productViewModels);
    }
}
