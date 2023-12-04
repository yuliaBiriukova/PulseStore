namespace PulseStore.PL.ViewModels.Filters;

public record ProductFilterViewModel(
    int PageNumber,
    int PageSize,
    int? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SortBy,
    string? SortOrder,
    IEnumerable<int>? Ids) : PaginationFilterViewModel(PageNumber, PageSize)
{
    public bool? IsPublished { get; set; }
}