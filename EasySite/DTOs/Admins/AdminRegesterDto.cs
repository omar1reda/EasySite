using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.Admins
{
    public class AdminRegesterDto
    {
 
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()-++_])(?=.*\\d).{6,}$",
            ErrorMessage = "يجب ان يحتوي الباسورد علي ارقام و حروف صغيره وكبيره ورموز")]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
