namespace PulseStore.PL.ViewModels.Order;

public record AdminOrderProductViewModel(
    int ProductId,
    string ProductPhotoPath,
    string ProductName,
    string ProductCategoryName,
    decimal PricePerItem,
    int Quantity,
    int AvailableQuantity);