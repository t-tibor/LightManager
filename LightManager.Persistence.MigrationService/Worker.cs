using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using OpenTelemetry.Trace;

namespace LightManager.Persistence.MigrationService;

public class Worker(
	IServiceProvider serviceProvider,
	IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
	public const string ActivitySourceName = "Migrations";
	private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

		try
		{
			using var scope = serviceProvider.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<LightManagerDbContext>();

			await RunMigrationAsync(dbContext, cancellationToken);
			await SeedDataAsync(dbContext, cancellationToken);
		}
		catch (Exception ex)
		{
			activity?.RecordException(ex);
			throw;
		}

		hostApplicationLifetime.StopApplication();
	}

	private static async Task RunMigrationAsync(LightManagerDbContext dbContext, CancellationToken cancellationToken)
	{
		var strategy = dbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () =>
		{
			var migrations = dbContext.Database.GetMigrations();
			var applMig = dbContext.Database.GetAppliedMigrations();
			try
			{

			var pendMig = dbContext.Database.GetPendingMigrations();
			}catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			await dbContext.Database.MigrateAsync(cancellationToken);
		});
	}

	private static async Task SeedDataAsync(LightManagerDbContext dbContext, CancellationToken cancellationToken)
	{
	}
}