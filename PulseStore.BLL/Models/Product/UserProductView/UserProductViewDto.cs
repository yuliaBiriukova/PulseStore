namespace PulseStore.BLL.Models.Product.UserProductView;

public record UserProductViewDto(
    Guid UserId,
    int ProductId,
    DateTime ViewedAt)
{
    public CatalogProductDto? Product { get; set; }
}