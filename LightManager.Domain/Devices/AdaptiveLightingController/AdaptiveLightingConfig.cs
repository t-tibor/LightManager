using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LightManager.Domain.Devices.AdaptiveLightingController
{
	public class AdaptiveLightingConfig
	{
		[Range(0, 1.0)]
		[Description("Default intensity during the day")]
		public double DefaultDayIntensity { get; set; }

		[Range(0, 4.0)]
		[Description("Transition time in hours between the daytime and night intensity.")]
		public double TransitionTimeHours { get; set; }

		[Range(0, 1.0)]
		[Description("Default intensity during the night")]
		public double DefaultNightIntensity { get; set; }
	}
}
