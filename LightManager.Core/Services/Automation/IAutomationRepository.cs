using LightManager.Domain.Services.Automations;

namespace LightManager.Application.Services.Automation;

public interface IAutomationRepository
{
	IReadOnlyList<IAutomation> GetAutomations();

	IAutomation? GetAutomationByName(string name);
}