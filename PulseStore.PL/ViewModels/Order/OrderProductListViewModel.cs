using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.ViewModels.Order;

public record OrderProductListViewModel(
    int ProductId,
    int Quantity,
    decimal PricePerItem,
    CartProductViewModel product);