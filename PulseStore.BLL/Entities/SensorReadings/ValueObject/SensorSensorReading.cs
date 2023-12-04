using AutoMapper;
using PulseStore.BLL.Entities.SensorReadings.Enums;
using Newtonsoft.Json;

namespace PulseStore.BLL.Entities.SensorReadings.ValueObject;

[AutoMap(typeof(SensorReading), ReverseMap = true)]
public class SensorSensorReading
{
    [JsonProperty("device_address")]
    public string DeviceAddress { get; set; }  =null!;
    
    [JsonProperty("gateway_adress")]
    public string GatewayAddress { get; set; } =null!;

    [JsonProperty("temperature")]
    public float? Temperature { get; set; }

    [JsonProperty("humidity")]
    public float? Humidity { get; set; }

    [JsonProperty("time_stamp")]
    public DateTime? ReadingDate { get; set; }

    public SensorType SensorType
    {
        get
        {
            var sensorType = (Temperature, Humidity) switch
            {
                (not null, not null) => SensorType.TemperatureHumidity,
                (not null, null) => SensorType.Temperature,
                (null, not null) => SensorType.Humidity,
                (null, null) => SensorType.Unknown,
            };
            
            return sensorType;

        }
    }

    public static SensorSensorReading CreateMockReading() =>
        new SensorSensorReading
        {
            DeviceAddress = "test-device-addr",
            GatewayAddress = "test-gateway-addr",
            Temperature = Random.Shared.NextSingle() * 100,
            Humidity = Random.Shared.NextSingle() * 100,
            ReadingDate = DateTime.Now,
        };
}