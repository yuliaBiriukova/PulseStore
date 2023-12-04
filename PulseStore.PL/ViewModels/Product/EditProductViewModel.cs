using PulseStore.PL.ViewModels.ProductPhoto;

namespace PulseStore.PL.ViewModels.Product;

public record EditProductViewModel(
    int Id,
    string? Name,
    int CategoryId,
    decimal? Price,
    int? MaxTempreture,
    int? MinTempreture,
    string? Description,
    bool? IsPublished,
    DateTime? DateCreated,
    int[]? PhotoesDeleteId,
    IFormFile[]? PhotoesToAdd
    );