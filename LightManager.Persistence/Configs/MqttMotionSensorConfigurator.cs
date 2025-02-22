using LightManager.Domain.Devices.Motion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightManager.Persistence.Configs
{
	internal class MqttMotionSensorConfigurator : IEntityTypeConfiguration<MqttMotionSensorConfig>
	{
		public void Configure(EntityTypeBuilder<MqttMotionSensorConfig> builder)
		{
			builder.HasKey(x => x.Name);

			builder.Property(x => x.Name)
				.IsUnicode(false)
				.IsRequired();


			builder.Property(x => x.MotionDetectorTopic)
				.IsUnicode(false)
				.IsRequired();
		}
	}
}
