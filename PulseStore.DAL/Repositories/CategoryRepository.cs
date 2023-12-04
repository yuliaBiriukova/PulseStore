using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly PulseStoreContext _dbContext;

    public CategoryRepository(PulseStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _dbContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryByName(string? name)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IEnumerable<Category>> GetAllExtendedAsync()
    {
        return await _dbContext.Categories
            .Include(c => c.Products)
            .ToListAsync();
    }

    public async Task<bool> DeleteCategoriesByIdsAsync(int[] ids)
    {
        await _dbContext.Categories.Where(c => ids.Contains(c.Id)).ExecuteDeleteAsync();

        return true;
    }

    public async Task EditAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return category.Id;
    }
}