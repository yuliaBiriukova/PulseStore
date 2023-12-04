using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Catalog;
using PulseStore.BLL.Models.Filters;

namespace PulseStore.BLL.Repositories;

public interface IProductRepository
{
    /// <summary>
    ///     Gets <see cref="CatalogModel{T}"/> with <see cref="Product"/> entities by conditions in <see cref="ProductFilter"/> with pagination and possible filters.
    /// </summary>
    /// <param name = "filter" ><see cref="ProductFilter"/> entity with conditions for product</param>
    /// <returns>
    ///     List of <see cref="Product"/> entities, that satisfy the filter, with pagination and possible filters.
    /// </returns>
    Task<CatalogModel<Product>> GetAsync(ProductFilter filter);

    Task<CatalogModel<Product>> GetProductsSearchAsync(ProductSearchFilter filter);

    /// <summary>
    ///     Gets <see cref="Product"/> entities which than add to database.
    /// </summary>
    /// <param name="product"><see cref="Product"/> one of the entities of the database.</param>
    /// <returns>
    ///     Returns <see cref="Product.Id"/> as <see cref="int"/>, of entity that was added
    /// </returns>
    Task<int> CreateAsync(Product product);

    /// <summary>
    ///     Get <see cref="Product"/> entity by product id.
    /// </summary>
    /// <param name = "productId" ><see cref="int"/> int</param>
    /// <returns>
    ///     <see cref="Product"/> entity.
    /// </returns>
    Task<Product?> GetByIdAsync(int productId);

    /// <summary>
    ///     Gets <see cref="IEnumerable{T}"/> with 10 last added <see cref="Product"/> entities.
    /// </summary>
    /// <param name = "amount" >Amount of recently added products to get.</param>
    /// <returns>
    ///     List of <see cref="Product"/> entities.
    /// </returns>
    Task<IEnumerable<Product>> GetRecentlyAddedAsync(int amount);

    /// <summary>
    ///     Gets Id as <see cref="int"/> of added product.
    /// </summary>
    /// <param name = "product"> Entity of <see cref="Product"/> to change </param>
    /// <returns>
    ///     Id of updated producr as <see cref="int"/>
    /// </returns>
    Task<int> UpdateAsync(Product product);

    /// <summary>
    ///     Gets <see cref="string"/> with result of Delete.
    /// </summary>
    /// <param name = "ids"> Array of <see cref="int"/> with ids of <see cref="Product"/> which should be deleted</param>
    /// <returns>
    ///     <see cref="string"/> with result of DeleteAsync method.
    /// </returns>
    Task<string> DeleteAsync(int[] ids);

    /// <summary>
    ///     Gets array of <see cref="int"/> which contain Ids of photo that related to <see cref="Product"/>.
    /// </summary>
    /// <param name = "productId" > Id of <see cref="Product"/> which value should be returned.</param>
    /// <returns>
    ///     <see cref="int"/> array with photos id of Product.
    /// </returns>
    Task<int[]> GetAllPhotoesId(int productId);

    /// <summary>
    ///     Gets value of <see cref="Product.IsPublished"/>.
    /// </summary>
    /// <param name = "productId" > Id of <see cref="Product"/> which value should be returned.</param>
    /// <returns>
    ///     <see cref="bool"/> value of property <see cref="Product.IsPublished"/> of <see cref="Product"/> 
    /// </returns>
    Task<bool> IsPublished(int productId);

    /// <summary>
    ///     Checks if <see cref="Product"/> entity with specified id exists.
    /// </summary>
    /// <param name="id">Id of entity to check.</param>
    /// <returns>
    ///      <list>
    ///         <item>
    ///             true if entity with specified id exists.
    ///         </item>
    ///         <item>
    ///             false if entity with specified id does not exist.
    ///         </item>
    ///     </list>
    /// </returns>
    Task<bool> CheckProductExistsAsync(int id);

    /// <summary>
    ///     Gets list of <see cref="Product"/> entities with specified ids.
    /// </summary>
    /// <param name="ids">Array of ids by which to get products.</param>
    /// <returns>
    ///     List of products with specified ids.
    /// </returns>
    Task<IEnumerable<Product>> GetProductsByIdsAsync(int[] ids);

    Task UpdateProductsAsync(List<Product> products);

    Task<bool> ChangeProductPublishedStatus(int productId, bool isPublished);
}