using PulseStore.BLL.Models.ProductPhoto;

namespace PulseStore.BLL.Models.Product;

public record CartProductDto(
    int Id,
    string Name,
    decimal Price)
{
    public CatalogProductPhotoDto? ProductPhoto { get; set; }
};