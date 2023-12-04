using PulseStore.PL.ViewModels.Product;

namespace PulseStore.PL.ViewModels.Cart;

public record UserCartProductViewModel(
    int Id,
    int ProductId,
    CartProductViewModel Product,
    int Quantity,
    int MaxQuantity);