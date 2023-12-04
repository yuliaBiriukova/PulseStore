namespace PulseStore.BLL.Entities;

public class StockProduct
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public Stock Stock { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}