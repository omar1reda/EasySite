using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Order;
using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.OrdersDto
{
    public class OrderFromBasketDto
    {
        [Required]
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
        //public List<OrderItemDto> OrderItemDto { get; set; } 
        //public double TotalPrice { get; set; }
        [Required]
        public string  BasketId { get; set; }

       



    }
}
