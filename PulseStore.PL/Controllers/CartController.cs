using AutoMapper;
using PulseStore.BLL.Models.Cart;
using PulseStore.BLL.Services.UserCartProducts;
using PulseStore.PL.Extensions;
using PulseStore.PL.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAuthorizedUser")]
    public class CartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICartService _userCartProductService;

        public CartController(IMapper mapper, ICartService userCartProductService)
        {
            _mapper = mapper;
            _userCartProductService = userCartProductService;
        }

        /// <summary>
        ///     Gets cart products by userId.
        /// </summary>
        /// <returns>
        ///     HTTP 200 OK with a list of user cart products items 
        /// </returns>
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<UserCartProductViewModel>>> GetCartProducts()
        {
            var userId = User.GetUserId();
            var userCartProducts = await _userCartProductService.GetAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserCartProductViewModel>>(userCartProducts));
        }

        /// <summary>
        ///     Adds list of cart products with userId and data from <see cref="AddCartProductViewModel"/>.
        /// </summary>
        /// <param name="model">List of <see cref="AddCartProductViewModel"/> entities to add.</param>
        /// <remarks>
        ///     Requires an authorized user.
        /// </remarks>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK with list of ids of the added entities if addition succeded.
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if addition failed.
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpPost("products")]
        public async Task<ActionResult<IEnumerable<int>>> AddManyCartProducts([FromBody] IEnumerable<AddCartProductViewModel> model)
        {
            var userId = User.GetUserId();
            var newUserCartProducts = _mapper.Map<IEnumerable<AddUserCartProductDto>>(model);
            var result = await _userCartProductService.AddManyAsync(newUserCartProducts, userId);
            return result.Match<ActionResult<IEnumerable<int>>>(
                value => Ok(value),
                err => BadRequest(err.Message));
        }

        /// <summary>
        ///     Adds cart product with userId and data from <see cref="AddCartProductViewModel"/>.
        /// </summary>
        /// <param name="model"><see cref="AddCartProductViewModel"/> entity to add.</param>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK with Id of the added entity if addition succeded.
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if addition failed.
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpPost("products/{productId}")]
        public async Task<ActionResult<int>> AddCartProduct(int productId, [FromBody] AddCartProductViewModel model)
        {
            var userId = User.GetUserId();
            var newUserCartProduct = _mapper.Map<AddUserCartProductDto>(model);
            var result = await _userCartProductService.UpsertAsync(newUserCartProduct, userId);
            return result.Match<ActionResult<int>>(
                value => Ok(value),
                err => BadRequest(err.Message));
        }

        /// <summary>
        ///     Updates cart product with userId and data from <see cref="UpdateUserCartProductViewModel"/>.
        /// </summary>
        /// <param name="model"><see cref="UpdateUserCartProductViewModel"/> entity to add.</param>
        /// <returns>
        ///     <list>
        ///         <item>
        ///             HTTP 200 OK with amount of rows updated if update succeded.
        ///         </item>
        ///         <item>
        ///             HTTP 400 BadRequest if update failed.
        ///         </item>
        ///     </list>
        /// </returns>
        [HttpPatch("products/{id}")]
        public async Task<ActionResult> UpdateCartProduct(int id, [FromBody] UpdateUserCartProductViewModel model)
        {
            var userId = User.GetUserId();
            var updatedUserCartProduct = _mapper.Map<UpdateUserCartProductDto>(model);
            var result = await _userCartProductService.UpdateAsync(updatedUserCartProduct, userId);
            return result.Match<ActionResult>(
                value => Ok($"{value} rows updated."),
                err => BadRequest(err.Message));
        }

        /// <summary>
        ///     Deletes cart product by productId.
        /// </summary>
        /// <param name="productId">Id of product to delete from cart.</param>
        /// <returns>
        ///     HTTP 200 OK with amount of rows deleted.
        /// </returns>
        [HttpDelete("products/{productId}")]
        public async Task<ActionResult> DeleteCartProduct(int productId)
        {
            var userId = User.GetUserId();
            var deletedRowsCount = await _userCartProductService.DeleteAsync(productId, userId);
            return Ok($"{deletedRowsCount} rows deleted.");
        }

        /// <summary>
        /// Deletes all products from the user's cart.
        /// </summary>
        [HttpDelete("products")]
        public async Task<ActionResult> DeleteAllCartProducts()
        {
            var userId = User.GetUserId();
            await _userCartProductService.DeleteAllItemsAsync(userId);

            return Ok($"All products from cart were deleted!");
        }
    }
}