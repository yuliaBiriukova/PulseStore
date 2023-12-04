using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Order.AdminOrder;

public record AdminOrderDto(
    int Id,
    DateTime DateCreated,
    string Email,
    string PhoneNumber,
    string DeliveryAddress,
    OrderStatus OrderStatus)
{
    public int ItemsAmount { get; set; }
    public decimal Sum { get; set; }
}