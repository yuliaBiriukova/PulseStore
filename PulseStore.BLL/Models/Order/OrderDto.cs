using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Order;

public record OrderDto(
    int Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string DeliveryAddress,
    DateTime DateCreated,
    PaymentMethod PaymentMethod,
    OrderStatus OrderStatus,
    IEnumerable<OrderProductDto> OrderProducts);