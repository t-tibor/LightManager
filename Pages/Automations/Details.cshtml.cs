using LightManager.Services.Automation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightManager.Pages.Automations;

public class AutomationDetailModel(
    ILogger<AutomationDetailModel> _logger,
    IAutomationRepository _automationRepository
    ) : PageModel
{
    public string Name {get; set;} = string.Empty;

    public string CurrentState { get; set;} = string.Empty;

    public IActionResult OnGet(string id)
    {
        var automation =  _automationRepository.GetAutomationByName(id);

        if (automation is null)
        {
            return NotFound();
        }

        this.Name = automation.Name;
        this.CurrentState = automation.CurrentState.ToString();

        return Page();
    }

    public IActionResult OnPostStop(string id)
    {
        _logger.LogInformation("Stopping automation {id}", id);

        var automation =  _automationRepository.GetAutomationByName(id);
        if (automation is null)
        {
            return NotFound();
        }

        automation.Stop();

        this.Name = automation.Name;
        this.CurrentState = automation.CurrentState.ToString();

        return Page();
    }

    public IActionResult OnPostStart(string id)
    {
        _logger.LogInformation("Starting automation {id}", id);

        var automation =  _automationRepository.GetAutomationByName(id);
        if (automation is null)
        {
            return NotFound();
        }

        automation.Start();

        this.Name = automation.Name;
        this.CurrentState = automation.CurrentState.ToString();

        return Page();
    }
}
