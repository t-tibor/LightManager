using System.Collections.Immutable;
using Microsoft.Extensions.Hosting;

namespace LightManager.Core.Services.Automation;


public class AutomationRepository(IEnumerable<IAutomation> automations) : IHostedService, IAutomationRepository
{
	private readonly IReadOnlyList<IAutomation> automationList = automations.ToImmutableList();

	public IAutomation? GetAutomationByName(string name)
	{
		return automationList.FirstOrDefault(a => a.Name == name);
	}

	public IReadOnlyList<IAutomation> GetAutomations()
	{
		return automationList;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		foreach (var a in automationList)
		{
			a.Start();
		}

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		foreach (var a in automationList)
		{
			a.Stop();
		}

		return Task.CompletedTask;
	}
}