using Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Product
{
    public class product_Variant_Options_ProductData
    {
        public int ProductDataId { get; set; }
        public ProductData ProductData { get; set; }

        public int Product_Variant_OptionsId { get; set; }
        public Product_Variant_Options Product_Variant_Option { get; set; }
    }
}
