using Core.Entites;

namespace EasySite.DTOs
{
    public class ThanksDto
    {
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsbuttonToHome { get; set; }
        public string ShowDepartment { get; set; }
        public int SiteId { get; set; }
    }
}
