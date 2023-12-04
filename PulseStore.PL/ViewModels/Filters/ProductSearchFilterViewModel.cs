namespace PulseStore.PL.ViewModels.Filters;

public record ProductSearchFilterViewModel(
    int PageNumber,
    int PageSize,
    string SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy,
    string? SortOrder): PaginationFilterViewModel(PageNumber, PageSize);