using LightManager.Services.Manager;
using LightManager.Services.LightBulb;
using LightManager.App;
using LightManager.Infrastructure.Location;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Instapp application
InfrastructureInstaller.RegisterInfrastructure(builder.Services, builder.Configuration);
DevicesInstaller.RegisterDevices(builder.Services, builder.Configuration);
AutomationInstaller.RegisterAutomations(builder.Services, builder.Configuration);


builder.Services.Configure<LightingTimetable>(builder.Configuration.GetRequiredSection(nameof(LightingTimetable)));
builder.Services.AddSingleton<ILightingTimetableSource, ConfigBasedTimetableSource>();
builder.Services.Configure<LocationServiceConfig>(builder.Configuration.GetRequiredSection(nameof(LocationServiceConfig)));
builder.Services.AddSingleton<ILocationService, LocationService>();
builder.Services.AddSingleton<ILightingManager, LightingManager>();

builder.Services.AddSingleton<LightBulbControllerService>();
builder.Services.AddSingleton<ILightBulbController>(svc => svc.GetRequiredService<LightBulbControllerService>());
builder.Services.AddHostedService(svc => svc.GetRequiredService<LightBulbControllerService>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
