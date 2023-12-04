using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class SearchHistoryRepository : ISearchHistoryRepository
{
    private readonly PulseStoreContext _dbContext;

    public SearchHistoryRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(SearchHistoryItem searchHistoryItem)
    {
        _dbContext.SearchHistory.Add(searchHistoryItem);
        await _dbContext.SaveChangesAsync();
        return searchHistoryItem.Id;
    }

    public async Task DeleteAllAsync(string userId)
    {
        await _dbContext.SearchHistory
            .Where(sh => sh.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.SearchHistory
            .Where(sh => sh.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<SearchHistoryItem?> GetAsync(string userId, string query)
    {
        return await _dbContext.SearchHistory.FirstOrDefaultAsync(sh => sh.UserId == userId && sh.Query == query);
    }

    public async Task<IEnumerable<SearchHistoryItem>> GetAsync(string userId)
    {
        return await _dbContext.SearchHistory
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(SearchHistoryItem searchHistoryItem)
    {
        var searchHistoryItemExists = await _dbContext.SearchHistory.AnyAsync(sh => sh.Id == searchHistoryItem.Id);
        if (searchHistoryItemExists)
        {
            _dbContext.Attach(searchHistoryItem);
            _dbContext.Entry(searchHistoryItem).Property(sh => sh.Date).IsModified = true;
        }
        var rowsAffected = await _dbContext.SaveChangesAsync();
        return rowsAffected > 0 || !searchHistoryItemExists;
    }
}
