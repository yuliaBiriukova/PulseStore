using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Order.AdminOrder;

public record AdminOrderDetailsDto(
    int Id,
    OrderStatus OrderStatus,
    string Email,
    string PhoneNumber,
    string DeliveryAddress,
    IEnumerable<AdminOrderProductDto> OrderProducts)
{
    public string? FullName { get; set; }
};