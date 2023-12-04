using PulseStore.PL.ViewModels.Category;
using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.ViewModels.Product
{
    public record AdminCatalogProductViewModel(
        int Id,
        string Name,
        decimal Price,
        CategoryViewModel Category,
        bool IsPublished,
        int Quantity,
        ICollection<CatalogProductPhotoViewModel> ProductPhotos
    );
}