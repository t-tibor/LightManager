using System.Reactive.Subjects;
using System.Text;
using LightManager.Domain.Contracts;

namespace LightManager.Infrastructure.MQTT;

public class MqttWatcher<T>(
	IMqttConnector mqttConnector,
	string topic,
	Func<string, T> evaluation
	) : IMqttWatcher<T>
{
	private IDisposable? subscription;
	private readonly Subject<T> subject = new Subject<T>();

	public IObservable<T> Trigger => subject;

	public T? Value { get; private set; }


	public async Task StartAsync()
	{
		subscription = await mqttConnector.SubscribeAsync(
			subscriptionConfig => subscriptionConfig.WithTopicFilter(topic),
			msg => Task.Run(() =>
			{
				var payload = Encoding.ASCII.GetString(msg.ApplicationMessage.PayloadSegment);

				var eval = evaluation(payload);

				// update the current value
				Value = eval;

				// publish the new evaluation
				subject.OnNext(eval);
			})
		);
	}

	public Task StopAsync()
	{
		subscription?.Dispose();
		subscription = null;

		return Task.CompletedTask;
	}

	public ValueTask DisposeAsync()
	{
		subscription?.Dispose();
		subscription = null;

		return ValueTask.CompletedTask;
	}
}
