using EasySite.Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class Product_Variant_Options: BaseEntites
    {
        //public string Id { get; set; }
        public string OptionName { get; set; }

        public int Product_VariantsId { get; set; }

        //[InverseProperty("Product_Variant_Options")]
        public Product_Variants Product_Variant { get; set; }

        public ICollection<product_Variant_Options_ProductData> product_Variant_Options_ProductData { get; set; } = new HashSet<product_Variant_Options_ProductData>();




    }
}
