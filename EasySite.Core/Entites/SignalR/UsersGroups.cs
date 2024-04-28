using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SignalR
{
    public class UsersGroups
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Group Group { get; set; }
        public int GroupId { get; set; }
    }
}
