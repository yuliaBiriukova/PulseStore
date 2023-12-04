using PulseStore.BLL.Models.Product;

namespace PulseStore.BLL.Models.Order;

public record OrderProductDto(
    int ProductId,
    int Quantity,
    decimal PricePerItem,
    CartProductDto Product);