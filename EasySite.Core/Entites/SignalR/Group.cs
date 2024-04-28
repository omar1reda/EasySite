using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SignalR
{
    public class Group:BaseEntites
    {
        public string Name { get; set; }
        public ICollection<UsersGroups> UsersGroups { get; set; } = new HashSet<UsersGroups>();
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
