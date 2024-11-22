namespace LightManager.Infrastructure.Time;

public class MockTimeSource: ITimeSource
{
    public DateTimeOffset Now { get; set; } = DateTimeOffset.Now;
}