using Core.Entites.Product;
using EasySite.Core.Entites.Params;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class ProductSpeification:Spesification<Product>
    {
        // Get Paroduct By Link Name
        public ProductSpeification(int departmentId, string linkName):base(p=>p.DepartmentId==departmentId&& p.LinkName== linkName)
        {
            Includes.Add(p => p.UpSells);
            Includes.Add(p => p.CrossSelling);
            Includes.Add(p=>p.OtherImagesOfProduct);
            Includes.Add(p => p.Ratings);
            IncludeStrings.Add("Product_VariantsS.Product_Variant_OptionsS");
            IncludeStrings.Add("ProductData.product_Variant_Options_ProductData.Product_Variant_Option");
        }

        // Get Paroduct By id
        public ProductSpeification(int ProductId) : base(p => p.Id == ProductId)
        {
            Includes.Add(p => p.UpSells);
            Includes.Add(p => p.CrossSelling);
            Includes.Add(p => p.OtherImagesOfProduct);
            Includes.Add(p => p.Ratings);
            IncludeStrings.Add("Product_VariantsS.Product_Variant_OptionsS");
            IncludeStrings.Add("ProductData.product_Variant_Options_ProductData.Product_Variant_Option");
        }
        // ==> Get All Products
        public ProductSpeification(int siteId, int departmentId , QureyParams productParams) : base(p =>  departmentId != 0 ? p.DepartmentId== departmentId:p.SiteId== siteId&& string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search) )
        {
            Includes.Add(p => p.UpSells);
            Includes.Add(p => p.CrossSelling);
            Includes.Add(p => p.OtherImagesOfProduct);
            Includes.Add(p => p.Ratings);
            IncludeStrings.Add("Product_VariantsS.Product_Variant_OptionsS");
            IncludeStrings.Add("ProductData.product_Variant_Options_ProductData.Product_Variant_Option");

            switch(productParams.Sort)
            {
                case "PriceAsc":
                    AddOrderBy(p=>p.Price);
                    break;
                case "PriceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                case "NameAsc":
                    AddOrderBy(p => p.Name);
                    break;
                case "NameDesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                case "DateCreatedAsc":
                    AddOrderBy(p => p.DateCreated);
                    break;
                case "DateCreatedDesc":
                    AddOrderByDescending(p => p.DateCreated);
                    break;
                case "NumberSoldAsc":
                    AddOrderBy(p => p.NumberSold);
                    break;
                case "NumberSoldDesc":
                    AddOrderByDescending(p => p.NumberSold);
                    break;
                case "RatingAsc":
                    AddOrderBy(p => p.Rating);
                    break;
                case "RatingDesc":
                    AddOrderByDescending(p => p.Rating);
                    break;
                default:
                    AddOrderBy(p => p.Index);
                    break;
            }

            if(productParams.IsPagination)
            {
                AddPagination(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
            }

                
        }

        // ==> Get All Products ==> Sort form End User ====>
        public ProductSpeification(  string sortBy, int depratmentId) : base(p =>  p.DepartmentId == depratmentId)
        {
            Includes.Add(p => p.UpSells);
            Includes.Add(p => p.CrossSelling);
            Includes.Add(p => p.OtherImagesOfProduct);
            Includes.Add(p => p.Ratings);
            IncludeStrings.Add("Product_VariantsS.Product_Variant_OptionsS");
            IncludeStrings.Add("ProductData.product_Variant_Options_ProductData.Product_Variant_Option");

            switch (sortBy)
            {
                case "ShowProductesSale":
                    AddOrderByDescending(p => p.Sale);
                    break;
                case "LatestProducts":
                    AddOrderByDescending(p => p.DateCreated);
                    break;
                case "ShowCheapestProducts":
                    AddOrderBy(p => p.Price);
                    break;
                case "ShowExpensiveProducts":
                    AddOrderByDescending(p => p.Price);
                    break;
                case "HighestRatings":
                    AddOrderByDescending(p => p.Rating);
                    break;
                default:
                    AddOrderBy(p => p.Index);
                    break;
            }

        }


    }
}
