using Core.Entites;

namespace EasySite.DTOs
{
    public class BlockedNumbersToReturnDto:BaseEntites
    {
        public string MessegeError { get; set; }
        public string Number { get; set; }
        public int SiteId { get; set; }
    }
}
