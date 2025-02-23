namespace LightManager.Domain.Contracts;

public interface ITimeSource
{
	DateTimeOffset Now { get; }
}