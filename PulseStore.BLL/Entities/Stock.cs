using PulseStore.BLL.Entities.Security;
using PulseStore.BLL.Entities.SensorReadings;

namespace PulseStore.BLL.Entities;

public class Stock
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Sensor> Sensors { get; set; }
    public ICollection<StockProduct> StockProducts { get; set; }
    public ICollection<SecurityUser> SecurityUsers { get; set; }
}