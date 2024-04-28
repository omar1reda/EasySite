using Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Product
{
    public class product_variant_option_combination
    {
      
        public int SKUId { get; set; }

        public int Product_Variant_OptionsId { get; set; }



    }
}
