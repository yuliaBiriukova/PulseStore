namespace PulseStore.PL.ViewModels.Filters;

public record ProductFilterExtendedViewModel(
    int PageNumber,
    int PageSize,
    int? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinQuantity,
    int? MaxQuantity,
    string? SortBy,
    string? SortOrder,
    int? StockId) : ProductFilterViewModel(PageNumber, PageSize, CategoryId, MinPrice, MaxPrice, SortBy, SortOrder, null);