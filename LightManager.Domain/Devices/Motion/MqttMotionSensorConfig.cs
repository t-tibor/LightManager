using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LightManager.Domain.Devices.Motion;

public class MqttMotionSensorConfig
{
	[Required]
	[Description("The name of the motion sensor.")]
	public string Name { get; set; } = null!;


	[Required]
	[Description("The MQTT topic to listen for motion detection events.")]
	public string MotionDetectorTopic { get; set; } = null!;
}
