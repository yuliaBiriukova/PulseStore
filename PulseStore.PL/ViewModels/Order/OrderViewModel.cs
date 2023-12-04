using PulseStore.BLL.Entities.Orders.Enums;
using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.ViewModels.Order;

public record OrderViewModel(
    int Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string DeliveryAddress,
    DateTime DateCreated,
    PaymentMethod PaymentMethod,
    OrderStatus OrderStatus,
    IEnumerable<OrderProductListViewModel> OrderProducts);