using PulseStore.BLL.Models.SensorReadings;

namespace PulseStore.BLL.Services.SensorsReadings
{
    public interface ISensorReadingService
    {
        Task ConnectToMqttServer(MqttOptionsDto optionsMapped);

    }
}
