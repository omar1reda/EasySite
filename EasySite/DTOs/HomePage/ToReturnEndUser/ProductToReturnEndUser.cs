using EasySite.Core.Entites.SittingFormOrder;
using EasySite.DTOs.productDTO.ToReturn;

namespace EasySite.DTOs.HomePage.ToReturnEndUser
{
    public class ProductToReturnEndUser 
    {
        public string TypeItem { get; set; }
        public int Index { get; set; }
        public string Titele { get; set; }
        public int FormatNumber { get; set; }

        public List<ProductToReturnDto> ProductToReturnDto { get; set; }=new List<ProductToReturnDto>();
    }
}
