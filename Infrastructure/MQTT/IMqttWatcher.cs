namespace LightManager.Infrastructure.MQTT;

public interface IMqttWatcher<T> : IAsyncDisposable
{
    IObservable<T> Trigger { get; }

    Task StartAsync();

    Task StopAsync();
}
