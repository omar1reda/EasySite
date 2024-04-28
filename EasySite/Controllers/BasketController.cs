using EasySite.Core.Entites.Baskets;
using EasySite.Core.I_Repository;
using EasySite.DTOs.HomePage.ToReturn;
using EasySite.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this._basketRepository = basketRepository;
        }

        [HttpPut("CreateOrUpdateBasket")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult< CustomerBasket>> CreateOrUpdateBasket(CustomerBasket basket)
        {
            var BasketAdded = await _basketRepository.CteateOrUpdateAsync(basket);
            if (BasketAdded == null)
                return BadRequest(new ApiResponse(400, "هناك مشكله في الباسكت"));

            return Ok(BasketAdded);
        }


        [HttpDelete("DeleteBasker")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<bool>> DeleteBasker(string BaskitId)
        {
            var Result =await _basketRepository.DeleteBasketAsync(BaskitId);
            return Result ;
        }

        [HttpGet("GetBasket")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]

        public async Task<ActionResult<CustomerBasket>> GetBasket(string BaskitId)
        {
            var basket = await _basketRepository.GetBasketAsync(BaskitId);
            if (basket == null)
            {
                return BadRequest(new ApiResponse(404, "لا يوجد Basket بهذا ال id"));
            }
            return Ok(basket);  
        }
    }
}
