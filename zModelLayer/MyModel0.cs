using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
      
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
       public string description { get; set; }
        public decimal price { get; set; }
        public List<Category> categories { get; set; }
    }
    public class MyModel0
    {
        public int Id { get; set; }

        public List<Product>  products { get; set; }
    }


    public class Container
    {
        public int ContainerId { get; set; }
        public List<ProductContainer> productContainers { get; set; }
    }
    public class ProductContainer
    {
        public int ProductContainer_Id { get; set; }
        public string ProductContainer_Name { get; set; }
        public string ProductContainer_description { get; set; }
        public string ProductContainer_price { get; set; }
        public List<CategoryContainer> categoryContainers { get; set; } 

    }
    public class CategoryContainer
    {
        public int CategoryContainer_Id { get; set; }
        public string CategoryContainer_Name { get; set; }
    }
}
