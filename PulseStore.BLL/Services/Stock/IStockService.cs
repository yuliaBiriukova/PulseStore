using PulseStore.BLL.Models.Stock;

namespace PulseStore.BLL.Services.Stock;

public interface IStockService
{
    /// <summary>
    ///     Gets all <see cref="StockDto"/> entities.
    /// </summary>
    /// <returns>
    ///     List of all <see cref="StockDto"/> entities.
    /// </returns>
    Task<IEnumerable<StockDto>> GetAllAsync();
}