using LightManager.Services.LightBulb;

namespace LightManager.Services.Manager
{
    public interface ILightingManager
    {
        LightSettingsDto? LastCalculatedLightSettings { get; }

        LightSettingsDto CalculateLightSettings();
    }

    public record LightSettingsDto(
    LightColorTemperature ColorTemperature,
    LightIntensity Intensity
    );
}