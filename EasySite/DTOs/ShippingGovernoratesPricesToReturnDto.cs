using Core.Entites;

namespace EasySite.DTOs
{
    public class ShippingGovernoratesPricesToReturnDto:BaseEntites
    {
        public int SiteId { get; set; }
        public string GovernorateName { get; set; }
        public int Price { get; set; }
        public bool IsShippingDuration { get; set; }
        public int? ShippingDuration { get; set; }
    }
}
