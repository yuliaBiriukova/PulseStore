using PulseStore.BLL.Entities.SensorReadings.Enums;

namespace PulseStore.BLL.Entities.SensorReadings;

public class SensorReading
{
    public int Id { get; set; }

    public DateTime ReadingDate { get; set; }

    public float? Temperature { get; set; }

    public float? Humidity { get; set; }

    public int SensorId { get; set; }

    public Sensor Sensor { get; set; } = null!;

    public bool CheckCorrectPropertySet(SensorType type) => type switch
    {
        SensorType.TemperatureHumidity => Temperature.HasValue && Humidity.HasValue,
        SensorType.Temperature => Temperature.HasValue,
        SensorType.Humidity => Humidity.HasValue,
        SensorType.Unknown => !Temperature.HasValue && !Humidity.HasValue,
    };
}