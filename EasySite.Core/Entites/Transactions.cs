using EasySite.Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class Transactions:BaseEntites
    {
        public int Money { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public DateTime TransactionTime { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
