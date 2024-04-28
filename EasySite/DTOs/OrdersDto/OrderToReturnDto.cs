using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Order;

namespace EasySite.DTOs.OrdersDto
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Government { get; set; }
        public int ShippingPrice { get; set; }
        public int? NumberShippingDays { get; set; }

        public List<OrderItemDto> OrderItemDto { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public bool IsWatched { get; set; }

         /// اسم الحمله ومصدرها
        public string? Utm_SourceCampaign { get; set; }
        public string? Utm_NameCampaign { get; set; }
    }
}
