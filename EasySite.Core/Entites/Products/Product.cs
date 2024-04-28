using EasySite.Core.Entites.Product;
using EasySite.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class Product: BaseEntites
    {
        //public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public int Sale { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public string LinkName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool InventoryTracking { get; set; }
        public bool Disableproduct { get; set; }
        public bool IsActive { get; set; }
        public int Count { get; set; }
        public bool SkipBasket { get; set; }
        public string TextBuyButton { get; set; }
        public bool FixedBuydownPage  { get; set; }
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
        public bool ProductSoldOut { get; set; }=false;
        public double Rating { get; set; }
        public int NumberSold { get; set; }

        public int DepartmentId { get; set; }

        //[InverseProperty("Product")]
        public Department Departments { get; set; }


        public int SiteId { get; set; }

        public Site Site { get; set; }


        [InverseProperty("Product")]
        public ICollection<Ratings> Ratings { get; set; } = new HashSet<Ratings>();

        //[InverseProperty("Product")]
        public ICollection<OtherImageOfProduct> OtherImagesOfProduct { get; set; } = new HashSet<OtherImageOfProduct>();


        [InverseProperty("Product")]
        public ICollection<Product_Variants> Product_VariantsS { get; set; } = new HashSet<Product_Variants>();


        //[InverseProperty("Product")]
        public ICollection<ProductData> ProductData { get; set; } = new HashSet<ProductData>();

        public ICollection<UpSell> UpSells { get; set; } = new HashSet<UpSell>();
        public CrossSelling CrossSelling { get; set; }

    }
}
