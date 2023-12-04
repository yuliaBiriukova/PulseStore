namespace PulseStore.BLL.Entities;

public class UserProductView
{
    public Guid UserId { get; set; } 
    public int ProductId { get; set; }
    public DateTime ViewedAt { get; set; }
    public Product Product { get; set; }
}
