using Core.Entites;

namespace EasySite.DTOs.HomePage.ToReturn
{
    public class HomePageToreturn
    {
        public int Id { get; set; }
        public bool ShowInHedear { get; set; }
        public bool IsActive { get; set; }
        public int SiteId { get; set; }

        public List<DepartmentsInHomePageDto> DepartmentsInHomePageDto { get; set; } = new List<DepartmentsInHomePageDto>();
        public List<ProductsInHomePageDto> ProductsInHomePageDto { get; set; } = new List<ProductsInHomePageDto>();
        public List<SliderOtReturnDto> SliderOtReturnDto { get; set; } = new List<SliderOtReturnDto>();
    }
}
