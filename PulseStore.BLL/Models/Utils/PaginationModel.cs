namespace PulseStore.BLL.Models.Utils;

public record PaginationModel<T> (
    int PageNumber,
    int PageSize,
    int TotalItems,
    IEnumerable<T> Items)
{
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;
}