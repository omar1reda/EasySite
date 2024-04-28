using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class Product_Variant_Options:BaseEntites
    {
        public string OptionName { get; set; }

        public int Product_VariantsId { get; set; }

        //[InverseProperty("Product_Variant_Options")]
        public Product_Variants Product_Variant { get; set; }
    }
}
