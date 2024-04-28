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
        public string TransactionTime { get; set; }


        public int SiteId { get; set; }

        [InverseProperty("Transactions")]
        public Site Site { get; set; }
    }
}
