using EasySite.Core.Entites.Product;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class product_Variant_Options_ProductDataSpecification:Spesification<product_Variant_Options_ProductData>
    {
        public product_Variant_Options_ProductDataSpecification(int productDataId , string optionName ):base(r=>r.ProductDataId== productDataId && r.Product_Variant_Option.OptionName == optionName)
        {
            Includes.Add(o => o.Product_Variant_Option);
            Includes.Add(o => o.ProductData);
        }
    }
}
