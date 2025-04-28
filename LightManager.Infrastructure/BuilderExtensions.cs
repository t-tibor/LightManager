using LightManager.Infrastructure.MQTT;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LightManager.Infrastructure;

public static class BuilderExtensions
{
	public static IHostApplicationBuilder AddMqttInfrastructure(this IHostApplicationBuilder builder)
	{
		builder.Services.Configure<MqttConfig>(builder.Configuration.GetSection(nameof(MqttConfig)));
		builder.Services.AddSingleton<IMqttConnector, MqttConnector>();
		return builder;
	}
}
