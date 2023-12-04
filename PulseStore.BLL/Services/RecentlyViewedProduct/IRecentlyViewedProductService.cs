using PulseStore.BLL.Models.Product.UserProductView;
using LanguageExt.Common;

namespace PulseStore.BLL.Services.RecentlyViewedProduct;

public interface IRecentlyViewedProductService
{
    /// <summary>
    ///     Gets the recently viewed products by userId.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>List of user recently viewed products.</returns>
    Task<IEnumerable<UserProductViewDto>> GetByUserIdAsync(Guid userId);

    /// <summary>
    ///     Adds or updates a product view record for a specific user and product.
    /// </summary>
    /// <param name="userId">Id of the user whose product view to add.</param>
    /// <param name="productId">Id of the product that was viewed.</param>
    /// <remarks>
    ///     Checks if product with such id exists.
    /// </remarks>
    /// <returns>
    ///     true if the product view record is successfully added or updated; otherwise false.
    /// </returns>
    Task<Result<bool>> UpsertAsync(Guid userId, int productId);

    /// <summary>
    ///     Adds or updates a product view record with specified userId and data from <see cref="AddUserProductViewDto"/>.
    /// </summary>
    /// <param name="userId">Id of the user whose product view to add.</param>
    /// <param name="userProductView">Entity to add or update.</param>
    /// <remarks>
    ///     Checks if product with such id exists.
    /// </remarks>
    /// <returns>
    ///     true if the product view record is successfully added or updated; otherwise false.
    /// </returns>
    Task<Result<bool>> UpsertAsync(Guid userId, AddUserProductViewDto userProductView);

    /// <summary>
    ///     Adds or updates list of <see cref="AddUserProductViewDto"/> entities with specified userId to repository.
    /// </summary>
    /// <param name="userProductViews">Recently viewed products to add.</param>
    /// <param name="userId">Id of user which recently viewed products to add.</param>
    /// <returns>
    ///     true if the product view record is successfully added; otherwise false.
    /// </returns>
    Task<bool> UpsertManyAsync(IEnumerable<AddUserProductViewDto> userProductViews, Guid userId);
}