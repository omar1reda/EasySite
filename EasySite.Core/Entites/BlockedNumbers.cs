using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class BlockedNumbers:BaseEntites
    {
        public string MessegeError { get; set; }

        public string Number { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
