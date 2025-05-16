using LightManager.Infrastructure.MQTT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightManager.PanelUi.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;
	private readonly IMqttConnector _mqttConnector;

	public IndexModel(ILogger<IndexModel> logger, IMqttConnector mqttConnector)
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
					.WithTopic("zigbee2mqtt/LedController/set")
					.WithPayload("{\"state\": \"ON\", \"brightness\": 255}")
					.Build()
				);

				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/KitchenLedLightSwitch/set")
					.WithPayload("{ \"state\": \"ON\"}")
					.Build()
				);

				await Task.Delay(2000);

				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/LedController/set")
					.WithPayload("{\"state\": \"ON\", \"brightness\": 255}")
					.Build()
				);

				break;
			case "mild":
				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/LedController/set")
					.WithPayload("{\"state\": \"ON\", \"brightness\": 26}")
					.Build()
				);

				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/KitchenLedLightSwitch/set")
					.WithPayload("{ \"state\": \"ON\"}")
					.Build()
				);

				await Task.Delay(2000);

				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/LedController/set")
					.WithPayload("{\"state\": \"ON\", \"brightness\": 26}")
					.Build()
				);			
				break;
			case "off":
				await _mqttConnector.Publish(x => x
					.WithTopic("zigbee2mqtt/LedController/set")
					.WithPayload("{\"state\": \"OFF\", \"brightness\": 0}")
					.Build()
				);

				await Task.Delay(2000);

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

	private async Task TurnOn()
	{
		// subscribe to the led controller topic

		// send out a brightness update command

		// switch on the led power supply

		// wait for the led controller status update with a timeout

		// if timeouted or the current brightness is not the required one, then 


	}

}
