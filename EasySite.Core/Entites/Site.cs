using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entites.Homepage;
using Core.Entites.Order;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Entites
{
    public class Site:BaseEntites
    {

        public string? TitleSite { get; set; } = "خصم 10%";
        public string ColorTextHedar { get; set; } = "#fff";
        public string colorSite { get; set; } = "#fd9c03";
        public string? Logo { get; set; }
        public string? WhatsApp { get; set; }
        public string? Coll_Phone { get; set; }
        public int? ShippingPrice { get; set; }
        public string? MiniIcon { get; set; }
        public FontType? FontType { get; set; }


        //[InverseProperty("Site")]
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }


        [InverseProperty("Site")]
        public ICollection<ShippingGovernoratesPrices> ShippingGovernoratesPrices { get; set; } = new HashSet<ShippingGovernoratesPrices>();


        [InverseProperty("Site")]
        public ICollection<Pages> Pages { get; set; } = new HashSet<Pages>();


        [InverseProperty("Site")]
        public Thanks Thanks { get; set; }


        [InverseProperty("Site")]
        public ICollection<Transactions> Transactions { get; set; } = new HashSet<Transactions>();


        [InverseProperty("Site")]
        public Homepage.Homepage Homepage { get; set; }


        [InverseProperty("Site")]
        public ICollection<Department> Departments { get; set; } = new HashSet<Department>();


    }
}
