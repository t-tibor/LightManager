using LightManager.Persistence;
using LightManager.Persistence.MigrationService;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
	.WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<LightManagerDbContext>("postgresdb", null, cfg =>
{
	cfg.ConfigureWarnings(cfg => cfg.Log(RelationalEventId.PendingModelChangesWarning));
});

var host = builder.Build();
await host.RunAsync();
