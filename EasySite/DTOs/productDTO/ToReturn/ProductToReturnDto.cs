using Core.Entites;
using EasySite.Core.Entites.Product;

namespace EasySite.DTOs.productDTO.ToReturn
{
    public class ProductToReturnDto 
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public int Sale { get; set; }
        public int Rating { get; set; }
        public int NumberSold { get; set; }
        public string Image { get; set; }
        public List<OtherImageOfProduct> OtherImagesOfProduct { get; set; }
        public int Price { get; set; }
        public string LinkName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool InventoryTracking { get; set; }
        public bool Disableproduct { get; set; }
        public bool IsActive { get; set; }
        public int Count { get; set; }
        public bool SkipBasket { get; set; }
        public string TextBuyButton { get; set; }
        public bool FixedBuydownPage { get; set; }
        public bool BuyProductFromPage { get; set; }
        public bool ShowRatings { get; set; }
        public bool FakeVisitor { get; set; }
        public int StartFakeVisitor { get; set; }
        public int EndFakeVisitor { get; set; }
        public bool FakeProduct { get; set; }
        public int CountFakeProduct { get; set; }
        public bool FakeHours { get; set; }
        public int CountFakeHours { get; set; }
        public bool FreeShipping { get; set; }
        public bool HideHeader { get; set; }

        public bool ProductSoldOut { get; set; }
        public List<Product_VariantsToReturnDto> Product_VariantsToReturnDto { get; set; }

        public List<ProductDataToReturnDto> ProductDataToReturnDto { get; set; }

        public List<UpSalleToReturn> UpSalleToReturn { get; set; }

        public CrossSellingToReturnDto CrossSellingToReturnDto { get; set; }
    }
}
