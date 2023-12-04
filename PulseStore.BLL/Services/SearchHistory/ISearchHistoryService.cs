using PulseStore.BLL.Models.SearchHistory;

namespace PulseStore.BLL.Services.SearchHistory;

public interface ISearchHistoryService
{
    /// <summary>
    ///     Adds or updates search histiry item depending whether it exists.
    /// </summary>
    /// <remarks>
    ///     Deletes the oldest user search history item if search history size exeeds 8.
    /// </remarks>
    /// <param name="searchHistoryItem">New <see cref="AddSearchHistoryItemDto"/> entity.</param>
    /// <returns>
    ///     Id of the added SearchHistory entity.
    /// </returns>
    Task<int> UpsertAsync(AddSearchHistoryItemDto searchHistoryItem);

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
    ///     Gets <see cref="IEnumerable{T}"/> with <see cref="SearchHistoryItemDto"/> entities by userId.
    /// </summary>
    /// <param name="userId">User Id by which select <see cref="SearchHistoryItemDto"/>.</param>
    /// <returns>
    ///     List of <see cref="SearchHistoryItemDto"/> entities by userId.
    /// </returns>
    Task<IEnumerable<SearchHistoryItemDto>> GetAsync(string userId);
}