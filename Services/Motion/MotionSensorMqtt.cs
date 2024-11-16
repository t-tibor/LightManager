using LightManager.Infrastructure.MQTT;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace LightManager.Services.Motion
{

    public class MotionSensorMqtt(
        MotionDetectorConfig config,
        IMqttConnector mqttConnector
            ) : IMotionSensor, IHostedService
    {
        private readonly IMqttWatcher<bool> mqttWatcher = new MqttWatcher<bool>(mqttConnector, 
            config.MotionDetectorTopic,
            payload => {
                    var obj = JObject.Parse(payload);
                    var state = obj["occupancy"]?.ToString() ?? string.Empty;

                    return string.Compare(state, "true", ignoreCase: true) == 0;
            });

        public IObservable<bool> Occupancy => mqttWatcher.Trigger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mqttWatcher.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await mqttWatcher.StopAsync();            
        }
    }


    public class MotionDetectorConfig
    {
        [Required]
        public string MotionDetectorTopic { get; set; } = null!;
    }
}
