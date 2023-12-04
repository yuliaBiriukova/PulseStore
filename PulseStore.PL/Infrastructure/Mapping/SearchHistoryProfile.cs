using AutoMapper;
using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.SearchHistory;
using PulseStore.PL.ViewModels.SearchHistory;

namespace PulseStore.PL.Infrastructure.Mapping
{
    public class SearchHistoryProfile : Profile
    {
        public SearchHistoryProfile()
        {
            CreateMap<SearchHistoryItem, SearchHistoryItemDto>();
            CreateMap<SearchHistoryItemDto, SearchHistoryViewModel>();
            CreateMap<AddSearchHistoryViewModel, AddSearchHistoryItemDto>();
            CreateMap<AddSearchHistoryItemDto, SearchHistoryItem>();
        }
    }
}