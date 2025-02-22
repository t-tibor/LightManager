namespace LightManager.Domain.Devices.Lighting
{

	public interface ILightSource : IDevice
	{
		IObservable<LightSourceState> StateChanges { get; }

		LightSourceState State { get; }

		Task SetStateAsync(LightSourceState state, CancellationToken token = default);
	}

	public static class ILightSourceExtensions
	{
		public static Task TurnOnAsync(this ILightSource lightSource, CancellationToken token = default)
		{
			return lightSource.SetStateAsync(LightSourceState.On, token);
		}

		public static Task TurnOffAsync(this ILightSource lightSource, CancellationToken token = default)
		{
			return lightSource.SetStateAsync(LightSourceState.Off, token);
		}

		public static Task ToggleAsync(this ILightSource lightSource, CancellationToken token = default)
		{
			return lightSource.SetStateAsync(lightSource.State == LightSourceState.On ? LightSourceState.Off : LightSourceState.On, token);
		}
	}
}
