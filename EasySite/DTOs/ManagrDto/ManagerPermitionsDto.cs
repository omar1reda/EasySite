using EasySite.Core.Entites;
using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.ManagrDto
{
    public class ManagerPermitionsDto
    {
        public string UserId { get; set; }

        public string? Email { get; set; } = $"{Guid.NewGuid().ToString()}@gmail.com";
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()-++_])(?=.*\\d).{6,}$",
        ErrorMessage = "يجب ان يحتوي الباسورد علي ارقام و حروف صغيره وكبيره ورموز")]
        public string Password { get; set; }

        public PermitionsDto PermitionsDto { get; set; }
        //[Required]
        //public ManagerDTO ManagerDTO { get; set; }

        
        //public bool ViewOrders { get; set; }
        //public bool EditStatusOrder { get; set; }
        //public bool AddProduct { get; set; }
        //public bool UpdateProduct { get; set; }
        //public bool DeleteProduct { get; set; }
        //public bool AddDepartment { get; set; }
        //public bool UpdateDepartment { get; set; }
        //public bool DeleteDepartment { get; set; }
        //public bool UpdateSite { get; set; }
        //public bool UpdateHomePage { get; set; }
        //public bool UpdateSittingFormOrder { get; set; }
        //public bool DeleteShippingGovernorates { get; set; }
        //public bool AddDeleteShippingGovernorates { get; set; }
        //public bool AddRating { get; set; }
        //public bool UpdateRating { get; set; }
        //public bool DeleteRating { get; set; }


    }
}
