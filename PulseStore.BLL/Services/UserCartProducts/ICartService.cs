using PulseStore.BLL.Models.Cart;
using LanguageExt;
using LanguageExt.Common;

namespace PulseStore.BLL.Services.UserCartProducts;

public interface ICartService
{
    /// <summary>
    ///     Updates the entity if such one exists. Adds a new entity if the entity does not exist.
    /// </summary>
    /// <param name="cartProduct"><see cref="AddUserCartProductDto"/> entity to add.</param>
    /// <param name="userId">Id of user which cart product to add.</param>
    /// <remarks>
    ///     <para>
    ///         Checks if product with such id exists.
    ///     </para>
    ///     <para>
    ///         Checks if product quantity is less of qeual to quantity in stocks.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     <list>
    ///         <item>
    ///             Id of the added entity if model validation succeded.
    ///         </item>
    ///         <item>
    ///             Error if model validation failed.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<Result<int>> UpsertAsync(AddUserCartProductDto userCartProduct, Guid userId);

    /// <summary>
    ///     Adds list of <see cref="AddUserCartProductDto"/> entities with specified userId to repository.
    /// </summary>
    /// <param name="userCartProducts">List of <see cref="AddUserCartProductDto"/> entities to add.</param>
    /// <param name="userId">Id of user which cart products to add.</param>
    /// <remarks>
    ///     <para>
    ///         Checks if product with such id exists.
    ///     </para>
    ///     <para>
    ///         Checks if product quantity is less of qeual to quantity in stocks.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     <list>
    ///         <item>
    ///             List of ids of the added entities if model validation succeded.
    ///         </item>
    ///         <item>
    ///             Error if model validation failed.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<Result<IEnumerable<int>>> AddManyAsync(IEnumerable<AddUserCartProductDto> userCartProducts, Guid userId);

    /// <summary>
    ///     Deletes entity with specified productId and userId from repository.
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
    /// Deletes all items for a specific user from the cart.
    /// </summary>
    /// <param name="userId">Id of the user whose cart items will be deleted.</param>
    Task DeleteAllItemsAsync(Guid userId);

    /// <summary>
    ///     Gets <see cref="UserCartProductDto"/> entities by userId.
    /// </summary>
    /// <param name="userId">Id of user whose cart products to get.</param>
    /// <returns>
    ///     List of <see cref="UserCartProductDto"/> entities.
    /// </returns>
    Task<IEnumerable<UserCartProductDto>> GetAsync(Guid userId);

    /// <summary>
    ///     Deletes entity with specified id from repository.
    /// </summary>
    /// <param name="userCartProduct"><see cref="UpdateUserCartProductDto"/> entity to update.</param>
    /// <param name="userId">Id of user which cart product to update.</param>
    /// <remarks>
    ///     <para>
    ///         Checks if product with such id exists.
    ///     </para>
    ///     <para>
    ///         Checks if product quantity is less of qeual to quantity in stocks.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     <list>
    ///         <item>
    ///             1 if entity was updated and model validation succeded.
    ///         </item>
    ///         <item>
    ///             0 if entity was not updated and model validation succeded.
    ///         </item>
    ///         <item>
    ///             Error if model validation failed.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<Result<int>> UpdateAsync(UpdateUserCartProductDto userCartProduct, Guid userId);
}