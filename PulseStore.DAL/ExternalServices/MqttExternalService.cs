using PulseStore.BLL.ExternalServices;
using PulseStore.BLL.Models.SensorReadings;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace PulseStore.DAL.ExternalServices;

public class MqttExternalService : IMqttExternalService
{
    private IMqttClient? mqttClient;
    public IObservable<MqttApplicationMessageReceivedEventArgs>? NewReadingStream { get; private set; }

    public async Task CreateMqttSubscription(MqttOptionsDto optionsMapped)
    {
        var connectResult = await ConnectToMqttBroker(optionsMapped);
        await SubscribeToTopic(optionsMapped);
    }

    //TODO: remove this when real data wil be available
    public async Task TestPublish<T>(MqttOptionsDto optionsMapped, IEnumerable<T> sensorSensorReadings) 
        where T: new()
    {
        foreach (var reading in sensorSensorReadings)
        {
            var convertedReading= JsonConvert.SerializeObject(reading);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(optionsMapped.Topic)
                .WithPayload(convertedReading)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            await mqttClient!.PublishAsync(message);
            await Task.Delay(1000); // Wait for 1 second
        }
    }

    private async Task<MqttClientConnectResult> ConnectToMqttBroker(MqttOptionsDto mqttOptions)
    {
        var (broker, port, _, username, password, _) = mqttOptions;
        string clientId = Guid.NewGuid().ToString();

        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();


        // Create MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port); // MQTT broker address and port
        if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            options = options.WithCredentials(username, password); // Set username and password
        }

        var optionsBuild = options.WithClientId(clientId)
            .WithCleanSession()
            .Build();

        try
        {
            var connectResult = await mqttClient.ConnectAsync(optionsBuild);

            if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
            {
                throw new Exception("Failed to connect to the broker.");

            }
            else
            {
                return connectResult;
            }
        }
        catch (Exception e)
        {
            await mqttClient.DisconnectAsync();
            throw;
        }
        
    }

    private async Task SubscribeToTopic(MqttOptionsDto mqttOptions)
    {
            // NewReadingStream = Observable.FromEventPattern<Func<MqttApplicationMessageReceivedEventArgs, Task>, MqttApplicationMessageReceivedEventArgs>(
            //     h => mqttClient.ApplicationMessageReceivedAsync  += h,
            //     h => mqttClient.ApplicationMessageReceivedAsync  -= h)
            //     .Select(x => x.EventArgs);
            NewReadingStream = mqttClient.WhenMessageReceived();
            await mqttClient.SubscribeAsync(mqttOptions.Topic);
    }

    public async Task DisconnectFromMqttAsync() => await mqttClient.DisconnectAsync();

}
