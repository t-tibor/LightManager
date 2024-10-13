namespace LightManager.Services.LightBulb;

public struct LightIntensity
{
    public int Value { get; }

    private LightIntensity(int value)
    {
        Value = value;
    }

    const int MaxIntensity = 254;
    const int MinIntensity = 1;

    public static LightIntensity Max => new LightIntensity(MaxIntensity);

    public static LightIntensity Min => new LightIntensity(MinIntensity);

    public static LightIntensity FromValue(int value) => new LightIntensity(Math.Clamp(value, MinIntensity, MaxIntensity));

    public static LightIntensity FromRatio(double ratio)
    {

        var intensityDouble = Math.Clamp(
            MinIntensity + (MaxIntensity - MinIntensity) * ratio,
            MinIntensity,
            MaxIntensity
            );

        var intensity = (int)intensityDouble;

        return new LightIntensity(intensity);
    }
}
