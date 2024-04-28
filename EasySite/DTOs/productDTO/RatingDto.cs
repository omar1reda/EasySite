using Core.Entites;
using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.productDTO
{
    public class RatingDto:BaseEntites
    {
        public string Text { get; set; }
        [Required]
        public int Count { get; set; } = 5;
        public bool ShowRating { get; set; } = false;
        public string UerName { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
