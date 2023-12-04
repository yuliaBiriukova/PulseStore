using PulseStore.BLL.Entities;
using PulseStore.BLL.Entities.OrderDocuments;
using PulseStore.BLL.Entities.Orders;
using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Entities.SensorReadings;
using PulseStore.BLL.Entities.TemplateFiles;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.Database;

public class PulseStoreContext : DbContext
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductPhoto> ProductPhotos { get; set; }

    public DbSet<SearchHistoryItem> SearchHistory { get; set; }

    public DbSet<UserProductView> UserProductViews { get; set; }

    public DbSet<Stock> Stocks { get; set; }

    public DbSet<StockProduct> StockProducts { get; set; }

    public DbSet<UserCartProduct> UserCartProducts { get; set; }

    public DbSet<Sensor> Sensors { get; set; }

    public DbSet<SensorReading> SensorReadings { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderProduct> OrderProducts { get; set; }

    public DbSet<NfcDevice> NfcDevices { get; set; }

    public DbSet<SecurityUser> SecurityUsers { get; set; }

    public DbSet<TemplateFile> TemplateFiles { get; set; }

    public DbSet<OrderDocument> OrderDocuments { get; set; }

    public PulseStoreContext(DbContextOptions<PulseStoreContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PulseStoreContext).Assembly);
    }
}