using PulseStore.BLL.Entities;

namespace PulseStore.BLL.Repositories;

public interface IUserCartProductRepository
{
    /// <summary>
    ///     Adds <see cref="UserCartProduct"/> entity to database.
    /// </summary>
    /// <param name="userCartProduct"><see cref="UserCartProduct"/> entity to add.</param>
    /// <returns>
    ///     Id of the added entity.
    /// </returns>
    Task<int> AddAsync(UserCartProduct userCartProduct);

    /// <summary>
    ///     Checks if <see cref="UserCartProduct"/> entity with specified id, productId and userId exists.
    /// </summary>
    /// <param name="id">Id of entity to check.</param>
    /// <param name="productId">Id of product in cart to check.</param>
    /// <param name="userId">Id of user whose cart to check.</param>
    /// <returns>
    ///      <list>
    ///         <item>
    ///             true if entity with specified id, productId and userId exists.
    ///         </item>
    ///         <item>
    ///             false if entity with specified id, productId and userId does not exist.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<bool> CheckUserCartProductExistsAsync(int id, int productId, Guid userId);

    /// <summary>
    ///     Deletes all <see cref="UserCartProduct"/> entities by userId.
    /// </summary>
    /// <param name="userId">Id of user whose cart products to delete.</param>
    /// <returns>
    ///     Amount of deleted entities. 
    /// </returns>
    Task<int> DeleteAllAsync(Guid userId);

    /// <summary>
    ///     Deletes <see cref="UserCartProduct"/> entity by productId and userId.
    /// </summary>
    /// <param name="productId">Id of product to delete from cart.</param>
    /// <param name="userId">Id of user from whose cart to delete.</param>
    /// <returns>
    ///     <list>
    ///         <item>
    ///             1 if entity was deleted.
    ///         </item>
    ///         <item>
    ///             0 if entity was not deleted.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<int> DeleteAsync(int productId, Guid userId);

    /// <summary>
    ///     Gets <see cref="UserCartProduct"/> entities by userId.
    /// </summary>
    /// <param name="userId">Id of user whose cart products to get.</param>
    /// <returns>
    ///     List of <see cref="UserCartProduct"/> entities.
    /// </returns>
    Task<IEnumerable<UserCartProduct>> GetAsync(Guid userId);

    /// <summary>
    ///     Gets <see cref="UserCartProduct"/> entity by productId and userId.
    /// </summary>
    /// <param name="productId">Id of product which chould be in cart.</param>
    /// <param name="userId">Id of user whose cart to get.</param>
    /// <returns>
    ///     <see cref="UserCartProduct"/> entity.
    /// </returns>
    Task<UserCartProduct?> GetByProductIdAsync(int productId, Guid userId);

    /// <summary>
    ///     Updates <see cref="UserCartProduct"/> entity in database.
    /// </summary>
    /// <param name="userCartProduct"><see cref="UserCartProduct"/> entity to update.</param>
    /// <returns>
    ///     <list>
    ///         <item>
    ///             1 if entity was updated.
    ///         </item>
    ///         <item>
    ///             0 if entity was not updated.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<int> UpdateAsync(UserCartProduct userCartProduct);
}