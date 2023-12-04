namespace PulseStore.BLL.Entities;

public class UserCartProduct
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}