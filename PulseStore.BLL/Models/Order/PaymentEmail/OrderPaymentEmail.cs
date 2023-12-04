using PulseStore.BLL.Entities.Orders.Enums;

namespace PulseStore.BLL.Models.Order.PaymentEmail;

public record OrderPaymentEmail(
    int Id,
    OrderStatus OrderStatus,
    string Email,
    string PhoneNumber,
    string DeliveryAddress,
    IEnumerable<PaymentOrderProductDto> OrderProducts)
{
    public string? FullName { get; set; }
    public decimal TotalPrice { get; set; }
    public string? CancelOrderPath { get; set; }
    public string? ConfirmPaymentPath { get; set; }
    public string? ProductPdfPath { get; set; }
    public string? PayWithStripePath { get; set; }
};