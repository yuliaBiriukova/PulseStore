namespace PulseStore.BLL.Models.Filters;

public record ProductFilterExtended(
    int PageNumber,
    int PageSize,
    int? CategoryId,
    bool? IsPublished,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinQuantity,
    int? MaxQuantity,
    string? SortBy,
    string? SortOrder,
    int? StockId) : ProductFilter(PageNumber, PageSize, CategoryId, IsPublished, MinPrice, MaxPrice, SortBy, SortOrder, null);