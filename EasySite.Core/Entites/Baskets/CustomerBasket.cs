using EasySite.Core.Entites.Order;


namespace EasySite.Core.Entites.Baskets
{
    public class CustomerBasket
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString(); 
        public List<BasketItem>? BasketItems { get; set; }
        public Double TotalPrice { get; set; }

    }
}
