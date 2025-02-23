using LightManager.Domain.Devices.Motion;
using Microsoft.EntityFrameworkCore;

namespace LightManager.Persistence
{
	public class LightManagerDbContext : DbContext
	{
		public DbSet<MqttMotionSensorConfig> MqttMotionSensorConfigs { get; set; }

		//public DbSet<AdaptiveLightingConfig> AdaptiveLightingConfigs { get; set; }

		public LightManagerDbContext(DbContextOptions<LightManagerDbContext> options) : base(options)
		{
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(LightManagerDbContext).Assembly);
		}
	}
}
