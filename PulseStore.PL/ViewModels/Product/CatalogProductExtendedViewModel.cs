using PulseStore.PL.ViewModels.Category;
using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.ViewModels.Product;

public record CatalogProductExtendedViewModel (
    int Id,
    string Name,
    decimal Price,
    DateTime DateCreated,
    string? Description,
    CategoryViewModel Category,
    int Quantity,
    bool IsPublished,
    int? MinTemperature,
    int? MaxTemperature,
    ICollection<CatalogProductPhotoViewModel> ProductPhotos
) : CatalogProductViewModel(Id, Name, Price, DateCreated, Description, IsPublished, ProductPhotos);