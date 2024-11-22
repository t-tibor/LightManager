using LightManager.Infrastructure.MQTT;
using LightManager.Services.LightBulb;
using LightManager.Services.Motion;

namespace LightManager.App;

public static class DevicesInstaller
{
    public static void RegisterDevices(IServiceCollection services, IConfigurationRoot config)
    {
        var devConf = config.Get<DeviceRegistry>() ?? throw new ApplicationException($"{nameof(DeviceRegistry)} section is missing from the config");

        foreach (var item in devConf.MotionSensors)
        {
            var sensorName = item.Key;

            services.AddMotionSensor(sensorName, item.Value);
        }

        foreach (var item in devConf.LightBulbs)
        {
            services.AddLightBulb(item.Key, item.Value);
        }
    }

    private static void AddMotionSensor(this IServiceCollection services, object? svcKey, MotionDetectorConfig config)
    {
        services.AddKeyedSingleton<MotionSensorMqtt>(
            svcKey,
            (svc, key) => new MotionSensorMqtt(
                svc.GetRequiredService<ILogger<MotionSensorMqtt>>(),
                config,
                svc.GetRequiredService<IMqttConnector>()
            )
        );

        services.AddKeyedSingleton<IMotionSensor>(svcKey, (svc, key) => svc.GetRequiredKeyedService<MotionSensorMqtt>(key));
        services.AddHostedService(svc => svc.GetRequiredKeyedService<MotionSensorMqtt>(svcKey));
    }

    private static void AddLightBulb(this IServiceCollection services, object? svcKey, LightBulbConfig config)
    {
        services.AddKeyedSingleton(
            svcKey,
            (svc, key) => new LightBulbControllerService(
                svc.GetRequiredService<ILogger<LightBulbControllerService>>(),
                config,
                svc.GetRequiredService<IMqttConnector>()
            )
        );

        services.AddKeyedSingleton<ILightBulbController>(svcKey, (svc, key) => svc.GetRequiredKeyedService<LightBulbControllerService>(key));
        services.AddHostedService(svc => svc.GetRequiredKeyedService<LightBulbControllerService>(svcKey));
    }
}

public class DeviceRegistry
{
    public Dictionary<string, MotionDetectorConfig> MotionSensors { get; set; } = [];

    public Dictionary<string, LightBulbConfig> LightBulbs { get; set; } = [];
}