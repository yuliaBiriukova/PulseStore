using PulseStore.BLL.Models.StockProduct;

namespace PulseStore.BLL.Services.StockProducts
{
    public interface IStockProductService
    {
        Task<IEnumerable<StockProductQuantityDto>> GetProductsQuantitiesInAllStocksAsync(IEnumerable<int> productIds);
    }
}