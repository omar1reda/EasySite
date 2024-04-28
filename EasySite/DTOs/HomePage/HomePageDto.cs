namespace EasySite.DTOs.HomePage
{
    public class HomePageDto
    {
        public int Id { get; set; }
        public bool ShowInHedear { get; set; }
        public bool IsActive { get; set; }
        public int SiteId { get; set; }

        public List<DepartmentsInHomePageDto> DepartmentsInHomePageDto { get; set; } = new List<DepartmentsInHomePageDto>();
        public List<ProductsInHomePageDto> ProductsInHomePageDto { get; set; } = new List<ProductsInHomePageDto>();
        public List<SliderDto> SliderDto { get; set; } = new List<SliderDto>();

    }
}
