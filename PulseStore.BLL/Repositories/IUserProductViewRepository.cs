using PulseStore.BLL.Entities;

namespace PulseStore.BLL.Repositories;

public interface IUserProductViewRepository
{
    /// <summary>
    /// Adds a recently viewed product record.
    /// </summary>
    /// <param name="userProductView">The recently viewed product record to add.</param>
    /// <returns>True if the record was added successfully; otherwise, false.</returns>
    Task<bool> AddUserProductViewAsync(UserProductView userProductView);

    /// <summary>
    ///     Checks if <see cref="UserProductView"/> entity with specified userId and productId exists.
    /// </summary>
    /// <param name="userId">Id of user whose recently viewed product to check.</param>
    /// <param name="productId">Id of product to check.</param>
    /// <returns>
    ///     true if entity with specified userId and productId exists, otherwise false.
    /// </returns>
    Task<bool> CheckUserProductViewExistsAsync(Guid userId, int productId);

    /// <summary>
    ///     Deletes <see cref="UserProductView"/> entities by userId and productId from list of productIds.
    /// </summary>
    /// <param name="userId">Id of user whose recently viewed product to delete.</param>
    /// <param name="productIds">List of product ids to delete.</param>
    /// <returns>
    ///     true if entities with specified userId and productIds were deleted, otherwise false.
    /// </returns>
    Task<bool> DeleteManyAsync(Guid userId, IEnumerable<int> productIds);

    /// <summary>
    /// Gets the recently viewed products by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of recently viewed products.</returns>
    Task<IEnumerable<UserProductView>> GetUserProductViewsByUserIdAsync(Guid userId);

    /// <summary>
    /// Updates a recently viewed product record.
    /// </summary>
    /// <param name="userId">Id of user whose product view to update.</param>
    /// <param name="productId">Id of product which is viewed.</param>
    /// <param name="viewedAt">Date and time of viewing.</param>
    /// <returns>True if the record was updated successfully; otherwise, false.</returns>
    Task<bool> UpdateUserProductViewAsync(Guid userId, int productId, DateTime viewedAt);
}