namespace LightManager.Infrastructure.Time;

public class MockTimeSource: ITimeSource
{
    public DateTimeOffset { get; set;} = DateTimeOffset.Now;
}