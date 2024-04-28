using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SittingFormOrder
{
    public class FullName : BaseEntites
    {
        public string? TypeItem { get; set; } = "FullName";
        public string fullName { get; set; }
        public string? TextPlaceholder { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
    }
}
