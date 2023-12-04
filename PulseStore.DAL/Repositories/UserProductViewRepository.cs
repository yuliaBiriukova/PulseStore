using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using LanguageExt.Pipes;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class UserProductViewRepository : IUserProductViewRepository
{
    private readonly PulseStoreContext _dbContext;

    public UserProductViewRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddUserProductViewAsync(UserProductView userProductView)
    {
        _dbContext.UserProductViews.Add(userProductView);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> CheckUserProductViewExistsAsync(Guid userId, int productId)
    {
        return await _dbContext.UserProductViews.AnyAsync(up => up.UserId == userId && up.ProductId == productId);
    }

    public async Task<bool> DeleteManyAsync(Guid userId, IEnumerable<int> productIds)
    {
        await _dbContext.UserProductViews
            .Where(up => up.UserId == userId && productIds.Contains(up.ProductId))
            .ExecuteDeleteAsync();
        return true;
    }

    public async Task<IEnumerable<UserProductView>> GetUserProductViewsByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserProductViews
            .Include(up => up.Product)
            .ThenInclude(p => p.ProductPhotos)
            .Where(up => up.UserId == userId && up.Product.ProductPhotos.Any() && up.Product.IsPublished)
            .OrderByDescending(up => up.ViewedAt)
            .ToListAsync();
    }

    public async Task<bool> UpdateUserProductViewAsync(Guid userId, int productId, DateTime viewedAt)
    {
        var userProductView = new UserProductView() { UserId = userId, ProductId = productId, ViewedAt = viewedAt };
        _dbContext.Attach(userProductView);
        _dbContext.Entry(userProductView).Property(up => up.ViewedAt).IsModified = true;
        return await _dbContext.SaveChangesAsync() > 0;
    }
}