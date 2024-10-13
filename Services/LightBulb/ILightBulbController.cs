using LightManager.Services.Manager;

namespace LightManager.Services.LightBulb;

public interface ILightBulbController
{
    LightBulbState? CurrentState { get; }

    Task SetState(LightBulbState state, CancellationToken token = default);

    Task SetSettings(LightSettingsDto settings, CancellationToken token = default);
}


