using Core.Entites;

namespace EasySite.DTOs.productDTO.ToReturn
{
    public class Product_VariantsToReturnDto
    {
        public int Id { get; set; }
        public string VariantsName { get; set; }

        public List<Product_Variant_OptionsToReturnDto> Product_Variant_OptionsToReturnDto { get; set; }

    }
}
