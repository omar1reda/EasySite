using Core.Entites;
using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs.HomePage
{
    public class SliderImageDto:BaseEntites
    {
        public IFormFile? ImageFile { get; set; }
        public string Image { get; set; }

        public string? TypeName { get; set; } = "NoType";
        /// id ====> product || Department || Page
        public int TypeId { get; set; }
        public int SliderId { get; set; }
    }
}
