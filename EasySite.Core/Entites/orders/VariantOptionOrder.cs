using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Order
{
    public class VariantOptionOrder:BaseEntites
    {
        public string Variant { get; set; }
        public string Option { get; set; }
        public int ProductVariantsInOrderId { get; set; }
    }
}
