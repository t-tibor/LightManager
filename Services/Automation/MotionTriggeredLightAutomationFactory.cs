using System.ComponentModel.DataAnnotations;
using LightManager.Services.LightBulb;
using LightManager.Services.Motion;
using LightManager.Infrastructure.Time;
using System.Reactive;

namespace LightManager.Services.Automation;

public class MotionTriggeredLightAutomationFactory(
    ILogger<MotionTriggeredLightAutomationFactory> logger,
    ILoggerFactory loggerFactory, 
    IServiceProvider svc, 
    ITimeSource timeSource)
{
    /// <summary>
    /// Creates a new instance of a motion triggered light automation
    /// </summary>
    public IAutomation CreateAutomation(string automationName, MotionTriggeredLightAutomationConfig config)
    {
        logger.LogDebug("Creating automation {AutomationName} with config: {AutomationConfig}.", automationName, config);

        var motionSensor = svc.GetRequiredKeyedService<IMotionSensor>(config.MotionSensorName);
        var lightSource = svc.GetRequiredKeyedService<ILightBulbController>(config.LightSourceName);

        Func<Timestamped<bool>, bool> predicate = timedTrigger => {
            if(timedTrigger.Value == false) return true;

            var currentLocalTime = timeSource.Now.ToLocalTime();

            if(currentLocalTime.Hour >= config.OperateAfterHour || currentLocalTime.Hour <= config.OperateBeforeHour) return true;

            return false;
        };

        Action<Timestamped<bool>> action = timedTrigger =>
        {
            var newState = timedTrigger.Value ? LightSourceState.On : LightSourceState.Off;

            lightSource.SetState(newState);
        };        

        return new Automation<bool>(
            loggerFactory.CreateLogger<Automation<bool>>(),
            automationName,
            motionSensor.Occupancy,
            predicate,
            action
        );
    }

    /// <summary>
    /// Validates a <see cref="MotionTriggeredLightAutomationConfig"/>.
    /// </summary>
    /// <param name="config">The configuration to validate.</param>
    /// <returns>An enumeration of the validation errors.</returns>
    /// <remarks>
    /// The validation errors are in the form of a <see cref="ValidationResult"/> and are returned
    /// as an enumeration. The error messages will be in the form of a string.
    /// </remarks>
    public IEnumerable<ValidationResult> Validate(MotionTriggeredLightAutomationConfig config)
    {
        if(svc.GetKeyedService<IMotionSensor>(config.MotionSensorName) is null)
        {
            yield return new ValidationResult($"Motion sensor '{config.MotionSensorName}' does not exist");
        }

        if(svc.GetKeyedService<ILightBulbController>(config.LightSourceName) is null)
        {
            yield return new ValidationResult($"Light source '{config.LightSourceName}' does not exist");
        }
    }

    /// <summary>
    /// Creates the automation instances from the given configuration.
    /// </summary>
    /// <param name="configs">The configurations to create automation instances from.</param>
    /// <returns>The created automation instances.</returns>
    public IEnumerable<IAutomation> CreateAutomations(Dictionary<string, MotionTriggeredLightAutomationConfig> configs)
        => configs.Select(kv => CreateAutomation(kv.Key, kv.Value));
}

public record MotionTriggeredLightAutomationConfig
{
    [Required(AllowEmptyStrings = false)]
    public string MotionSensorName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string LightSourceName { get; set; } = string.Empty;

    [Range(15, 24)]
    public int OperateAfterHour { get; set; }

    [Range(0, 9)]
    public int OperateBeforeHour { get; set; }
}