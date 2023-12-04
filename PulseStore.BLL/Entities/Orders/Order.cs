using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Entities.Orders;

public class Order
{
    public int Id { get; set; }
    public Guid? UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string DeliveryAddress { get; set; }
    public DateTime DateCreated { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
}