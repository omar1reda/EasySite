using Core.Entites;
using EasySite.Core.Entites.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SignalR
{
    public class Message:BaseEntites
    {
        public string messageText { get; set; }
        public DateTime Date { get; set; }

        // ==> شاف الرساله؟
        public StatusMessage statusMessage { get; set; }

        public MessageType messageType { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
