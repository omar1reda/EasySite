using Core.Entites;
using EasySite.Core.Entites.Baskets;
using EasySite.Core.Entites.orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Order
{
    public class OrderItem :BaseEntites
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public bool IsUpSalle { get; set; }
        public int OrderId { get; set; }
        public ProductDataInOrder ProductDataInOrder { get; set; }
        public int ProductDataInOrderId { get; set; }

    }
}
