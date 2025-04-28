using LightManager.Infrastructure.MQTT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightManager.PanelUi.Pages;

public class KitchenModel : PageModel
{
	private readonly ILogger<KitchenModel> _logger;
	private readonly IMqttConnector _mqttConnector;

	public KitchenModel(ILogger<KitchenModel> logger, IMqttConnector mqttConnector)
	{
		_logger = logger;
		this._mqttConnector = mqttConnector;
	}

	public Task<IActionResult> OnGetAsync()
	{
		return Task.FromResult<IActionResult>(Page());
	}

	public async Task<IActionResult> OnPostAsync(string mode)
	{
		if (string.IsNullOrEmpty(mode))
		{
			return Page();
		}

		switch (mode)
		{
			case "on":
				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/KitchenLedLightSwitch/set")
					.WithPayload("{ \"state\": \"ON\"}")
					.Build()
				);
				break;
			case "off":
				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/KitchenLedLightSwitch/set")
					.WithPayload("{ \"state\": \"OFF\"}")
					.Build()
				);
				break;
			default:
				return NotFound("Invalid mode");
		}

		return Page();
	}
}
