using EasySite.Core.Entites.Products;
using EasySite.DTOs.productDTO.ToReturn;

namespace EasySite.DTOs.productDTO
{
    public class ProductDto
    {

        public string Name { get; set; }
        public string LinkName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? Sale { get; set; }
        public int? Index { get; set; } = 0;
        public int DepartmentId { get; set; }

        public int SiteId { get; set; }
        public IFormFile? MainImageFile { get; set; }
        public List<IFormFile>? OtherImageFile { get; set; }

        public string? MainImage { get; set; }
        public List<string>? OtherImage { get; set; }


        public bool InventoryTracking { get; set; } = false;
        public bool Disableproduct { get; set; } = false;
        public int Count { get; set; }


        public bool IsActive { get; set; } = true;
        public bool SkipBasket { get; set; } = false;
        public string TextBuyButton { get; set; } = "اضغط هنا للشراء";
        public bool FixedBuydownPage { get; set; } = false;
        public bool BuyProductFromPage { get; set; } = false;
        public bool ShowRatings { get; set; } = false;
        public bool FakeVisitor { get; set; } = false;
        public int? StartFakeVisitor { get; set; } = 5;
        public int? EndFakeVisitor { get; set; } = 10;
        public bool FakeProduct { get; set; } = false;
        public int? CountFakeProduct { get; set; } = 10;
        public bool FakeHours { get; set; } = false;
        public int CountFakeHours { get; set; } = 3;
        public bool FreeShipping { get; set; } = false;
        public bool HideHeader { get; set; } = false;


        public List<Product_VariantsDto>? Products_VariantsDto { get; set; }


        public List<ProductDataDto>? ProductsDataDtos { get; set; }

        public List<UpSalleDto>? UpSalleDto { get; set; }

        public CrossSellingDto? CrossSellingDto { get; set; }
    }
}
