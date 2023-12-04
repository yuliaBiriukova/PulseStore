using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Models.Catalog;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Utils;
using PulseStore.BLL.Repositories.Security;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories.Security;

public class SecurityUserRepository : ISecurityUserRepository
{
    private readonly PulseStoreContext _dbContext;

    public SecurityUserRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAsync(SecurityUser securityUser)
    {
        _dbContext.Add(securityUser);
        await _dbContext.SaveChangesAsync();
        return securityUser.Id;
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var user = await _dbContext.SecurityUsers.Where(s=>s.Id == userId).FirstOrDefaultAsync();
        if (user != null)
        {
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        else
        {
            return false; 
        }

    }

    public async Task<PaginationModel<SecurityUser>> GetUsersAsync(PaginationFilter filter)
    {
        var query = _dbContext.SecurityUsers
            .Include(s => s.Stocks);

        var totalUsers = await query.CountAsync();

        var users = await query.OrderBy(o => o.Id)
                            .Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize)
                            .ToListAsync();
        var pageNumber = Math.Max(
            1,
            Math.Min(
                filter.PageNumber,
                (int)Math.Ceiling((double)query.Count() / filter.PageSize)
                )
            );
        var paginationModel = new PaginationModel<SecurityUser>(pageNumber, filter.PageSize, totalUsers, users);

        return paginationModel;
    }

    public async Task<int> EditSecurityUser(SecurityUser changedUser)
    {
        var user =  _dbContext.SecurityUsers.Include(s=>s.Stocks).FirstOrDefault(u=>u.Id == changedUser.Id);
        user.Stocks.Clear();
        foreach (var stock in changedUser.Stocks)
        {
            user.Stocks.Add(stock);
        }
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    public async Task<SecurityUser> GetSecurityUser(int id)
    {
        var user = await _dbContext.SecurityUsers.Include(s => s.Stocks).FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }
}
