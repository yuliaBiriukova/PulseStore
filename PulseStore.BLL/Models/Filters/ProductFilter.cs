namespace PulseStore.BLL.Models.Filters;

public record ProductFilter(
    int PageNumber,
    int PageSize,
    int? CategoryId,
    bool? IsPublished,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy,
    string? SortOrder,
    IEnumerable<int>? Ids) : PaginationFilter(PageNumber, PageSize);