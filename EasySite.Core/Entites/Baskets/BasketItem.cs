namespace EasySite.Core.Entites.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public bool IsUpSalle { get; set; }
        public productDataBasket? productDataBasket { get; set; }

    }
}
