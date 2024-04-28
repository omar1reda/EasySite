using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.SittingFormOrder
{
    public class SittingFormOrder:BaseEntites
    {
        public int SiteId { get; set; }
        [Required]
        public Phone Phone { get; set; }
        [Required]
        public Government Government { get; set; }
        public FullName? FullName { get; set; }
        public Email? Email { get; set; }
        public Country? Country { get; set; }
        [Required]
        public Address Address { get; set; }
    }
}
