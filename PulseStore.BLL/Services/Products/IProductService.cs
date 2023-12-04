using System.ComponentModel.DataAnnotations;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.Catalog;
using PulseStore.BLL.Models.Filters;
using PulseStore.BLL.Models.Product;
using PulseStore.BLL.Result;

namespace PulseStore.BLL.Services.Products;

public interface IProductService
{
    /// <summary>
    ///     Gets <see cref="CatalogModel{T}"/> with <see cref="CatalogProductDto"/> entities by conditions in <see cref="ProductFilter"/> with pagination and possible filters.
    /// </summary>
    /// <param name = "filter" ><see cref="ProductFilter"/> entity with conditions for product</param>
    /// <returns>
    ///     List of <see cref="CatalogProductDto"/> entities, that satisfy the filter, with pagination and possible filters.
    /// </returns>
    Task<CatalogModel<CatalogProductDto>> GetAsync(ProductFilter filter);

    /// <summary>
    ///     Gets <see cref="CatalogModel{T}"/> with <see cref="AdminCatalogProductDto"/> entities by conditions in <see cref="ProductSearchFilter"/> with pagination and possible filters.
    /// </summary>
    /// <param name = "filter" ><see cref="ProductSearchFilter"/> entity with conditions for administrator product search.</param>
    /// <returns>
    ///     List of <see cref="AdminCatalogProductDto"/> entities, that satisfy the filter, with pagination and possible filters.
    /// </returns>
    Task<CatalogModel<AdminCatalogProductDto>> GetAdminProductsSearchAsync(ProductSearchFilter filter);

    Task<CatalogModel<CatalogProductDto>> GetProductsSearchAsync(ProductSearchFilter filter);

    /// <summary>
    ///     Gets <see cref="CatalogModel{T}"/> with <see cref="CatalogProductExtendedDto"/> entities by conditions in <see cref="ProductFilter"/> with pagination and possible filters.
    /// </summary>
    /// <param name = "filter" ><see cref="ProductFilter"/> entity with conditions for product</param>
    /// <returns>
    ///     List of <see cref="CatalogProductExtendedDto"/> entities, that satisfy the filter, with pagination and possible filters.
    /// </returns>
    Task<CatalogModel<CatalogProductExtendedDto>> GetExtendedAsync(ProductFilterExtended filter);

    /// <summary>
    ///     Gets <see cref="Product"/> entities which than add to database.
    /// </summary>
    /// <param name="product"><see cref="Product"/> one of the entities of the database.</param>
    /// <returns>
    ///     Returns <see cref="Product.Id"/> as <see cref="int"/>, of entity that was added
    /// </returns>
    Task<int> CreateAsync(Product product);

    /// <summary>
    ///     Get <see cref="CatalogProductDto"/> entity by product id.
    /// </summary>
    /// <param name = "productId" ><see cref="int"/> int</param>
    /// <returns>
    ///     <see cref="CatalogProductDto"/> entity.
    /// </returns>
    Task<CatalogProductDto> GetByIdAsync(int productId);

    /// <summary>
    ///     Gets <see cref="IEnumerable{T}"/> with 10 last added <see cref="CatalogProductDto"/> entities.
    /// </summary>
    /// <param name = "amount" >Amount of recently added products to get.</param>
    /// <returns>
    ///     List of <see cref="CatalogProductDto"/> entities.
    /// </returns>
    Task<IEnumerable<CatalogProductDto>> GetRecentlyAddedAsync(int amount);

    /// <summary>
    ///     Gets Id as <see cref="int"/> of added product.
    /// </summary>
    /// <param name = "product"> Entity of <see cref="Product"/> to change </param>
    /// <returns>
    ///     Id of updated producr as <see cref="int"/>
    /// </returns>
    Task<string> UpdateAsync(Product product);

    /// <summary>
    ///     Gets <see cref="string"/> with result of Delete.
    /// </summary>
    /// <param name = "ids"> Array of <see cref="int"/> with ids of <see cref="Product"/> which should be deleted</param>
    /// <returns>
    ///     <see cref="string"/> with result of DeleteAsync method.
    /// </returns>
    Task<string> DeleteProducts(int[] ids);

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
    Task<bool> ProductIsPublished(int productId);

    /// <summary>
    ///     Gets list of <see cref="CartProductInfoDto"/> entities by specified ids.
    /// </summary>
    /// <param name="ids">Array of ids by which to get products.</param>
    /// <returns>
    ///     List of cart products with max quantity by specified ids.
    /// </returns>
    Task<IEnumerable<CartProductInfoDto>> GetCartProductsByIdsAsync(int[] ids);

    /// <summary>
    ///     Add some quantity of product on one of existing stocks.
    /// </summary>
    /// <param name="productId">Id of the product to add.</param>
    /// <param name="stockId">Id of the stock where to add.</param>
    /// <param name="quantity">Desired quantity.</param>
    Task<bool> PutInStockAsync(int productId, int stockId, int quantity);

    Task<Product?>GetProductById(int productId);

    Task<Result<string, ValidationException>> MoveToCategoryAsync(ProductMoveCategoryDto productMoveCategoryDto);

    Task<bool> ChangeProductPublishedStatus(int productId, bool isPublished);
}