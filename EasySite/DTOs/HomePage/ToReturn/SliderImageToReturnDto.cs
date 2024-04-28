using Core.Entites;

namespace EasySite.DTOs.HomePage.ToReturn
{
    public class SliderImageToReturnDto : BaseEntites
    {
        public string Image { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public int SliderId { get; set; }
    }
}
