using System.ComponentModel.DataAnnotations;
using LightManager.Services.LightBulb;
using LightManager.Services.Motion;
using LightManager.Infra.Motion;
using LightManager.Infrastructure.Time;

namespace LightManager.Services.Automation;

public class MotionTriggeredLightAutomationFactory(IServiceProvider svc, ITimeSource timeSource)
{
    public IAutomation CreateAutomation(MotionTriggeredLightAutomationConfig config)
    {
        var sensor = svc.GetRequiredKeyedService<IMotionSensor>(config.MotionSensorName);
        var light = svc.GetRequiredKeyedService<ILightBulbController>(config.LightSourceName);

        return new Automation<bool>(
            sensor.Occupancy,
            timedTrigger => {
                if(timedTrigger.Value == false) return true;

                if(timeSource.Now;

            },
            timedTrigger => 
            {
                if(trigger.Value == true)
                {
                    light.SetState(LightSourceState.On);
                }
                else
                {
                    light.SetState(LightSourceState.Off);
                }
            }
        );
    }

}

public class MotionTriggeredLightAutomationConfig
{
    [Required(AllowEmptyStrings = false)]
    public string MotionSensorName {get;set;} = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string LightSourceName {get; set;} = string.Empty;
}