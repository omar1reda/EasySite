using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class SKU:BaseEntites
    {
        public int Price { get; set; }
        public int Sale { get; set; }
        public bool ProductSoldOut  { get; set; }
        public int Count { get; set; }


        public int ProductId { get; set; }

        //[InverseProperty("SKU")]
        public Product Product { get; set; }
    }
}
