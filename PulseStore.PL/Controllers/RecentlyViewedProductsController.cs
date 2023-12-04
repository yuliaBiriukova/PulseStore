using AutoMapper;
using PulseStore.BLL.Models.Product.UserProductView;
using PulseStore.BLL.Services.RecentlyViewedProduct;
using PulseStore.PL.Extensions;
using PulseStore.PL.ViewModels.Product.UserProductView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers
{
    [Route("api/products/recently-viewed")]
    [ApiController]
    [Authorize(Policy = "RequireAuthorizedUser")]
    public class RecentlyViewedProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRecentlyViewedProductService _recentlyViewedProductService;

        public RecentlyViewedProductsController(IMapper mapper, IRecentlyViewedProductService recentlyViewedProductService)
        {
            _mapper = mapper;
            _recentlyViewedProductService = recentlyViewedProductService;
        }

        /// <summary>
        /// Retrieves a list of products recently viewed by the authenticated customer.
        /// </summary>
        /// <returns>
        ///    HTTP 200 OK with a list of recently viewed products.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProductViewViewModel>>> GetRecentlyViewedProducts()
        {
            Guid userId = User.GetUserId();
            var result = await _recentlyViewedProductService.GetByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<UserProductViewViewModel>>(result));
        }

        /// <summary>
        ///     Adds or updates a product as recently viewed for the authenticated customer.
        /// </summary>
        /// <param name="productId">The ID of the product to add or update as recently viewed.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item>
        ///       <description>HTTP 200 OK if the product is successfully added or updated.</description>
        ///     </item>
        ///     <item>
        ///       <description>HTTP 400 Bad Request if the addition or update fails.</description>
        ///     </item>
        ///   </list>
        /// </returns>
        [HttpPost("{productId}")]
        public async Task<ActionResult<bool>> AddRecentlyViewedProduct([FromBody] int productId)
        {
            var userId = User.GetUserId();
            var result = await _recentlyViewedProductService.UpsertAsync(userId, productId);

            return result.Match<ActionResult<bool>>(
                value => Ok(value),
                err => BadRequest(err.Message));
        }

        /// <summary>
        ///     Adds list of recently viewed products with userId and data from <see cref="AddUserProductViewViewModel"/>.
        /// </summary>
        /// <param name="model">List of recemtly viewed products to add.</param>
        /// <returns>
        ///     HTTP 200 OK with true if entity was successfully added, otherwise false.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<bool>> AddManyRecentlyViewedProducts([FromBody] IEnumerable<AddUserProductViewViewModel> model)
        {
            var userId = User.GetUserId();
            var newRecentlyViewedProducts = _mapper.Map<IEnumerable<AddUserProductViewDto>>(model);
            var result = await _recentlyViewedProductService.UpsertManyAsync(newRecentlyViewedProducts, userId);
            return Ok(result);
        }
    }
}