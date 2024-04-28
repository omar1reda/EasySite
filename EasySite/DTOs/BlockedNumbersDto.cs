using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs
{
    public class BlockedNumbersDto
    {
        public string MessegeError { get; set; }
        public string Number { get; set; }
        public int SiteId { get; set; }
    }
}
