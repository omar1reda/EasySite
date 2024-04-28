namespace EasySite.DTOs.productDTO
{
    public class ImagesProductDto
    {
        public IFormFile MainImage { get; set; }
        public List<IFormFile>? OtherImage { get; set; }

        //public ProductDto productDto { get; set; }
    }
}
