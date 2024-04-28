using Core.Entites;

namespace EasySite.DTOs.HomePage
{
    public class ProductsInHomePageDto:BaseEntites
    {
        public int Index { get; set; }
        public string Titele { get; set; }
        public int FormatNumber { get; set; }
        public string? SortBy { get; set; } = "NoType";
        public int DepartmentId { get; set; }
        //public int HomepageId { get; set; }
    }
}
