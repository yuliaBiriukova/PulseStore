namespace PulseStore.BLL.Models.Category;

public record CatalogCategoryDto(
    int Id, 
    string Name) : CategoryDto(Id, Name)
{
    public string? ImagePath { get; set; }
}