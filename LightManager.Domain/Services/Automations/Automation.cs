namespace LightManager.Domain.Services.Automations;

public class Lock
{
	private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	public IDisposable WaitFor()
	{
		_semaphore.Wait();
		return new LockReleaser(_semaphore);
	}

	private sealed class LockReleaser : IDisposable
	{
		private readonly SemaphoreSlim _semaphore;
		private bool disposed;

		public LockReleaser(SemaphoreSlim semaphore)
		{
			_semaphore = semaphore;
		}

		public void Dispose()
		{
			if(!disposed)
			{
				_semaphore.Release();
				disposed = true;
			}
		}
	}
}

public abstract class Automation(string name) : IAutomation
{
	private AutomationState _currentState = AutomationState.Stopped;
	private Lock _lock = new Lock();


	public string Name => name;

	public event EventHandler<AutomationState>? StateChanged;

	public AutomationState CurrentState
	{
		get => _currentState;
		private set
		{
			_currentState = value;
			StateChanged?.Invoke(this, value);
		}
	}

	public void Start()
	{
		using var _ = _lock.WaitFor();

		if (CurrentState == AutomationState.Stopped)
		{
			StartInternal();

			CurrentState = AutomationState.Running;
		}
	}

	public void Stop()
	{
		using var _ = _lock.WaitFor();

		if(CurrentState == AutomationState.Running)
		{
			StopInternal();
			CurrentState = AutomationState.Stopped;
		}
	}


	public event EventHandler? Triggered;


	protected void ReportTriggering()
	{
		Triggered?.Invoke(this, EventArgs.Empty);
	}

	protected abstract void StartInternal();

	protected abstract void StopInternal();
}