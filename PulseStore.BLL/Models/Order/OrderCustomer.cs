namespace PulseStore.BLL.Models.Order;

public record OrderCustomer(
    string Email)
{
    public string? FullName { get; set; }
}