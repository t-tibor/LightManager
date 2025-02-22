namespace LightManager.Domain.Services.AdaptiveLighting;

public interface IAdaptiveLightingStrategy
{
	bool IsLightingEnabled { get; }

	IObservable<bool> IsLightingEnabledChanges { get; }

	double? LastCalculatedLightSettings { get; }

	IObservable<double> LightSettingsChanges { get; }
}