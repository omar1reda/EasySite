using Core.Entites;

namespace EasySite.DTOs
{
    public class TransactionDto
    {
        public int Money { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public string AppUserId { get; set; }
    }
}
