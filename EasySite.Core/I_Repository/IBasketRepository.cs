
using EasySite.Core.Entites.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.I_Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> CteateOrUpdateAsync(CustomerBasket Basket);
        Task<CustomerBasket?> GetBasketAsync(string BasketId);
        Task<bool> DeleteBasketAsync(string BasketId);

    }
}
