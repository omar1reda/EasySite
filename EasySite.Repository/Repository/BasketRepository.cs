using EasySite.Core.Entites.Baskets;
using EasySite.Core.I_Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySite.Repository.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private IDatabase _redise;
        public BasketRepository(IConnectionMultiplexer Redise)
        {
            _redise = Redise.GetDatabase();
        }


        public async Task<CustomerBasket?> CteateOrUpdateAsync(CustomerBasket Basket)
        {
            var BasketJson = JsonSerializer.Serialize(Basket);
            var createOrOupate = await _redise.StringSetAsync(Basket.Id, BasketJson , TimeSpan.FromDays(1));

            if (!createOrOupate)
                return null;

            return await GetBasketAsync(Basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            var Result =await _redise.KeyDeleteAsync(BasketId);
            return Result;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket =await _redise.StringGetAsync(BasketId);
            if(basket.IsNull)
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<CustomerBasket>(basket);
            }
        }
    }
}
