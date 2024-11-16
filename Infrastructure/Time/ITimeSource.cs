namespace LightManager.Infrastructure.Time;

public interface ITimeSource
{
    DateTimeOffset Now { get; }
}