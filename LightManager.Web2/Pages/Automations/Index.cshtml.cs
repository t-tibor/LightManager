using LightManager.Services.Automation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightManager.Web.Pages.Automations;

public class AutomationsModel(
	ILogger<AutomationsModel> _logger,
	IAutomationRepository _automationRepository
) : PageModel
{

	public IReadOnlyList<IAutomation> Automations = [];

	public void OnGet()
	{
		_logger.LogDebug("Getting automations");
		Automations = _automationRepository.GetAutomations();
	}
}
