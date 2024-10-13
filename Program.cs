using Quartz;
using LightManager.Services.MQTT;
using LightManager.Services.Manager;
using LightManager.Services.LightBulb;
using LightManager.Services.Manager.Location;
using LightManager.Services.Motion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<LightingTimetable>(builder.Configuration.GetRequiredSection(nameof(LightingTimetable)));
builder.Services.AddSingleton<ILightingTimetableSource, ConfigBasedTimetableSource>();
builder.Services.Configure<LocationServiceConfig>(builder.Configuration.GetRequiredSection(nameof(LocationServiceConfig)));
builder.Services.AddSingleton<ILocationService, LocationService>();
builder.Services.AddSingleton<ILightingManager, LightingManager>();
builder.Services.Configure<MqttConfig>(builder.Configuration.GetRequiredSection(nameof(MqttConfig)));
builder.Services.AddSingleton<IMqttConnector, MqttConnector>();

builder.Services.AddSingleton<LightBulbControllerService>();
builder.Services.AddSingleton<ILightBulbController>(svc => svc.GetRequiredService<LightBulbControllerService>());
builder.Services.AddHostedService(svc => svc.GetRequiredService<LightBulbControllerService>());

builder.Services.AddOptions<MotionDetectionConfig>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(MotionDetectionConfig)))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddHostedService<MotionDetectionService>();

builder.Services.AddQuartzHostedService(c => {
    c.AwaitApplicationStarted = true;
    c.StartDelay = TimeSpan.FromSeconds(3);
});
builder.Services.AddQuartz(c =>
    c.ScheduleJob<SettingsUpdateJob>(
        tr => tr
        .WithIdentity("UpdateJobTrigger")
        .WithSimpleSchedule(sch => sch
            .WithIntervalInMinutes(30)
            .RepeatForever()
        )
        .StartNow(),
        j => j
        .WithIdentity("UpdateJob")
    )
);

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
