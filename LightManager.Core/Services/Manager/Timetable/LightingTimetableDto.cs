namespace LightManager.Core.Services.Manager.Timetable
{
	public record LightingTimetableDto(
	LightSettingsDto DefaultDaySettings,
	LightSettingsDto DefaultNightSettings,
	Dictionary<double, LightSettingsDto> TransitionSettings
	);
}
