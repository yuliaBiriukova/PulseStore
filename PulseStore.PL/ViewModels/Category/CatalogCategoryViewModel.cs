namespace PulseStore.PL.ViewModels.Category;

public record CatalogCategoryViewModel(
    int Id,
    string Name) : CategoryViewModel(Id, Name)
{
    public string? ImagePath { get; set; }
}