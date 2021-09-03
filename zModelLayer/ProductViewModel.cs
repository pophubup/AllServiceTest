using System;

namespace zModelLayer
{
    public class ProductViewModel
    {
        public string Neo4jProductId { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public CategoryViewModel categoryViewModel { get; set; }
    }
}
