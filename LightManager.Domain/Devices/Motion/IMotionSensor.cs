namespace LightManager.Domain.Devices.Motion
{
	public interface IMotionSensor : IDevice
	{
		IObservable<bool> OccupancyChanges { get; }

		bool? Occupied { get; }
	}
}
