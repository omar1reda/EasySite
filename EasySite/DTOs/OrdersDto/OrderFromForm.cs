using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.OrdersDto
{
    public class OrderFromForm
    {
        public int SiteId { get; set; }
        public string? FullName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public GovernorateDto Government { get; set; }
        public List< OrderItemDto> OrderItemDto { get; set; }

        
        public double TotalPrice()
        {
            double Total = 0;
            foreach (var item in OrderItemDto)
            {
                Total +=(double) item.Price * item.Count + Government.ShippingPrice;
            }
            return Total;
        }

    }
}
