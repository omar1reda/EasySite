using Core.Entites;
using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs
{
    public class DepartmentDto:BaseEntites
    {

        [Required(ErrorMessage = "الاسم مطلوب")]
        public string Name { get; set; }
        public int index { get; set; } = new Random().Next(1, 201);
        [Required(ErrorMessage = "اسم اللينك مطلوب")]
        public string LinkName { get; set; }
        public bool ShowInHedar { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "يجب وضع  صوره")]
        public IFormFile ImageFile { get; set; }

        public int SiteId { get; set; }
    }
}
