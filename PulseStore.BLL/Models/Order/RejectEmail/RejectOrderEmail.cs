using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Order.Reject;

public record RejectOrderEmail(
    int Id,
    OrderStatus OrderStatus,
    string Email,
    string PhoneNumber,
    string DeliveryAddress,
    IEnumerable<RejectOrderProductDto> OrderProducts)
{
    public string? FullName { get; set; }
    public decimal TotalPrice { get; set; }
    public string? CancelOrderPath { get; set; }
    public string? OrderAvailablePath { get; set; }
};