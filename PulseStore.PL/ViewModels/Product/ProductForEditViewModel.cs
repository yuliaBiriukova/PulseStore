using PulseStore.BLL.Models.ProductPhoto;
using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.ViewModels.Product;

public record ProductForEditViewModel(
        int Id,
        string Name,
        decimal Price,
        int CategoryId,
        int MinTemperature,
        int MaxTemperature,
        bool IsPublished,
        ICollection<ProductPhotoForEditViewModel> ProductPhotos,
        string Description
    );