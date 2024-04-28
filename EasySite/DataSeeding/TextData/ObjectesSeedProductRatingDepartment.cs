using Core.Entites;
using Core.Entites.Product;
using EasySite.Core.Entites.SittingFormOrder;
using EasySite.DTOs.HomePage;
using EasySite.DTOs.productDTO;

namespace EasySite.DataSeeding.TextData
{
    public class ObjectesSeedProductRatingDepartment
    {
        public List< List<ProductDto>> ListOfListProduct { get; set; } = new List<List<ProductDto>>();
        public HomePageDto homePageDto { get; set; }
        public List<Pages> pages { get; set; } = new List<Pages>();
        public List<Ratings> ratings { get; set; } = new List<Ratings>();
        public List<Department> departments { get; set; } = new List<Department>();
        public SittingFormOrder sittingFormOrder { get; set; }
    }
}
