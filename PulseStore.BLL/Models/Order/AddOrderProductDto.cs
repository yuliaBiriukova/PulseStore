namespace PulseStore.BLL.Models.Order;

public record AddOrderProductDto(
    int ProductId,
    int Quantity,
    decimal PricePerItem);