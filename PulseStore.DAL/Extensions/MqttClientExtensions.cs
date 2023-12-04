using System.Reactive.Disposables;
using System.Reactive.Linq;
using MQTTnet.Client;

public static class MqttClientExtensions
{
    public static IObservable<MqttApplicationMessageReceivedEventArgs> WhenMessageReceived(this IMqttClient client)
    {
        return Observable.Create<MqttApplicationMessageReceivedEventArgs>(observer =>
        {
            Task Handler(MqttApplicationMessageReceivedEventArgs eventArgs)
            {
                observer.OnNext(eventArgs);
                
                return Task.CompletedTask;
            }

            client.ApplicationMessageReceivedAsync += Handler;

            return Disposable.Create(() => client.ApplicationMessageReceivedAsync -= Handler);
        });
    }
}