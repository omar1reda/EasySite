using Core.Entites;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySite.DTOs
{
    public class PageDto
    {
        public string PageName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFooter { get; set; }
        public int Index { get; set; }
        public bool IsHeader { get; set; }
        public string content { get; set; }
        public int SiteId { get; set; }
    }
}
