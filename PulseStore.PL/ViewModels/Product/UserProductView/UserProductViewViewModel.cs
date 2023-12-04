using PulseStore.BLL.Models.Product;

namespace PulseStore.PL.ViewModels.Product.UserProductView;

public record UserProductViewViewModel(
    int ProductId,
    DateTime ViewedAt,
    CatalogProductViewModel Product
);