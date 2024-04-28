using Core.Entites;

namespace EasySite.DTOs.productDTO.ToReturn
{
    public class ProductDataToReturnDto 
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public bool ProductSoldOut { get; set; }
        public int Count { get; set; }

        public List<Product_Variant_OptionsToReturnDto> Product_Variant_OptionsToReturnDto { get; set; }
    }
}
