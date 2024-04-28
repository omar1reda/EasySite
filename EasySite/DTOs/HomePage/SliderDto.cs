using Core.Entites;

namespace EasySite.DTOs.HomePage
{
    public class SliderDto:BaseEntites
    {
        public int Index { get; set; }
        public List<SliderImageDto> SliderImageDto { get; set; } = new List<SliderImageDto>();
    }
}
