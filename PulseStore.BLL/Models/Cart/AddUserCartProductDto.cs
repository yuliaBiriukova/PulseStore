namespace PulseStore.BLL.Models.Cart;

public record AddUserCartProductDto(
    int ProductId,
    int Quantity)
{
    public Guid UserId { get; set; }
}