using LightManager.Services.Manager;

namespace LightManager.Services.LightBulb;

public interface ILightSource
{
    LightSourceState? CurrentState { get; }

    Task SetState(LightSourceState state, CancellationToken token = default);
}

public interface ILightBulbController: ILightSource
{
    Task SetSettings(LightSettingsDto settings, CancellationToken token = default);
}


