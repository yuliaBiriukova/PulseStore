using PulseStore.BLL.Entities;

namespace PulseStore.BLL.Repositories;

public interface IStockProductRepository
{
    /// <summary>
    ///     Gets quantity of products with the specified id in all stocks.
    /// </summary>
    /// <param name="productId" >Id of the product which quantity to get.</param>
    /// <returns>
    ///     Quantity of products with the specified id in all stocks.
    /// </returns>
    Task<int> GetProductQuantityAsync(int productId);

    /// <summary>
    ///     Gets quantities of products with the specified ids in all stocks.
    /// </summary>
    /// <param name="productIds">List of product ids which quantities to get.</param>
    /// <returns>
    ///     Key-value pairs of product id and quantity of product in all stocks.
    /// </returns>
    Task<Dictionary<int,int>> GetProductsQuantitiesInAllStocksAsync(IEnumerable<int> productIds);

    /// <summary>
    ///     Gets quantities of products with the specified ids in stock with specified id.
    /// </summary>
    /// <param name="productIds">List of product ids which quantities to get.</param>
    /// <param name="stockId">Id of stock which products quantities to get.</param>
    /// <returns>
    ///     Key-value pairs of product id and quantity of product in in stock with specified id.
    /// </returns>
    Task<Dictionary<int, int>> GetProductsQuantitiesInStockAsync(IEnumerable<int> productIds, int stockId);

    /// <summary>
    ///     Add some quantity of product on one of existing stocks.
    /// </summary>
    /// <param name="productId">Id of the product to add.</param>
    /// <param name="stockId">Id of the stock where to add.</param>
    /// <param name="quantity">Desired quantity.</param>
    Task<bool> PutProductInStockAsync(int productId, int stockId, int quantity);

    Task<Stock> GetMainStock();
}