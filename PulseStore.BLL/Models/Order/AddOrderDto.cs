using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Order;

public record AddOrderDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string DeliveryAddress,
    PaymentMethod PaymentMethod,
    IEnumerable<AddOrderProductDto> OrderProducts)
{
    public Guid? UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public OrderStatus OrderStatus { get; set; }
};