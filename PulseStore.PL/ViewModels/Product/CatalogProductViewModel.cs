using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.ViewModels.Product;

public record CatalogProductViewModel(
    int Id,
    string Name,
    decimal Price,
    DateTime DateCreated,
    string? Description,
    bool IsPublished,
    ICollection<CatalogProductPhotoViewModel> ProductPhotos);