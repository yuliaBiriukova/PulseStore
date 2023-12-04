using PulseStore.BLL.Models.Order.AdminOrder;

namespace PulseStore.BLL.Models.Order.PaymentEmail;

public record PaymentOrderProductDto(
    int ProductId,
    string ProductName,
    string ProductCategoryName,
    decimal PricePerItem,
    int Quantity) : AdminOrderProductDto(ProductId, ProductName, ProductCategoryName, PricePerItem, Quantity)
{
    public decimal Sum { get; set; }
}