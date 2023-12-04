namespace PulseStore.PL.ViewModels.SensorReading;

public record SensorReadingViewModel(
    int Id,
    DateTime ReadingDate,
    float? Temperature,
    float? Humidity);