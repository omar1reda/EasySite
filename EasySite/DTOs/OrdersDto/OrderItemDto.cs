using EasySite.Core.Entites.SittingFormOrder;

namespace EasySite.DTOs.OrdersDto
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public bool IsUpSalle { get; set; }
        public productDataInOrderDto? productDataInOrderDto { get; set; }
    }
}
