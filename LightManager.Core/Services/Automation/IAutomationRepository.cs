namespace LightManager.Core.Services.Automation;

public interface IAutomationRepository
{
	IReadOnlyList<IAutomation> GetAutomations();

	IAutomation? GetAutomationByName(string name);


}