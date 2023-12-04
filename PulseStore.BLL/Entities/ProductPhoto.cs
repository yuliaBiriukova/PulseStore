namespace PulseStore.BLL.Entities; 

public class ProductPhoto 
{
    public int Id { get; set; }

    public string ImagePath { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }

    /// <summary>
    ///     Order of product photos. Main product photo has SequenceNumber = 1
    /// </summary>
    public int SequenceNumber { get; set; }
}