using EasySite.Core.Entites.Product;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class productVariantOptions_ProductDataSpecification:Spesification<product_Variant_Options_ProductData>
    {
        public productVariantOptions_ProductDataSpecification(int Table_RelationId):base(t=>t.ProductDataId == Table_RelationId)
        {
            
        }
    }
}
