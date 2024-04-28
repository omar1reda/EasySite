using Core.Entites;
using Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Product
{
    public class UpSell:BaseEntites
    {
        public int Count { get; set; }
        public int TotlePrice { get; set; }
        public string BuyingText { get; set; }
        public bool ShowUnitPrice { get; set; }

        public int ProductId { get; set; }

    }
}
