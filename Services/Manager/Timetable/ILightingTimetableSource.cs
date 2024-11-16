namespace LightManager.Services.Manager
{
    public interface ILightingTimetableSource
    {
        LightingTimetableDto GetTimetable();
    }

    public record LightingTimetableDto(
    LightSettingsDto DefaultDaySettings,
    LightSettingsDto DefaultNightSettings,
    Dictionary<double, LightSettingsDto> TransitionSettings
    );
}
