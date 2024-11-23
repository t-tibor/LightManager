using LightManager.Services.Automation;

namespace LightManager.App;

/// <summary>
/// This class contains methods for registering automations in the application.
/// </summary>
public static class AutomationInstaller
{
    /// <summary>
    /// Registers motion triggered light automations based on the provided configuration.
    /// </summary>
    /// <param name="services">The collection of services to register automations in.</param>
    /// <param name="configuration">The application's configuration.</param>
    public static void RegisterAutomations(IServiceCollection services, IConfigurationRoot configuration)
    {
        var automationConfig = configuration.GetSection("Automations").Get<Automations>();

        if(automationConfig is null) throw new ("Automations configuration is not present in the configuration file.");

        // If motion triggered lights configuration is not present, return without registering any automations.
        services.AddSingleton<MotionTriggeredLightAutomationFactory>();
        foreach(var kv in automationConfig.MotionTriggeredLights)
        {            
            services.AddSingleton<IAutomation>(services =>
            {
                var factory = services.GetRequiredService<MotionTriggeredLightAutomationFactory>();

                // Create and register motion triggered light automations based on the configuration.
                return factory.CreateAutomation(kv.Key, kv.Value);
            });
        }

        // Add the automation service
        services.AddHostedService<AutomationService>();    
    }
}

public class Automations
{
    /// <summary>
    /// A dictionary containing configuration for motion triggered light automations.
    /// The key is the automation name, and the value is the automation configuration.
    /// </summary>
    public Dictionary<string, MotionTriggeredLightAutomationConfig> MotionTriggeredLights { get; set; } = [];
}
