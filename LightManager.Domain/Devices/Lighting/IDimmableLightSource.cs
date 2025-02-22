namespace LightManager.Domain.Devices.Lighting
{
	public interface IDimmableLightSource : ILightSource
	{
		IObservable<double> BrightnessChanges { get; }

		/// <summary>
		/// Brightness of the light source, from 0 to 1.
		/// </summary>
		double Brightness { get; }

		Task SetBrightnessAsync(double brightness, CancellationToken token = default);
	}
}
