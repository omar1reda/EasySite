using Core.Entites;
using EasySite.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.orders
{
    public class ProductDataInOrder:BaseEntites
    {
        public int productDataId { get; set; }
        public ICollection<VariantOptionOrder> VariantOptionsOrder { get; set; } = new HashSet<VariantOptionOrder>();
        public int OrderItemId { get; set; }
    }
}
