using Core.Entites;

namespace EasySite.DTOs
{
    public class TransactionAllDataDto:BaseEntites
    {
        public int Money { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public string AppUserId { get; set; }
        public string TransactionTime { get; set; }
    }
}
