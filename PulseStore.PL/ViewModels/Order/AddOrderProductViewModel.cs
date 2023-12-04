using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.ViewModels.Order;

public record AddOrderProductViewModel(
    int ProductId,
    int Quantity, 
    decimal PricePerItem);