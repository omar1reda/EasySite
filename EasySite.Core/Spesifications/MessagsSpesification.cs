using EasySite.Core.Entites.SignalR;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class MessagsSpesification:Spesification<Message>
    {
        public MessagsSpesification(int groupId):base(m=>m.GroupId== groupId)
        {
            
        }
    }
}
