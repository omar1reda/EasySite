namespace EasySite.DTOs.OrdersDto
{
    public class productDataInOrderDto
    {
        public List<VariantOptionOrderDto>? VariantOptionOrderDto { get; set; } = new List<VariantOptionOrderDto>();
        public int productDataId { get; set; }
    }
}
