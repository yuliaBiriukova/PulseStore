namespace PulseStore.BLL.Models.Category;

public record CategoryExtendedDto(
    int Id,
    string Name
) : CategoryDto(Id, Name)
{
    public int ProductsQuantity { get; set; }
};