using LightManager.Infrastructure.Location;
using LightManager.Services.LightBulb;

namespace LightManager.Services.Manager
{

    public class LightingManager(
        ILogger<LightingManager> logger,
        ILocationService locationService,
        ILightingTimetableSource timetableSource
        ) : ILightingManager
    {
        public LightSettingsDto? LastCalculatedLightSettings { get; private set; }

        public LightSettingsDto CalculateLightSettings()
        {
            var settings = RecalculateLightSettingsInternal();
            LastCalculatedLightSettings = settings;
            return settings;
        }

        private LightSettingsDto RecalculateLightSettingsInternal()
        {
            var timeTable = timetableSource.GetTimetable();

            var minDefinedDelta = timeTable.TransitionSettings.Keys.Min();
            var maxDefinedDelta = timeTable.TransitionSettings.Keys.Max();

            (var sunriseTime, var sunsetTime) = locationService.SunInfo;
            var minDefinedDateTime = sunsetTime + TimeSpan.FromHours(minDefinedDelta);
            var maxDefinedDateTime = sunsetTime + TimeSpan.FromHours(maxDefinedDelta);

            var now = DateTime.Now;

            if (now <= minDefinedDateTime)
            {
                if (now < sunriseTime)
                {
                    return timeTable.DefaultNightSettings;
                }
                else
                {
                    return timeTable.DefaultDaySettings;
                }
            }
            if (now < maxDefinedDateTime)
            {
                var currentDeltaFromSunset = now - sunsetTime;
                var x = currentDeltaFromSunset.TotalHours;

                // interpolation
                var lower = timeTable.TransitionSettings.Where(kv => kv.Key <= x).MaxBy(kv => kv.Key);
                var upper = timeTable.TransitionSettings.Where(kv => kv.Key >= x).MinBy(kv => kv.Key);

                var interpolatedTemp = interp(x, lower.Key, upper.Key, lower.Value.ColorTemperature.Value, upper.Value.ColorTemperature.Value);
                var interpolatedIntensity = interp(x, lower.Key, upper.Key, lower.Value.Intensity.Value, upper.Value.Intensity.Value);

                return new LightSettingsDto(
                    LightColorTemperature.FromValue((int)interpolatedTemp),
                    LightIntensity.FromValue((int)interpolatedIntensity)
                    );
            }
            if (now < sunsetTime) return timeTable.DefaultDaySettings;

            return timeTable.DefaultNightSettings;
        }

        private static double interp(double x, double x1, double x2, double y1, double y2)
        {
            return y1 + (x - x1) / (x2 - x1) * (y2 - y1);
        }
    }
}
