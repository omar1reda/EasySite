using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entites.Homepage;
using Core.Entites.orders;


//using Core.Entites.Order;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Order;
using EasySite.Core.Entites.SittingFormOrder;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Entites
{
    public class Site : BaseEntites
    {
        public bool IsActive { get; set; } = true;
        public string? Name { get; set; }
        public string LinkName { get; set; }
        public string? TitleSite { get; set; } = "خصم 10%";
        public string ColorTextHedar { get; set; } = "#fff";
        public string colorSite { get; set; } = "#fd9c03";
        public string? Logo { get; set; }
        public string? WhatsApp { get; set; }
        public string? Coll_Phone { get; set; }
        public int? ShippingPrice { get; set; }
        public bool ShippingAreasIsActive { get; set; }
        public string? MiniIcon { get; set; }
        public FontType? FontType { get; set; }
        public currency? Currency { get; set; }
        public string? HeaderCode { get; set; }
        // حجب الطلبات الوهميه
        public bool? IsBlockFakeOrders { get; set; }=false;
        public string? MassegeBlockFakeOrders { get; set; }
        public int? TimeBlockFakeOrders { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }


        [InverseProperty("Site")]
        public ICollection<ShippingGovernoratesPrices> ShippingGovernoratesPrices { get; set; } = new HashSet<ShippingGovernoratesPrices>();

        public ICollection<Order> Orsers { get; set; } = new HashSet<Order>();

        [InverseProperty("Site")]
        public ICollection<Pages> Pages { get; set; } = new HashSet<Pages>();


        [InverseProperty("Site")]
        public Thanks Thanks { get; set; }




        [InverseProperty("Site")]
        public Homepage.Homepage Homepage { get; set; }

        public SittingFormOrder SittingFormOrder { get; set; }

        [InverseProperty("Site")]
        public ICollection<Department> Departments { get; set; } = new HashSet<Department>();

        [InverseProperty("Site")]
        public ICollection<Product.Product> Products { get; set; } = new HashSet<Product.Product>();


    }
}
