using Core.Entites;

namespace EasySite.DTOs.productDTO
{
    public class UpSalleDto : BaseEntites
    {
        public int Count { get; set; }
        public int TotlePrice { get; set; }
        public string BuyingText { get; set; }
        public bool ShowUnitPrice { get; set; }
        //public int ProductId { get; set; }
    }
}
