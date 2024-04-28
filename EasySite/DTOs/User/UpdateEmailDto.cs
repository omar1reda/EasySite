using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.User
{
    public class UpdateEmailDto
    {
        public string UserId { get; set; }
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()-++_])(?=.*\\d).{6,}$",
        ErrorMessage = "يجب ان يحتوي الباسورد علي ارقام و حروف صغيره وكبيره ورموز")]
        public string Password { get; set; }
    }
}
