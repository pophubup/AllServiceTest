using Neo4jClient;
using Neo4jClientRepository.Entities;
using Neo4jClientRepository.IRespositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zModelLayer;

namespace Neo4jClientRepository.Repositories
{
    public class ProductRepository: IProducts
    {
        private IBoltGraphClient _client;
        public ProductRepository(IBoltGraphClient client)
        {
            _client = client;
        }
        public List<ProductViewModel> GetAllData()
        {
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            _client.Cypher.Match(@"(n:Product)").Return(n =>
            n.As<Neo4jProduct>()).ResultsAsync.GetAwaiter().GetResult()
                .ToList()
                .ForEach(g => {

                    productViewModels.Add(new ProductViewModel()
                    {
                        Neo4jProductId = g.productID,
                        ProductPrice = g.productPrice
                    });
                });
            return productViewModels;
        }
        public bool InsertBulkData(List<ProductViewModel> productViewModels)
        {
            List<Neo4jProduct> neo4JProducts = new List<Neo4jProduct>();
            productViewModels.ForEach(g =>
            {

                var data = _client.Cypher
                 .Create("(n:Product{ productID: " + g.Neo4jProductId + " , productPrice: " + g.ProductPrice + " })")
                  .Return(n => n.As<Neo4jProduct>()).ResultsAsync.GetAwaiter().GetResult().ToList();
                neo4JProducts.Concat(data);
            });

            return neo4JProducts.Count() == productViewModels.Count();
        }
    }
}
