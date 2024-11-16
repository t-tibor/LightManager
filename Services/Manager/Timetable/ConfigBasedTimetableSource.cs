using LightManager.Services.LightBulb;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace LightManager.Services.Manager
{
    internal class ConfigBasedTimetableSource(IOptions<LightingTimetable> configOptions) : ILightingTimetableSource
    {
        public LightingTimetableDto GetTimetable()
        {
            return configOptions.Value.ToDto();
        }
    }

    public class LightSettings
    {
        public double IntensityRatio { get; set; }
        public double TemperatureColorRatio { get; set; }

        public LightSettingsDto ToDto()
        {
            return new LightSettingsDto(
                LightColorTemperature.FromRatio(TemperatureColorRatio),
                LightIntensity.FromRatio(IntensityRatio)
                );
        }
    }

    internal class LightingTimetable
    {
        public LightSettings DefaultNightSettings { get; set; } = new();

        public LightSettings DefaultDaySettings { get; set; } = new();

        public Dictionary<string, LightSettings> TransitionSettings { get; set; } = [];

        public LightingTimetableDto ToDto()
        {
            return new LightingTimetableDto(
                DefaultDaySettings: DefaultDaySettings.ToDto(),
                DefaultNightSettings: DefaultNightSettings.ToDto(),
                TransitionSettings: TransitionSettings.ToDictionary(
                    kv => double.Parse(kv.Key, CultureInfo.InvariantCulture),
                    kv => kv.Value.ToDto()
                    )
                );
        }

    }
}
