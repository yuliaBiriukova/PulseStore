using System.ComponentModel.DataAnnotations;
using PulseStore.BLL.Models.Category;
using PulseStore.BLL.Result;

namespace PulseStore.BLL.Services.Category;

public interface ICategoryService
{
    /// <summary>
    ///     Gets <see cref="CategoryDto"/> entity by id.
    /// </summary>
    /// <param name="id">Id of the category.</param>
    /// <returns>
    ///     <see cref="CategoryDto"/> entity.
    /// </returns>
    Task<CategoryDto> GetByIdAsync(int id);

    /// <summary>
    ///     Gets all <see cref="CatalogCategoryDto"/> entities.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="CatalogCategoryDto"/> entities.
    /// </returns>
    Task<IEnumerable<CatalogCategoryDto>> GetAllAsync();

    /// <summary>
    ///     Gets all <see cref="CategoryExtendedDto"/> entities.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="CategoryExtendedDto"/> entities.
    /// </returns>
    Task<IEnumerable<CategoryExtendedDto>> GetAllExtendedAsync();

    /// <summary>
    ///     Deletes <see cref="Category"/> entities by id.
    /// </summary>
    /// <param name="ids">Category ids to delete.</param>
    /// <returns>
    ///     true - if entities have just been deleted or they don't exist
    /// </returns>
    Task<bool> DeleteCategoriesByIdsAsync(int[] ids);
    
    Task<Result<int, string>> CreateAsync(Entities.Category category);
    Task<Result<string, ValidationException>> EditAsync(int id, Entities.Category category);
}