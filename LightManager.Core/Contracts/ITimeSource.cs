namespace LightManager.Core.Contracts;

public interface ITimeSource
{
	DateTimeOffset Now { get; }
}