namespace PulseStore.BLL.Models.Order.Invoice;

public record OrderInvoice(
    int Id,
    string DeliveryAddress,
    IEnumerable<InvoiceOrderProduct> OrderProducts)
{
    public string? FullName { get; set; }
    public DateTime DateIssued { get; set; }
    public DateTime PaymentDueDate { get; set; }
    public decimal TotalPrice { get; set; } = OrderProducts.Sum(op => op.Sum);
};