using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Baskets
{
    public class productDataBasket
    {
        public List<VariantOptionBasket> VariantOptions { get; set; } = new List<VariantOptionBasket>();
        public int productDataId { get; set; }
    }
}
