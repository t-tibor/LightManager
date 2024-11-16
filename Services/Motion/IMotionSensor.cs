namespace LightManager.Services.Motion
{
    public interface IMotionSensor
    {
        IObservable<bool> Occupancy {get;}
    }
}
