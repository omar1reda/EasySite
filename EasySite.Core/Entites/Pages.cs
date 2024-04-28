using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class Pages:BaseEntites
    {
        public string PageName { get; set; }
        public bool IsActive  { get; set; }
        public bool IsFooter { get; set; }
        public int Index { get; set; }
        public bool IsHeader  { get; set; }
        public string content { get; set; }

        [InverseProperty("Pages")]
        public Site Site { get; set; }
        public int SiteId { get; set; }
    }
}
