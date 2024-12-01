namespace LightManager.Services.Automation;

public interface IAutomation
{
    string Name { get; }

    event EventHandler<AutomationState> StateChanged;

    AutomationState CurrentState { get; }

    event EventHandler Triggered;

    void Start();

    void Stop();
}