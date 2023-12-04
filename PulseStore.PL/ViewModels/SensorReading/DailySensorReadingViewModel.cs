namespace PulseStore.PL.ViewModels.SensorReading;

public record DailySensorReadingViewModel(
    DateTime Date,
    float? AverageTemperature,
    float? AverageHumidity);