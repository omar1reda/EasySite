using Core.Entites;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SittingFormOrder
{
    public class Phone : BaseEntites
    {
        public string? TypeItem { get; set; } = "Phone";
        public string phone { get; set; }
        public string? TextPlaceholder { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
    }
}
