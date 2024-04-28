using Core.Entites;

namespace EasySite.DTOs
{
    public class DepartmentToreturnDto:BaseEntites
    {
        public string Name { get; set; }
        public int index { get; set; }
        public string LinkName { get; set; }
        public bool ShowInHedar { get; set; }
        public string Image { get; set; }
        public int SiteId { get; set; }
    }
}
