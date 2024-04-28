using Core.Entites;

namespace EasySite.DTOs.HomePage.ToReturn
{
    public class SliderOtReturnDto:BaseEntites
    {
        public int Index { get; set; }
        public List<SliderImageToReturnDto> SliderImageToReturnDto { get; set; } = new List<SliderImageToReturnDto>();
    }
}
