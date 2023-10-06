using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly IUnitOfWork _uow;

        public CartController(CartService cartService, IUnitOfWork UOW)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _uow = UOW ?? throw new ArgumentNullException(nameof(UOW));
        }

        [HttpGet("current")]
        public async Task<ActionResult<CartResponse>> GetCurrentCart(CancellationToken cancellationToken)
        {
            try
            {
                var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var guid = Guid.Parse(strId!);
                var cart = await _cartService.GetAccountCart(guid, cancellationToken);
                var response = await MakeCartResponse(cart, _uow.ProductRepository, cancellationToken);
                return response;
            }
            catch (Exception)
            {
                return Conflict(new ErrorResponse("Cart Not Found For Indicated Account!"));
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCartItem(
            [FromBody] AddCartItemRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.AddProduct(request.AccountId, request.ProductId, request.Quantity, cancellationToken);
                return Ok();
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Cart Not Found!"));
            }
        }

         [HttpPost("delete")]
        public async Task<IActionResult> DeleteCartItem(CartIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var guid = Guid.Parse(strId!);
                await _cartService.DeleteCartItemById(request.id, guid, cancellationToken);
                return Ok();
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Cart Not Found!"));
            }
        }

        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseCartItemQuantity(CartIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var guid = Guid.Parse(strId!);
                await _cartService.IncreaseCartItemQuantityById(request.id, guid, cancellationToken);
                return Ok();
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Cart Not Found!"));
            }
        }

        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseCartItemQuantity(CartIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var guid = Guid.Parse(strId!);
                await _cartService.DecreaseCartItemQuantityById(request.id, guid, cancellationToken);
                return Ok();
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Cart Not Found!"));
            }
        }



        private async Task<CartResponse> MakeCartResponse(Cart cart, IRepository<Product> repository, CancellationToken cancellationToken)
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));

            if (cart.Items.IsNullOrEmpty())
                return new CartResponse();
            

            var response = new CartResponse();
            foreach (var it in cart.Items!)
            {
                var product = await repository.GetById(it.ProductId, cancellationToken);
                response.Items.Add(new CartItemDTO(it.Id, product.Name, it.Quantity));
            }
            return response;
        }
    }
}
