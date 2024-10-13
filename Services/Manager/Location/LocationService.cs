using CoordinateSharp;
using Microsoft.Extensions.Options;

namespace LightManager.Services.Manager.Location
{

    internal class LocationService(IOptions<LocationServiceConfig> configOptions) : ILocationService
    {
        private readonly LocationServiceConfig config = configOptions.Value;

        private DateTime? _lastCacheUpdate;
        private DateTime? _sunsetTimeCache;
        private DateTime? _sunriseTimeCache;

        public double Latitude => config.Latitude;

        public double Longitude => config.Longitude;

        public (DateTime TodaySunriseTime, DateTime TodaySunsetTime) SunInfo
        {
            get
            {
                var now = DateTime.Now;
                CheckCacheForUpdate();
                return (_sunriseTimeCache ?? new DateTime(new DateOnly(now.Year, now.Month, now.Day), new TimeOnly(18, 0, 0)),
                        _sunsetTimeCache ?? new DateTime(new DateOnly(now.Year, now.Month, now.Day), new TimeOnly(6, 0, 0)));
            }
        }

        private void CheckCacheForUpdate()
        {
            var now = DateTime.Now;

            if (!_sunsetTimeCache.HasValue ||
                !_sunriseTimeCache.HasValue ||
                !_lastCacheUpdate.HasValue ||
                _lastCacheUpdate.Value < now - TimeSpan.FromHours(6) ||
                _lastCacheUpdate.Value.Day != now.Day)
            {
                var c = new Coordinate(config.Latitude, config.Longitude, now, new EagerLoad { Celestial = true });
                _sunsetTimeCache = c.CelestialInfo.SunSet;
                _sunriseTimeCache = c.CelestialInfo.SunRise;
            }

            _lastCacheUpdate = now;
        }
    }

    public class LocationServiceConfig
    {
        public double Latitude { get; set; } = 47.49801;

        public double Longitude { get; set; } = 19.03991;
    }
}
