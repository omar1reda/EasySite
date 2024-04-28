using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.OrdersDto
{
    public class BlockFakeOrdersDto
    {
        [Required]
        public int SiteId { get; set; }
        [Required]
        public bool IsBlockFakeOrders { get; set; } 
        public string? MassegeBlockFakeOrders { get; set; }
        public int? TimeBlockFakeOrders { get; set; } 
    }
}
