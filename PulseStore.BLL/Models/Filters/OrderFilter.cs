using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Filters;

public record OrderFilter(
    int PageNumber,
    int PageSize,
    DateTime? MinDate,
    DateTime? MaxDate,
    string? SortBy,
    string? SortOrder,
    IEnumerable<OrderStatus>? OrderStatuses
    ) : PaginationFilter(PageNumber, PageSize);