namespace PulseStore.BLL.Models.Order.AdminOrder;

public record AdminOrderProductDto(
    int ProductId,
    string ProductName,
    string ProductCategoryName,
    decimal PricePerItem,
    int Quantity)
{
    public string? ProductPhotoPath { get; set; }
    public int AvailableQuantity { get; set; }
}