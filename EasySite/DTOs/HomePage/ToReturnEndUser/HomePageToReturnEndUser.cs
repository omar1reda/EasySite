using EasySite.Core.Entites.SittingFormOrder;
using EasySite.DTOs.HomePage.ToReturn;
using EasySite.DTOs.productDTO;
using EasySite.DTOs.productDTO.ToReturn;

namespace EasySite.DTOs.HomePage.ToReturnEndUser
{
    public class HomePageToReturnEndUser
    {
        public int Id { get; set; }
        public bool ShowInHedear { get; set; }
        public bool IsActive { get; set; }
        //public List<DepartmentsToReturnEndUser> DepartmentsToReturnEndUser { get; set; } = new List<DepartmentsToReturnEndUser>();
        //public List<ProductToReturnEndUser> ProductToReturnEndUser { get; set; } = new List<ProductToReturnEndUser>();
        //public List<SliderToReturnEndUser> SliderToReturnEndUser { get; set; } = new List<SliderToReturnEndUser>();
        public List<object> AllItemesInHomePage { get; set; } = new List<object>();
    }
}
