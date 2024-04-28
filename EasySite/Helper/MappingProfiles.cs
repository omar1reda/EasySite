using AutoMapper;
using Core.Entites;
using Core.Entites.Homepage;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Baskets;
using EasySite.Core.Entites.Homepag;
using EasySite.Core.Entites.Order;
using EasySite.Core.Entites.Product;
using EasySite.Core.Entites.Products;
using EasySite.DTOs;
using EasySite.DTOs.HomePage;
using EasySite.DTOs.HomePage.ToReturn;
using EasySite.DTOs.ManagrDto;
using EasySite.DTOs.OrdersDto;
using EasySite.DTOs.productDTO;
using EasySite.DTOs.productDTO.ToReturn;

namespace EasySite.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Site, SiteDto>().ReverseMap();

            CreateMap<SiteNoFileDto,Site>().ReverseMap()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom<ResolveImageInSite>())
                .ForMember(dest => dest.MiniIcon, opt => opt.MapFrom<ResolveImageInSite>());

            CreateMap<SiteDto, SiteNoFileDto>();

            CreateMap<SocialMedia, SocialMediaDto>().ReverseMap();

            CreateMap<Thanks, ThanksDto>().ReverseMap();

            CreateMap<Transactions, TransactionDto>().ReverseMap();
            CreateMap<Transactions, TransactionAllDataDto>().ReverseMap();


            CreateMap<ShippingGovernoratesPrices, ShippingGovernoratesPricesDto>().ReverseMap();
            CreateMap<ShippingGovernoratesPrices, ShippingGovernoratesPricesToReturnDto>().ReverseMap();

            CreateMap<BlockedNumbers, BlockedNumbersDto>().ReverseMap();
            CreateMap<BlockedNumbers, BlockedNumbersToReturnDto>().ReverseMap();

            CreateMap<PageDto, Pages>().ReverseMap();
            CreateMap<Pages, PageToReturnDto>().ReverseMap();


            CreateMap<DepartmentDto, Department>().ReverseMap();
            CreateMap< DepartmentToreturnDto,Department > ().ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom<ResolveImageInDepartment>());


            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductToReturnDto, Product>().ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom<ResolveImageInProduct>());

            CreateMap<Product_Variants, Product_VariantsDto>().ReverseMap();
            CreateMap<Product_Variants, Product_VariantsToReturnDto>().ReverseMap();

            CreateMap<Product_Variant_Options, Product_Variant_OptionsDto>().ReverseMap();
            CreateMap<Product_Variant_Options, Product_Variant_OptionsToReturnDto>().ReverseMap();

            CreateMap<ProductData, ProductDataDto>().ReverseMap();
            CreateMap<ProductData, ProductDataToReturnDto>().ReverseMap();

            CreateMap<UpSell, UpSalleDto>().ReverseMap();
            CreateMap<UpSell, UpSalleToReturn>().ReverseMap();

            CreateMap<OtherImageOfProduct, OtherImageOfProductToReturnDto>().ReverseMap();
            CreateMap<RatingDto, Ratings>().ReverseMap();

            CreateMap<Homepage, HomePageDto>().ReverseMap();
            CreateMap<Homepage, HomePageToreturn>().ReverseMap();

            CreateMap<ProductsInHomePage, ProductsInHomePageDto>().ReverseMap();

            CreateMap<DepartmentsInHomePage, DepartmentsInHomePageDto>().ReverseMap();

            CreateMap<Slider, SliderDto>().ReverseMap();
            CreateMap<Slider, SliderOtReturnDto>().ReverseMap();

            CreateMap<SliderImage, SliderImageDto>().ReverseMap();
            CreateMap<SliderImage, SliderImageToReturnDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<VariantOptionOrder, VariantOptionOrderDto>().ReverseMap();
            CreateMap<BasketItem, OrderItem>().ReverseMap();
            CreateMap<VariantOptionBasket, VariantOptionOrder>().ReverseMap();

            CreateMap<PermitionsDto, Permitions>().ReverseMap();

            CreateMap<CrossSelling, CrossSellingDto>().ReverseMap();
            CreateMap<CrossSelling, CrossSellingToReturnDto>().ReverseMap();
        }
    }
}
