using Core.Entites;

namespace EasySite.DTOs.productDTO.ToReturn
{
    public class UpSalleToReturn
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int TotlePrice { get; set; }
        public string BuyingText { get; set; }
        public bool ShowUnitPrice { get; set; }

        public Decimal UnitPrice { get; set; }

    }
}
