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
        public bool IsActive { get; set; }
        public string GovernorateName { get; set; }
        public string Price { get; set; }
        public bool IsShippingDuration { get; set; }
        public string? ShippingDuration { get; set; }


        public int SiteId { get; set; }

        [InverseProperty("ShippingGovernoratesPrices")]
        public Site Site { get; set; }

    }
}
