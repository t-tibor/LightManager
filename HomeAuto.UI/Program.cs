using HomeAuto.UI.Components;
using LightManager.Infrastructure.MQTT;
using LightManager.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddSingleton<IMqttConnector, MqttConnector>();
builder.Services.AddOptions<MqttConfig>().Configure(c =>
{
    c.ServerHostName = "localhost";
    c.Port = 8082;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
