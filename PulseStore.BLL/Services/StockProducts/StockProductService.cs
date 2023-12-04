using PulseStore.BLL.Models.StockProduct;
using PulseStore.BLL.Repositories;

namespace PulseStore.BLL.Services.StockProducts
{
    public class StockProductService : IStockProductService
    {
        private readonly IStockProductRepository _stockProductRepository;

        public StockProductService(IStockProductRepository stockProductRepository)
        {
            _stockProductRepository = stockProductRepository;
        }

        public async Task<IEnumerable<StockProductQuantityDto>> GetProductsQuantitiesInAllStocksAsync(IEnumerable<int> productIds)
        {
            var productsQuantities = await _stockProductRepository.GetProductsQuantitiesInAllStocksAsync(productIds);

            return productsQuantities.Select(productKeyValuePair =>
                new StockProductQuantityDto(productKeyValuePair.Key, productKeyValuePair.Value));
        }
    }
}