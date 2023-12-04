namespace PulseStore.PL.ViewModels.Product;

public record CreateProductViewModel(
    string Name,
    int CategoryId,
    decimal Price,
    int? MaxTemperature,
    int? MinTemperature,
    string? Description,
    IFormFile[]? Files
    );