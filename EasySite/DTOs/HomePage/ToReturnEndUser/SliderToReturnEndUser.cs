using EasySite.Core.Entites.SittingFormOrder;

namespace EasySite.DTOs.HomePage.ToReturnEndUser
{
    public class SliderToReturnEndUser 
    {
        public string TypeItem { get; set; }
        public int Index { get; set; }
        public List<SliderImagesToReturnEndUser> SliderImagesToReturnEndUser { get; set; } = new List<SliderImagesToReturnEndUser>();

    }
}
