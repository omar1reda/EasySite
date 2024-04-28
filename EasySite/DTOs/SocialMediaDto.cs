namespace EasySite.DTOs
{
    public class SocialMediaDto
    {
        public bool IsActive { get; set; }
        public string? LinkedIn { get; set; }
        public string? MyProperty { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? TikTok { get; set; }
        public string? YouTube { get; set; }

        public int SiteId { get; set; }
    }
}
