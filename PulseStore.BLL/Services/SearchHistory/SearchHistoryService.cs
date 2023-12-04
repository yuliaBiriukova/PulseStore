using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.SearchHistory;
using PulseStore.BLL.Repositories;

namespace PulseStore.BLL.Services.SearchHistory;

public class SearchHistoryService : ISearchHistoryService
{
    private readonly IMapper _mapper;
    private readonly ISearchHistoryRepository _searchHistoryRepository;

    public SearchHistoryService(IMapper mapper, ISearchHistoryRepository searchHistoryRepository)
    {
        _mapper = mapper;
        _searchHistoryRepository = searchHistoryRepository;
    }

    public async Task<int> UpsertAsync(AddSearchHistoryItemDto searchHistoryItem)
    {
        var existingSearchHistoryItem = await _searchHistoryRepository.GetAsync(searchHistoryItem.UserId, searchHistoryItem.Query);
        if (existingSearchHistoryItem is not null) 
        {
            existingSearchHistoryItem.Date = searchHistoryItem.Date;
            var isUpdated = await _searchHistoryRepository.UpdateAsync(existingSearchHistoryItem);
            return existingSearchHistoryItem.Id;
        }

        var newSearchHistoryItem = _mapper.Map<SearchHistoryItem>(searchHistoryItem);
        var newItemId = await _searchHistoryRepository.AddAsync(newSearchHistoryItem);

        if(newItemId <= 0)
        {
            return newItemId;
        }

        var searchHistory = await _searchHistoryRepository.GetAsync(searchHistoryItem.UserId);
        var maxItemsCount = 8;
        if (searchHistory.Count() > maxItemsCount)
        {
            var itemToDelete = searchHistory.LastOrDefault();
            if (itemToDelete is not null)
            {
                await _searchHistoryRepository.DeleteAsync(itemToDelete.Id);
            }
        }

        return newItemId;
    }

    public async Task DeleteAllAsync(string userId)
    {
        await _searchHistoryRepository.DeleteAllAsync(userId);
    }

    public async Task DeleteAsync(int id)
    {
        await _searchHistoryRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<SearchHistoryItemDto>> GetAsync(string userId)
    {
        var searchHistory = await _searchHistoryRepository.GetAsync(userId);
        return _mapper.Map<IEnumerable<SearchHistoryItemDto>>(searchHistory);
    }
}