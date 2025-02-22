namespace LightManager.Core.Contracts;

public interface ILocationService
{
	public double Latitude { get; }

	public double Longitude { get; }

	public (DateTime TodaySunriseTime, DateTime TodaySunsetTime) SunInfo { get; }
}
