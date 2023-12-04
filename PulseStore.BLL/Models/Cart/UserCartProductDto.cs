using PulseStore.BLL.Models.Product;

namespace PulseStore.BLL.Models.Cart;

public record UserCartProductDto(
    int Id, 
    int ProductId,
    CartProductDto Product,
    int Quantity)
{
    public Guid UserId { get; set; }
    public int MaxQuantity { get; set; }
};