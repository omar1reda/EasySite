namespace EasySite.DTOs
{
    public class UpdatePasswordDto
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string NewRePassword { get; set; }
    }
}
