namespace EasySite.DTOs.productDTO
{
    public class RatingToReturn
    {
        public int Count { get; set; }
        public List<RatingDto> ratingDto { get; set; } 
       public List<CTRrating> CTRrating { get; set; }= new List<CTRrating>();
    }
}
