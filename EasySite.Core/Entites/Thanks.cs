using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class Thanks:BaseEntites
    {
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsbuttonToHome { get; set; }
        public string ShowDepartment { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
