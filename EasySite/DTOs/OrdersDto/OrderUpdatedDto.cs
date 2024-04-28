using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs.OrdersDto
{
    public class OrderUpdatedDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public bool IsWatched { get; set; }
    }
}
