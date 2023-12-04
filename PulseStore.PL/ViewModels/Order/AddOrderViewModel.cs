using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.PL.ViewModels.Order;

public record AddOrderViewModel(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string DeliveryAddress,
    PaymentMethod PaymentMethod,
    IEnumerable<AddOrderProductViewModel> OrderProducts);