using PulseStore.BLL.Models.Category;
using PulseStore.BLL.Models.ProductPhoto;

namespace PulseStore.BLL.Models.Product;

public record CatalogProductExtendedDto(
    int Id,
    string Name,
    decimal Price,
    DateTime DateCreated,
    string? Description,
    CategoryDto Category,
    bool IsPublished,
    int? MinTemperature,
    int? MaxTemperature,
    ICollection<CatalogProductPhotoDto> ProductPhotos
) : CatalogProductDto(Id, Name, Price, DateCreated, Description, IsPublished, ProductPhotos)
{
    public int Quantity { get; set; }
}