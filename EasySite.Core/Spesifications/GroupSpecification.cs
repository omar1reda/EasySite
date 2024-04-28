using EasySite.Core.Entites.SignalR;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class GroupSpecification:Spesification<Group>
    {
        public GroupSpecification(string UserId) : base(g=>g.UsersGroups.Any(u=>u.AppUserId==UserId)) 
        {
            IncludeStrings.Add("UsersGroups.AppUser");
            IncludeStrings.Add("UsersGroups.Group");
            Includes.Add(g => g.Messages);
        }

        public GroupSpecification(string name , int _) : base(g => g.Name == name)
        {
            IncludeStrings.Add("UsersGroups.AppUser");
            IncludeStrings.Add("UsersGroups.Group");
            Includes.Add(g => g.Messages);
        }
    }
}
