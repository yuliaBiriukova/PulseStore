using PulseStore.BLL.Entities.SensorReadings.ValueObject;
using PulseStore.BLL.Models.SensorReadings;
using MQTTnet.Client;

namespace PulseStore.BLL.ExternalServices
{
    public interface IMqttExternalService
    {
        IObservable<MqttApplicationMessageReceivedEventArgs> NewReadingStream { get; }

        Task CreateMqttSubscription(MqttOptionsDto optionsMapped);
        
        Task TestPublish<T>(MqttOptionsDto optionsMapped, IEnumerable<T> sensorSensorReadings) where T: new();

        Task DisconnectFromMqttAsync();
    }
}