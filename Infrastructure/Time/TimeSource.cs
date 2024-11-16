namespace LightManager.Infrastructure.Time;

public class TimeSource : ITimeSource
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}