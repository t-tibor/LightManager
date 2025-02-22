namespace LightManager.Domain.Contracts;

public interface IMqttWatcher<out T> : IAsyncDisposable
{
	IObservable<T> Trigger { get; }

	T? Value { get; }

	Task StartAsync();

	Task StopAsync();
}
