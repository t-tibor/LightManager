public interface IAutomation
{
    event EventHandler<AutomationState> StateChanged;

    AutomationState CurrentState {get;}

    event EventHandler Triggered;

    void Start();

    void Stop();
}
