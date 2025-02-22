using LightManager.Domain.Contracts;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;

namespace LightManager.Domain.Devices.Motion
{

	public class MqttMotionSensorClient(
		MqttMotionSensorConfig config,
		IMqttWatcherFactory<bool> mqttConnector
			) : IMotionSensor, IHostedService
	{
		private readonly IMqttWatcher<bool> mqttWatcher = mqttConnector.CreateWatcher(
			config.MotionDetectorTopic,
			payload =>
			{
				var obj = JObject.Parse(payload);
				var state = obj["occupancy"]?.ToString() ?? string.Empty;

				return string.Compare(state, "true", ignoreCase: true) == 0;
			});


		public IObservable<bool> OccupancyChanges => mqttWatcher.Trigger;

		public bool? Occupied => mqttWatcher.Value;

		public string Name => config.Name;

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			//logger.LogInformation("Starting motion sensor MQTT watcher for topic {Topic}", config.MotionDetectorTopic);
			await mqttWatcher.StartAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			//logger.LogInformation("Stopping motion sensor MQTT watcher for topic {Topic}", config.MotionDetectorTopic);
			await mqttWatcher.StopAsync();
		}
	}
}
