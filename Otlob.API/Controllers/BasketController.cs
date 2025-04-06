using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.API.Errors;
using Otlob.Core.Models;
using Otlob.Core.Repositories;

namespace Otlob.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository; // Add This Service in ApplicationServicesExtension
        }

        #region EndPoints

        #region GET: BaseUrl/api/Basket
        // Get Or  Recreate Basket
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            return (basket is null) ? new CustomerBasket(basketId) : basket;
        }
        #endregion

        #region POST: BaseUrl/api/Basket
        // Update Or Create New Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var createdOrUpdated = await _basketRepository.UpdateBasketAsync(basket);

            if (createdOrUpdated is null)
                return BadRequest(new ErrorResponse(400));

            return Ok(createdOrUpdated);
        }
        #endregion

        #region DELETE: BaseUrl/api/Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }
        #endregion

        #endregion
    }
}
