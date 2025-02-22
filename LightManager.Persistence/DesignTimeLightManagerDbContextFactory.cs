using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LightManager.Persistence
{
	public class DesignTimeLightManagerDbContextFactory : IDesignTimeDbContextFactory<LightManagerDbContext>
	{
		public LightManagerDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<LightManagerDbContext>();
			// use in-memory sqlite database
			optionsBuilder.UseNpgsql("DataSource=:memory:");
			return new LightManagerDbContext(optionsBuilder.Options);
		}
	}
}
