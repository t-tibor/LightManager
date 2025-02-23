namespace LightManager.Domain.Devices.AdaptiveLightingController;

public interface IAdaptiveLightingController
{
	bool IsLightingEnabled { get; }

	IObservable<bool> IsLightingEnabledChanges { get; }

	double? LastCalculatedLightSettings { get; }

	IObservable<double> LightSettingsChanges { get; }
}