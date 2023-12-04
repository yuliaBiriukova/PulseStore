namespace PulseStore.BLL.Models.Order.Invoice;

public record InvoiceOrderProduct(
    int ProductId,
    int Quantity,
    decimal PricePerItem,
    string ProductName)
{
    public decimal Sum { get; set; } = Quantity * PricePerItem;
}