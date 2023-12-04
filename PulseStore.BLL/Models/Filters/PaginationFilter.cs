namespace PulseStore.BLL.Models.Filters;

public record PaginationFilter(
    int PageNumber,
    int PageSize);