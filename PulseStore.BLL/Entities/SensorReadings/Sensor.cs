using FluentValidation;
using PulseStore.BLL.Entities.SensorReadings.Enums;
using PulseStore.BLL.Entities.SensorReadings.Handlers;
using PulseStore.BLL.Entities.SensorReadings.ValueObject;

namespace PulseStore.BLL.Entities.SensorReadings;

public class Sensor
{
    public int Id { get; private set; }

    public string DeviceMacAddress { get; private set; } = null!;

    public string GatewayMacAddress { get; private set; } = null!;

    public SensorType SensorType { get; private set; }

    public int StockId { get; private set; }

    public Stock Stock { get; private set; } = null!;

    public ICollection<SensorReading> SensorReadings { get; init; } = new List<SensorReading>();

    public static SensorSensorReading CreateSensorSensorReading(string json) => SensorReadingHandler.CreateSensorReading(json);

    internal static Sensor CreateFromSensorSensorReading(SensorSensorReading sensorSensorReading) =>
        new()
        {
            DeviceMacAddress = sensorSensorReading.DeviceAddress,
            GatewayMacAddress = sensorSensorReading.GatewayAddress,
            SensorType = sensorSensorReading.SensorType,
        };

    internal void AddReading(SensorReading sensorReading)
    {
        SensorReadings.Add(sensorReading);
    }

    internal void SetStockId(int stockId)
    {
        StockId = stockId;
    }

    internal void Validate()
    {
        var validator = new Validator();
        var results = validator.Validate(this);

        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                throw new Exception("Property: {failure.PropertyName} Error: {failure.ErrorMessage}");
            }
        }
    }

    private class Validator : AbstractValidator<Sensor>
    {
        public Validator()
        {
            RuleFor(s => s.DeviceMacAddress).NotEmpty();
            RuleFor(s => s.GatewayMacAddress).NotEmpty();
            RuleForEach(s => s.SensorReadings)
                .Must((sensor, reading) => reading.CheckCorrectPropertySet(sensor.SensorType))
                .WithMessage("Should set correct sensor type");
            RuleFor(s => new { s.StockId, s.Stock })
                .Must(s => s.StockId != 0 || s.Stock is not null)
                .WithMessage("Stock should be set.");

        }
    }
}
