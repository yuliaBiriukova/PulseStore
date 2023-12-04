namespace PulseStore.BLL.Models.Filters;

public record ProductSearchFilter(
    int PageNumber,
    int PageSize,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy,
    string? SortOrder) : ProductFilter(PageNumber, PageSize, null, null, MinPrice, MaxPrice, SortBy, SortOrder, null);