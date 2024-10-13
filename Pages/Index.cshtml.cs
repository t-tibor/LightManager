using LightManager.Services.Manager.Location;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightManager.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public ILocationService LocationService { get; }

    public IndexModel(ILogger<IndexModel> logger, ILocationService locationService)
    {
        _logger = logger;
        LocationService = locationService;
    }
       

    public void OnGet()
    {

    }
}