namespace LightManager.Domain.Services.Automations;

public interface IAutomation
{
	string Name { get; }


	event EventHandler<AutomationState> StateChanged;

	AutomationState CurrentState { get; }

	void Start();

	void Stop();


	event EventHandler Triggered;
}