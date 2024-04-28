using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class SocialMedia:BaseEntites
    {
        public bool IsActive { get; set; }
        public string? LinkedIn { get; set; }
        public string? MyProperty { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? TikTok { get; set; }
        public string? YouTube { get; set; }

        public int SiteId { get; set; }

        //[InverseProperty("SocialMedia")]
        public Site Site { get; set; }
    }
}
