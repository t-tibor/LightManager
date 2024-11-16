using System.Reactive;
using System.Reactive.Linq;

public class Automation<T>(
    IObservable<T> trigger,
    Func<Timestamped<T>, bool> predicate,
    Action<Timestamped<T>> action
        ): IAutomation
{
    public event EventHandler<AutomationState>? StateChanged;

    private AutomationState _currentState;
    public AutomationState CurrentState {
        get =>_currentState;
        set {
            _currentState = value;
            StateChanged?.Invoke(this, value);
        }
    }
    
    public event EventHandler? Triggered;

    private IDisposable? subscription;

    public void Start()
    {
        if(this.CurrentState != AutomationState.Running) return;

        subscription = trigger.Timestamp()
        .Where(predicate)
        .Subscribe( arg => {
            action(arg);
            Triggered?.Invoke(this, EventArgs.Empty);
        }
    );

        this.CurrentState = AutomationState.Running;        
    }

    public void Stop()
    {
        if(this.CurrentState == AutomationState.Running) return;

        subscription?.Dispose();
        subscription = null;

        this.CurrentState = AutomationState.Stopped; 
    }
}
