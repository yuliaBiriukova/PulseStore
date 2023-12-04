using AutoMapper;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Services.Products;
using PulseStore.BLL.Services.RecentlyViewedProduct;
using PulseStore.PL.ViewModels.Catalog;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IRecentlyViewedProductService _recentlyViewedProductService;

        public ProductsController(IMapper mapper, IProductService productService, IRecentlyViewedProductService recentlyViewedProductService)
        {
            _mapper = mapper;
            _productService = productService;
            _recentlyViewedProductService = recentlyViewedProductService;
        }

        /// <summary>
        ///     Gets <see cref="CatalogViewModel"/> entity by conditions in <see cref="ProductFilterViewModel"/> with pagination and filters for customer.
        /// </summary>
        /// <param name = "filter" ><see cref="ProductFilterViewModel"/> entity with conditions for product</param>
        /// <remarks>
        ///     Sets <see cref="ProductFilterViewModel.IsPublished"/> = true.
        /// </remarks>
        /// <returns>
        ///     <see cref="CatalogViewModel"/> entity with products with pagination that satisfy the filter and possible filters.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<CatalogViewModel>> GetCatalogProducts([FromQuery] ProductFilterViewModel filterModel)
        {
            filterModel.IsPublished = true;
            var filter = _mapper.Map<ProductFilter>(filterModel);
            var result = await _productService.GetAsync(filter);
            return Ok(_mapper.Map<CatalogViewModel>(result));
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<CatalogViewModel>> GetSearchProducts([FromQuery] ProductSearchFilterViewModel filterModel)
        {
            var filter = _mapper.Map<ProductSearchFilter>(filterModel);
            var result = await _productService.GetProductsSearchAsync(filter);

            return Ok(_mapper.Map<CatalogViewModel>(result));
        }

        /// <summary>
        ///     Get <see cref="CatalogProductViewModel"/> entity by product id.
        /// </summary>
        /// <param name = "productId" ><see cref="int"/> int</param>
        /// <returns>
        ///     <see cref="CatalogProductViewModel"/> entity.
        /// </returns>
        [HttpGet("/api/catalog/products/{productId}")]
        public async Task<ActionResult<CatalogProductViewModel>> GetProductById(int productId)
        {
            var result = await _productService.GetByIdAsync(productId);
            return Ok(_mapper.Map<CatalogProductViewModel>(result));
        }

        /// <summary>
        ///     Get <see cref="IEnumerable{T}"/> with 10 last added <see cref="CatalogProductViewModel"/> entities.
        /// </summary>
        /// <param name = "amount">Amount of recently added products to get.</param>
        /// <returns>
        ///     List of <see cref="CatalogProductViewModel"/> entities.
        /// </returns>
        [HttpGet("recently-added")]
        public async Task<ActionResult<IEnumerable<CatalogProductViewModel>>> GetRecentlyAddedProducts(int amount)
        {
            var result = await _productService.GetRecentlyAddedAsync(amount);
            return Ok(_mapper.Map<IEnumerable<CatalogProductViewModel>>(result));
        }

        /// <summary>
        ///     Get list of cart products with max quantities by ids.
        /// </summary>
        /// <param name="ids">Array of ids by which to get products.</param>
        /// <returns>
        ///     List of <see cref="CartProductInfoViewModel"/> entities.
        /// </returns>
        [HttpGet("cart")]
        public async Task<ActionResult<IEnumerable<CartProductInfoViewModel>>> GetCartProducts([FromQuery] int[] ids)
        {
            var result = await _productService.GetCartProductsByIdsAsync(ids);
            return Ok(_mapper.Map<IEnumerable<CartProductInfoViewModel>>(result));
        }
    }
}