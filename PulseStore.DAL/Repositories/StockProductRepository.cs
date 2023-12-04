using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class StockProductRepository : IStockProductRepository
{
    private readonly PulseStoreContext _dbContext;

    public StockProductRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetProductQuantityAsync(int productId)
    {
        return await _dbContext.StockProducts
            .Where(sp => sp.ProductId == productId)
            .SumAsync(sp => sp.Quantity);
    }

    public async Task<Dictionary<int, int>> GetProductsQuantitiesInAllStocksAsync(IEnumerable<int> productIds)
    {
        return await _dbContext.StockProducts
           .Where(sp => productIds.Contains(sp.ProductId))
           .GroupBy(sp => sp.ProductId)
           .Select(group => new
           {
               ProductId = group.Key,
               MaxQuantity = group.Sum(sp => sp.Quantity)
           })
           .ToDictionaryAsync(x => x.ProductId, x => x.MaxQuantity);
    }

    public async Task<Dictionary<int, int>> GetProductsQuantitiesInStockAsync(IEnumerable<int> productIds, int stockId)
    {
        return await _dbContext.StockProducts
            .Where(sp => productIds.Contains(sp.ProductId) && sp.StockId == stockId)
            .ToDictionaryAsync(
                sp => sp.ProductId,
                sp => sp.Quantity
            );
    }

    public async Task<bool> PutProductInStockAsync(int productId, int stockId, int quantity)
    {
        var stock = await _dbContext.Stocks.Where(s => s.Id == stockId).FirstOrDefaultAsync();
        if (stock == null)
        {
            return false;
        }

        var stockProduct = await _dbContext.StockProducts.Where(s => s.StockId == stockId && s.ProductId == productId).FirstOrDefaultAsync();
        if (stockProduct == null)
        {
            stockProduct = new StockProduct()
            {
                ProductId = productId,
                Quantity = quantity,
                StockId = stockId
            };
            _dbContext.Add(stockProduct);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            stockProduct.Quantity += quantity;
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    public async Task<Stock> GetMainStock()
    {
        return await _dbContext.Stocks.FirstOrDefaultAsync();
    }
}