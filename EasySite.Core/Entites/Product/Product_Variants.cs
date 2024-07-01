using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class Product_Variants:BaseEntites
    {
        public string VariantsName { get; set; }

        public int ProductId { get; set; }

        //[InverseProperty("Product_Variants")]
        public Product Product { get; set; }


        //[InverseProperty("Product_Variants")]
        public ICollection<Product_Variant_Options> Product_Variant_OptionsS { get; set; } = new HashSet<Product_Variant_Options>();

    }
}
