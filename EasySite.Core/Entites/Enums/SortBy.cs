using System.Runtime.Serialization;

namespace EasySite.DTOs.HomePage
{
    public enum SortBy
    {
        [EnumMember(Value = "منتجات ذات تخفيض")]
        ShowProductesSale,
        [EnumMember(Value = "احدث المنتجات")]
        LatestProducts,
        [EnumMember(Value = "اغلي المنتجات")]
        ShowExpensiveProducts,
        [EnumMember(Value = "ارخص المنتجات")]
        ShowCheapestProducts,
        [EnumMember(Value = "اعلي تقييمات")]
        HighestRatings,
        [EnumMember(Value = "")]
        NoType

    }
}
