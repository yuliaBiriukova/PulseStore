using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.PL.ViewModels.Order;

public record AdminOrderViewModel(
    int Id,
    DateTime DateCreated,
    string Email,
    string PhoneNumber,
    string DeliveryAddress,
    OrderStatus OrderStatus,
    int ItemsAmount,
    decimal Sum);