using LightManager.Domain.Devices.AdaptiveLightingController;
using LightManager.Domain.Devices.Lighting;
using LightManager.Domain.Devices.Motion;

namespace LightManager.Domain.Services.Automations;

public class MotionTriggeredLightAutomation(
	string name,
	IMotionSensor motionSensor,
	ILightSource lightSource,
	IAdaptiveLightingController lightingStrategy
		) : Automation(name)
{
	private IDisposable? subscription = null;

	protected override void StartInternal()
	{
		subscription = motionSensor.OccupancyChanges.Subscribe(triggered =>
		{
			if (triggered)
			{
				if (lightingStrategy.IsLightingEnabled)
				{
					lightSource.TurnOnAsync().Wait();
					ReportTriggering();
				}
			}
			else
			{
				lightSource.TurnOffAsync().Wait();
				ReportTriggering();
			}
		});
	}

	protected override void StopInternal()
	{
		subscription?.Dispose();
		subscription = null;
	}
}