using PulseStore.BLL.Entities;

namespace PulseStore.BLL.Repositories;

public interface ICategoryRepository
{
    /// <summary>
    ///     Gets <see cref="Category"/> entity by id.
    /// </summary>
    /// <param name="id">Id of the category.</param>
    /// <returns>
    ///     <see cref="Category"/> entity.
    /// </returns>
    Task<Category?> GetByIdAsync(int id);

    /// <summary>
    ///     Gets all <see cref="Category"/> entities.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="Category"/> entities.
    /// </returns>
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetCategoryByName(string? name);
    
    Task<int> CreateAsync(Category category);

    /// <summary>
    ///     Gets all <see cref="Category"/> entities with additional information.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="Category"/> entities.
    /// </returns>
    Task<IEnumerable<Category>> GetAllExtendedAsync();

    /// <summary>
    ///     Deletes <see cref="Category"/> entities by id.
    /// </summary>
    /// <param name="ids">Category ids to delete.</param>
    /// <returns>
    ///     true - if entities have just been deleted or they don't exist
    /// </returns>
    Task<bool> DeleteCategoriesByIdsAsync(int[] ids);

    Task EditAsync(Category category);
}