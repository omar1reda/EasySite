using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SittingFormOrder
{
    public class Address:  BaseEntites
    {
        [Required]
        public string address { get; set; }
        public string? TypeItem { get; set; } = "Address";
        public string? TextPlaceholder { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
    }
}
