using PulseStore.BLL.Models.ProductPhoto;

namespace PulseStore.BLL.Models.Product;

public record CatalogProductDto(
    int Id,
    string Name,
    decimal Price,
    DateTime DateCreated,
    string? Description,
    bool isPublished,
    ICollection<CatalogProductPhotoDto> ProductPhotos);
