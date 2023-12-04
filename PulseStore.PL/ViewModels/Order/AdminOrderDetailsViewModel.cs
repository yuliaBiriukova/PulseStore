using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.PL.ViewModels.Order;

public record AdminOrderDetailsViewModel(
    int Id,
    OrderStatus OrderStatus,
    string FullName,
    string Email,
    string PhoneNumber,
    string DeliveryAddress,
    IEnumerable<AdminOrderProductViewModel> OrderProducts);