using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class ShippingGovernoratesPrices:BaseEntites
    {
       
        public string GovernorateName { get; set; }
        public int Price { get; set; }
        public bool IsShippingDuration { get; set; }
        public int? ShippingDuration { get; set; }


        public int SiteId { get; set; }

        [InverseProperty("ShippingGovernoratesPrices")]
        public Site Site { get; set; }

    }
}
