using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Order;
using EasySite.Core.Entites.SittingFormOrder;

namespace Core.Entites.orders
{
    public class Order : BaseEntites
    {
        public string? FullName { get; set; }
        public string Phone { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public string Government { get; set; }
        public int ShippingPrice { get; set; }
        public int? NumberShippingDays { get; set; }
        public bool PaymentMade { get; set; }

        public string IpAddres { get; set; }


        public string? Utm_SourceCampaign { get; set; }
        public string? Utm_NameCampaign { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public StatusOrder Status { get; set; } = StatusOrder.UnderReview;
        public bool IsWatched { get; set; } = false;
        public int SiteId { get; set; }

    }
}
