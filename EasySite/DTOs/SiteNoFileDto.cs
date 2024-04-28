using Core.Entites;
using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs
{
    public class SiteNoFileDto : BaseEntites
    {
        public string LinkName { get; set; }
        public string? Name { get; set; }
        public string? TitleSite { get; set; } = "خصم 10%";
        public string? ColorTextHedar { get; set; } = "#fff";
        public string? colorSite { get; set; } = "#fd9c03";
        public string Logo { get; set; }
        public string? WhatsApp { get; set; }
        public string? Coll_Phone { get; set; }
        public int? ShippingPrice { get; set; }
        public string? MiniIcon { get; set; }
        public string? FontType { get; set; } = "Almarai";
        public string? Currency { get; set; } = "EgyptianPound";
        public string? HeaderCode { get; set; }
    }
}
