using Core.Entites.Product;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class ProductVirantOptionSpeification:Spesification<Product_Variant_Options>
    {
        public ProductVirantOptionSpeification(int ProductId , string OptionName):base(o=>o.Product_Variant.ProductId== ProductId&&o.OptionName== OptionName)
        {
            Includes.Add(o => o.Product_Variant);
            Includes.Add(o => o.product_Variant_Options_ProductData);

        }

        public ProductVirantOptionSpeification(int ProductId) : base(o => o.Product_Variant.ProductId == ProductId)
        {
            Includes.Add(o => o.Product_Variant);
            Includes.Add(o => o.product_Variant_Options_ProductData);

        }
    }
}
