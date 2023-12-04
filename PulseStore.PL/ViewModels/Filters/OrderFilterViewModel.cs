using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.PL.ViewModels.Filters;

public record OrderFilterViewModel(
    int PageNumber,
    int PageSize,
    DateTime? MinDate,
    DateTime? MaxDate,
    string? SortBy,
    string? SortOrder,
    IEnumerable<OrderStatus>? OrderStatuses
    ) : PaginationFilterViewModel(PageNumber, PageSize);