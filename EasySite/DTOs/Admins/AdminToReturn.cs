namespace EasySite.DTOs.Admins
{
    public class AdminToReturn
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? Token { get; set; }
    }
}
