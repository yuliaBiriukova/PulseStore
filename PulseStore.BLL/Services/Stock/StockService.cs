using System.ComponentModel.DataAnnotations;
using AutoMapper;
using PulseStore.BLL.Models.Category;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Models.Stock;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Result;

namespace PulseStore.BLL.Services.Stock;

public class StockService : IStockService
{
    private readonly IMapper _mapper;
    private readonly IStockRepository _stockRepository;

    public StockService(IMapper mapper, IStockRepository stockRepository)
    {
        _mapper = mapper;
        _stockRepository = stockRepository;
    }

    public async Task<IEnumerable<StockDto>> GetAllAsync()
    {
        var stocks = await _stockRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StockDto>>(stocks);
    }
}