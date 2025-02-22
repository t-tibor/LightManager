var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
	.WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(8080))
	.WithPgWeb(webpg => webpg.WithHostPort(8081))
	.WithDataVolume(isReadOnly: false);

var postgresdb = postgres.AddDatabase("postgresdb");

var migrationService = builder.AddProject<Projects.LightManager_Persistence_MigrationService>("migrationService")
	.WaitFor(postgres)
	.WithReference(postgresdb);

builder.AddProject<Projects.LightManager_Web>("lightmanager-web")
	.WithReference(postgresdb)
	.WaitForCompletion(migrationService);

await builder.Build().RunAsync();
