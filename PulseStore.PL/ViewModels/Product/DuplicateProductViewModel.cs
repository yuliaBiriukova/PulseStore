namespace PulseStore.PL.ViewModels.Product;

public record DuplicateProductViewModel(
    string Name,
    int CategoryId,
    decimal Price,
    int? MaxTemperature,
    int? MinTemperature,
    string? Description,
    bool? IsPublished,
    string[] PhotosUrls,
    IFormFile[]? NewPhotos
    );