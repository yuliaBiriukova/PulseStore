namespace PulseStore.BLL.Entities.SensorReadings.Enums;

[Flags]
public enum SensorType
{
    Unknown = 0,
    Temperature = 1,
    Humidity = 2,
    TemperatureHumidity = 4,
}