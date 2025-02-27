using System.Reactive;
using System.Reactive.Linq;

namespace LightManager.Domain.Services.Automations;

public class Automation<T>(
	string name,
	IObservable<T> trigger,
	Func<Timestamped<T>, bool> predicate,
	Action<Timestamped<T>> action
		) : IAutomation
{
	public event EventHandler<AutomationState>? StateChanged;

	private AutomationState _currentState = AutomationState.Stopped;
	public AutomationState CurrentState
	{
		get => _currentState;
		set
		{
			_currentState = value;
			StateChanged?.Invoke(this, value);
		}
	}

	public string Name => name;

	public event EventHandler? Triggered;

	private IDisposable? subscription;

	public void Start()
	{
		if (CurrentState == AutomationState.Running) return;

		subscription = trigger.Timestamp()
		/*.Do(
			t => logger.LogDebug("Automation {AutomationName} triggered. Value: {AutomationValue}", Name, t)
		)*/
		.Where(predicate)
		/*.Do(
			t => logger.LogDebug("Automation {AutomationName} predicate matched. Value: {AutomationValue}", Name, t)
		)*/
		.Subscribe(arg =>
		{
			//logger.LogDebug("Automation {AutomationName} action triggered. Value: {AutomationValue}", Name, arg);
			action(arg);
			Triggered?.Invoke(this, EventArgs.Empty);
		});

		CurrentState = AutomationState.Running;

		//logger.LogDebug("Automation {AutomationName} started.", Name);
	}

	public void Stop()
	{
		if (CurrentState != AutomationState.Running) return;

		subscription?.Dispose();
		subscription = null;

		CurrentState = AutomationState.Stopped;

		//logger.LogDebug("Automation {AutomationName} stopped.", Name);
	}
}