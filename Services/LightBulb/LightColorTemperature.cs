namespace LightManager.Services.LightBulb;

public struct LightColorTemperature
{
    public int Value { get; }

    private LightColorTemperature(int value)
    {
        Value = value;
    }

    const int MaxIntensity = 454;
    const int MinIntensity = 250;

    public static LightColorTemperature Warmest => new LightColorTemperature(MaxIntensity);

    public static LightColorTemperature Coolest => new LightColorTemperature(MinIntensity);

    public static LightColorTemperature FromValue(int value)
    {
        return new LightColorTemperature(Math.Clamp(value, MinIntensity, MaxIntensity));
    }

    public static LightColorTemperature FromRatio(double ratio)
    {

        var intensityDouble = Math.Clamp(
            MinIntensity + (MaxIntensity - MinIntensity) * ratio,
            MinIntensity,
            MaxIntensity
            );

        var intensity = (int)intensityDouble;

        return new LightColorTemperature(intensity);
    }
}