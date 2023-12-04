using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class StockRepository : IStockRepository
{
    private readonly PulseStoreContext _dbContext;

    public StockRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await _dbContext.Stocks.ToListAsync();
    }

    public async Task<IEnumerable<int>> GetAllIdsAsync()
    {
        return await _dbContext.Stocks.Select(s => s.Id).ToListAsync();
    }

    public async Task<IEnumerable<Stock>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _dbContext.Stocks
            .Where(s => ids.Contains(s.Id))
            .ToListAsync();
    }
}