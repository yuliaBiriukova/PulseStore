namespace PulseStore.PL.ViewModels.Category;

public record CategoryExtendedViewModel(
    int Id,
    string Name,
    int ProductsQuantity
) : CategoryViewModel(Id, Name);