using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Services.Photo;
using PulseStore.BLL.Services.Products;
using PulseStore.PL.ViewModels.Catalog;
using PulseStore.PL.ViewModels.Filters;
using PulseStore.PL.ViewModels.Product;
using PulseStore.PL.ViewModels.Stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Services.Stock;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin/Stocks")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;

        public StocksController(IMapper mapper, IStockService stockService)
        {
            _mapper = mapper;
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockViewModel>>> GetStocks()
        {
            var result = await _stockService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<StockViewModel>>(result));
        }
    }
}