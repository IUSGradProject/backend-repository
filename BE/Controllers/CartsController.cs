
using BLL.Services.Interface;
using DTOs.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("Items")]
        public async Task<ActionResult> CreateCart([FromBody] CartRequest cart)
        {
            await _cartService.CreateCartProducts(cart);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
            return Ok(await _cartService.GetCartItems());
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteCartItem(Guid productId)
        {
            await _cartService.DeleteCartItem(productId);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCart([FromBody] CartItemContract cart)
        {
            await _cartService.CreateCartProduct(cart);
            return Ok();
        }

        [HttpPost("Order")]
        public async Task<ActionResult> CreateOrder([FromBody] CartRequest cart)
        {
            await _cartService.CreateOrder(cart);
            return Ok();
        }

        [HttpGet("Order")]
        public async Task<ActionResult> GetOrders([AsParameters] int pageNumber = 1, int pageSize = 2)
        {
            return Ok(await _cartService.GetOrders(pageNumber, pageSize));
        }
    }
}
