using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs.SignalRDto
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string messageText { get; set; }
        public DateTime Date { get; set; }

        // ==> شاف الرساله؟
        public string statusMessage { get; set; }
        // ===> نوع الرساله 
        public string messageType { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
