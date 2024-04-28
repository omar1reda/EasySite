using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SittingFormOrder
{
    public class Email  :BaseEntites
    {
        public string? TypeItem { get; set; } = "Email";
        public string email { get; set; }
        public string? TextPlaceholder { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
    }
}
