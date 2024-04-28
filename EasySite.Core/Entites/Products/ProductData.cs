using EasySite.Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class ProductData: BaseEntites
    {
        //public string Id { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public bool ProductSoldOut  { get; set; }
        public int Count { get; set; }

        public int ProductId { get; set; }

        //[InverseProperty("ProductData")]
        public Product Product { get; set; }

        //[InverseProperty("ProductData")]

        public ICollection<product_Variant_Options_ProductData> product_Variant_Options_ProductData { get; set; } = new HashSet<product_Variant_Options_ProductData>();
    }
}
