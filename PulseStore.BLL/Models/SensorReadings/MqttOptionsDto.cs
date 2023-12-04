namespace PulseStore.BLL.Models.SensorReadings;

public record MqttOptionsDto(string Broker, int Port, string Topic, string Username, string Password, bool IsTestDataRequired);
