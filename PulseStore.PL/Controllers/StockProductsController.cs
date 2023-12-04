using AutoMapper;
using PulseStore.BLL.Services.StockProducts;
using PulseStore.PL.ViewModels.StockProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers
{
    [Route("api/stock-products")]
    [ApiController]
    [Authorize(Policy = "RequireAuthorizedUser")]
    public class StockProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStockProductService _stockProductService;

        public StockProductsController(IMapper mapper, IStockProductService stockProductService)
        {
            _mapper = mapper;
            _stockProductService = stockProductService;
        }

        /// <summary>
        ///     Gets quantities by productIds
        /// </summary>
        /// <returns>
        ///     HTTP 200 OK with a list of productId-quantity pair
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockProductQuantityViewModel>>> GetProductsQuantitiesById([FromQuery] int[] productIds)
        {
            var productsQuantities =
                await _stockProductService.GetProductsQuantitiesInAllStocksAsync(productIds);

            return Ok(_mapper.Map<IEnumerable<StockProductQuantityViewModel>>(productsQuantities));
        }
    }
}