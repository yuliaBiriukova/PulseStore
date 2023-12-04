namespace PulseStore.BLL.Models.Cart;

public record UpdateUserCartProductDto(
    int Id,
    int ProductId,
    int Quantity)
{
    public Guid UserId { get; set; }
};