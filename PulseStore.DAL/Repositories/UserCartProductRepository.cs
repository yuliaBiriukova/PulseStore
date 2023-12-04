using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class UserCartProductRepository : IUserCartProductRepository
{
    private readonly PulseStoreContext _dbContext;

    public UserCartProductRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(UserCartProduct userCartProduct)
    {
        _dbContext.UserCartProducts.Add(userCartProduct);
        await _dbContext.SaveChangesAsync();
        return userCartProduct.Id;
    }

    public async Task<bool> CheckUserCartProductExistsAsync(int id, int productId, Guid userId)
    {
        return await _dbContext.UserCartProducts
            .AnyAsync(c => c.Id == id && c.ProductId == productId && c.UserId == userId);
    }

    public async Task<int> DeleteAllAsync(Guid userId)
    {
        return await _dbContext.UserCartProducts
            .Where(ucp => ucp.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task<int> DeleteAsync(int productId, Guid userId)
    {
        return await _dbContext.UserCartProducts
            .Where(ucp => ucp.ProductId == productId && ucp.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<UserCartProduct>> GetAsync(Guid userId)
    {
        return await _dbContext.UserCartProducts
            .Include(c => c.Product)
            .ThenInclude(p => p.ProductPhotos)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<UserCartProduct?> GetByProductIdAsync(int productId, Guid userId)
    {
        return await _dbContext.UserCartProducts
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);
    }

    public async Task<int> UpdateAsync(UserCartProduct userCartProduct)
    {
        var userCartProductExists = await CheckUserCartProductExistsAsync(userCartProduct.Id, userCartProduct.ProductId, userCartProduct.UserId);
        if(userCartProductExists)
        {
            _dbContext.Attach(userCartProduct);
            _dbContext.Entry(userCartProduct).Property(c => c.Quantity).IsModified = true;
        }
        return await _dbContext.SaveChangesAsync();
    }
}