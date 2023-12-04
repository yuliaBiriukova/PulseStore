using PulseStore.BLL.Entities;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.BLL.Repositories;

public interface ISearchHistoryRepository
{
    /// <summary>
    ///     Adds <see cref="SearchHistoryItem"/> to database.
    /// </summary>
    /// <param name="searchHistoryItem">New <see cref="SearchHistoryItem"/> entity.</param>
    /// <returns>
    ///     Id of the added <see cref="SearchHistoryItem"/> entity.
    /// </returns>
    Task<int> AddAsync(SearchHistoryItem searchHistoryItem);

    /// <summary>
    ///     Deletes entity from SearchHistory by id.
    /// </summary>
    /// <param name="id">Id of entity to delete.</param>
    /// <returns>
    ///     <see cref="Task"/>
    /// </returns>
    Task DeleteAsync(int id);

    /// <summary>
    ///     Deletes all entities with specified userId from SearchHistory.
    /// </summary>
    /// <param name="userId">User Id by which delete entities from SearchHistory.</param>
    /// <returns>
    ///     <see cref="Task"/>
    /// </returns>
    Task DeleteAllAsync(string userId);

    /// <summary>
    ///     Gets <see cref="SearchHistoryItem"/> by userId and query.
    /// </summary>
    /// <param name="userId">Id of user whose sreach history item to get.</param>
    /// <param name="query">Search query to get.</param>
    /// <returns>
    ///     <see cref="SearchHistoryItem"/> entity with specified userId and query is such exists; otherwise null.
    /// </returns>
    Task<SearchHistoryItem?> GetAsync(string userId, string query);

    /// <summary>
    ///     Gets <see cref="IEnumerable{T}"/> with <see cref="SearchHistoryItem"/> entities by userId.
    /// </summary>
    /// <param name="userId">User Id by which select <see cref="SearchHistoryItem"/>.</param>
    /// <returns>
    ///     List of <see cref="SearchHistoryItem"/> entities by userId.
    /// </returns>
    Task<IEnumerable<SearchHistoryItem>> GetAsync(string userId);

    /// <summary>
    ///     Updates date of <see cref="SearchHistoryItem"/>.
    /// </summary>
    /// <param name="searchHistoryItem"><see cref="SearchHistoryItem"/> entity to update.</param>
    /// <returns>
    ///     true if <see cref="SearchHistoryItem"/> entity was updated; otherwise false.
    /// </returns>
    Task<bool> UpdateAsync(SearchHistoryItem searchHistoryItem);
}