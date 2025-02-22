namespace LightManager.Domain.Services.Automations;

public interface IAutomation
{
	string Name { get; }

	event EventHandler Triggered;

	event EventHandler<AutomationState> StateChanged;

	AutomationState CurrentState { get; }

	void Start();

	void Stop();
}