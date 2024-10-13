using LightManager.Services.Manager;
using LightManager.Services.MQTT;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;


namespace LightManager.Services.LightBulb;

internal class LightBulbControllerService(
    IOptions<LightBulbConfig> configOptions,
    IMqttConnector mqttConnector
    ) : ILightBulbController, IHostedService
{
    private readonly LightBulbConfig config = configOptions.Value;
    private IDisposable? statusSubscription;
    private object _lastStateLock = new object();
    private LightBulbState? _lastState;
    private LightSettingsDto? currentSettings;

    public LightBulbState? CurrentState
    {
        get
        {
            lock (_lastStateLock)
            {
                return _lastState;
            }
        }

        set
        {
            lock (_lastStateLock)
            {
                _lastState = value;
            }
        }
    }
    
    public async Task SetState(LightBulbState state, CancellationToken token = default)
    {
        if (state == LightBulbState.On && currentSettings is not null)
        {
            await SendCommand(builder =>
            {
                builder.SetIntensity(currentSettings.Intensity);
                builder.SetColorTemperature(currentSettings.ColorTemperature);
                builder.SetState(state);
            }, token);
        }
        else
        {
            await SendCommand(builder =>
            {
                builder.SetState(state);
            }, token);
        }
    }

    public async Task SetSettings(LightSettingsDto settings, CancellationToken token = default)
    {
        if (CurrentState.HasValue && CurrentState.Value == LightBulbState.On)
        {
            await SendCommand(builder =>
            {
                builder.SetIntensity(settings.Intensity);
                builder.SetColorTemperature(settings.ColorTemperature);
            }, token);
        }

        currentSettings = settings;
    }

    #region PrivateMethods
    private async Task SendCommand(Action<LightBulbCommandBuilder> builder, CancellationToken token)
    {
        var cmdBuilder = new LightBulbCommandBuilder();
        builder(cmdBuilder);
        var cmd = cmdBuilder.ToString();

        await mqttConnector.Publish(
            b => b.WithTopic($"{config.MqttTopicBase}/set").WithPayload(cmd).Build(),
            token
        );
    }
    #endregion

    #region IHostedService
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        statusSubscription = await mqttConnector.SubscribeAsync(
            builder => builder.WithTopicFilter(config.MqttTopicBase),
            messageRcvd =>
            {
                var payload = Encoding.ASCII.GetString(messageRcvd.ApplicationMessage.PayloadSegment);
                var msg = JObject.Parse(payload);
                var state = msg["state"]?.ToString() ?? string.Empty;

                if (string.Compare(state, "on", ignoreCase: true) == 0)
                {
                    CurrentState = LightBulbState.On;
                }
                else
                {
                    CurrentState = LightBulbState.Off;
                }

                return Task.CompletedTask;
            }
            , cancellationToken
        );

        // ping the bulb so that we get an initial state
        await mqttConnector.Publish(b => b
                    .WithTopic($"{config.MqttTopicBase}/set").WithPayload("{  \"power_on_behavior\":\"previous\" }")
                    .Build()
        );
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        statusSubscription?.Dispose();
        statusSubscription = null;
        return Task.CompletedTask;
    }
    #endregion
}

public class LightBulbCommandBuilder
{
    private readonly Dictionary<string, string> cmd = new();

    public LightBulbCommandBuilder SetColorTemperature(LightColorTemperature temperature, CancellationToken token = default)
    {
        cmd.Add("color_temp", temperature.Value.ToString());
        return this;
    }

    public LightBulbCommandBuilder SetIntensity(LightIntensity intensity, CancellationToken token = default)
    {
        cmd.Add("brightness", intensity.Value.ToString());
        return this;
    }


    public LightBulbCommandBuilder SetState(LightBulbState state, CancellationToken token = default)
    {
        cmd.Add("state", state switch
        {
            LightBulbState.On => "ON",
            LightBulbState.Off => "OFF",
            _ => throw new NotImplementedException()
        });
        return this;
    }

    public override string ToString() => JsonConvert.SerializeObject(cmd);
}

internal class LightBulbConfig
{
    public string MqttTopicBase { get; set; } = "zigbee2mqtt/Bulb1";
}