using LightManager.Services.LightBulb;
using LightManager.Services.MQTT;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LightManager.Services.Motion
{
    public class MotionDetectionService(
        IOptions<MotionDetectionConfig> configOptions,
        IMqttConnector mqttConnector, 
        ILightBulbController lightBulbController
        ) : IHostedService
    {
        private IDisposable? subscription;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            subscription = await mqttConnector.SubscribeAsync(subscriptionConfig => subscriptionConfig
                .WithTopicFilter(configOptions.Value.MotionDetectorTopic),
                async msg =>
                {
                    var payload = Encoding.ASCII.GetString(msg.ApplicationMessage.PayloadSegment);
                    var obj = JObject.Parse(payload);
                    var state = obj["occupancy"]?.ToString() ?? string.Empty;

                    if (string.Compare(state, "true", ignoreCase: true) == 0)
                    {
                        await lightBulbController.SetState(LightSourceState.On);
                    }
                    else
                    {
                        await lightBulbController.SetState(LightSourceState.Off);
                    }
                }
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            subscription?.Dispose();
            subscription = null;

            return Task.CompletedTask;
        }
    }

    public class MotionDetectionConfig
    {
        [Required]
        public string MotionDetectorTopic { get; set; } = null!;
    }
}
