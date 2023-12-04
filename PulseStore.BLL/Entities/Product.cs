namespace PulseStore.BLL.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; } = 0;

    public int? MinTemperature { get; set; }

    public int? MaxTemperature { get; set; }

    public bool IsPublished { get; set; }

    public DateTime DateCreated { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public ICollection<ProductPhoto> ProductPhotos { get; set; }

    public ICollection<UserProductView> UserProductViews { get; set; }

    public ICollection<StockProduct> StockProducts { get; set; }

    public ICollection<UserCartProduct> UserCartProducts { get; set; }
}