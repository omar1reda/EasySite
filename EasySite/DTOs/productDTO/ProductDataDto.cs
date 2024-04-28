namespace EasySite.DTOs.productDTO
{
    public class ProductDataDto
    {
        public int Price { get; set; }
        public int Sale { get; set; }
        public bool ProductSoldOut { get; set; }
        public int Count { get; set; }

        public List<Product_Variant_OptionsDto> Product_Variant_OptionsDto { get; set; }
    }
}
