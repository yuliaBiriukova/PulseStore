using PulseStore.BLL.Models.Category;
using PulseStore.BLL.Models.ProductPhoto;

namespace PulseStore.BLL.Models.Product
{
    public record AdminCatalogProductDto(
        int Id,
        string Name,
        decimal Price,
        CategoryDto Category,
        bool IsPublished,
        ICollection<CatalogProductPhotoDto> ProductPhotos
    )
    {
        public int Quantity { get; set; }
    };
}