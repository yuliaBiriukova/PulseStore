namespace PulseStore.PL.ViewModels.Product;

public record ChangeProductStatusCommandViewModel(
    int ProductId,
    bool IsPublished);