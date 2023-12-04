using PulseStore.BLL.Entities;

namespace PulseStore.BLL.Repositories;

public interface IStockRepository
{
    /// <summary>
    ///     Gets all <see cref="Stock"/> entities.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="Stock"/> entities.
    /// </returns>
    Task<IEnumerable<Stock>> GetAllAsync();

    /// <summary>
    ///     Gets ids of all <see cref="Stock"/> entities.
    /// </summary>
    /// <returns>
    ///     List of ids of all <see cref="Stock"/> entities.
    /// </returns>
    Task<IEnumerable<int>> GetAllIdsAsync();

    /// <summary>
    ///     Gets <see cref="Stock"/> entities with specified ids.
    /// </summary>
    /// <param name="stockIds">List of stock ids which to get.</param>
    /// <returns>
    ///     List of <see cref="Stock"/> entities with specified ids.
    /// </returns>
    Task<IEnumerable<Stock>> GetByIdsAsync(IEnumerable<int> ids);
}