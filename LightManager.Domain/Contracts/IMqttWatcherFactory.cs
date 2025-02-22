namespace LightManager.Domain.Contracts;

public interface IMqttWatcherFactory<T>
{
	IMqttWatcher<T> CreateWatcher(string topic, Func<string, T> parser);
}