
using System.Collections.Immutable;

public class AutomationService(IEnumerable<IAutomation> automations) : IHostedService
{
    private readonly IReadOnlyList<IAutomation> automationList = automations.ToImmutableList();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach(var a in automationList)
        {
            a.Start();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach(var a in automationList)
        {
            a.Stop();
        }

        return Task.CompletedTask;
    }
}