using LightManager.Infrastructure.Location;
using LightManager.Infrastructure.Time;
using LightManager.Infrastructure.MQTT;
using MQTTnet.Channel;

public static class InfrastructureInstaller
{
    public static void RegisterInfrastructure(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddOptions<MqttConfig>()
        .Bind(config.GetRequiredSection(nameof(MqttConfig)))
        .ValidateDataAnnotations()
        .ValidateOnStart();        
        services.AddSingleton<IMqttConnector, MqttConnector>();

        services.Configure<LocationServiceConfig>(config.GetRequiredSection(nameof(LocationServiceConfig)));
        services.AddSingleton<ILocationService, LocationService>();

        services.AddSingleton<ITimeSource, TimeSource>();
    }
}