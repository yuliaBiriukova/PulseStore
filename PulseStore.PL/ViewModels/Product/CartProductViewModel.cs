using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.ViewModels.Product;

public record CartProductViewModel(
    int Id,
    string Name,
    decimal Price,
    CatalogProductPhotoViewModel ProductPhoto);