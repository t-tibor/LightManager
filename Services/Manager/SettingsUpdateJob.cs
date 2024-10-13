using LightManager.Services.LightBulb;
using Quartz;
using System.Diagnostics;

namespace LightManager.Services.Manager
{
    public class SettingsUpdateJob(
        ILightBulbController controller,
        ILightingManager lightingManager
        ) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                var settings = lightingManager.CalculateLightSettings();

                await controller.SetSettings(settings);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                throw new JobExecutionException(ex, false);
            }

        }
    }
}
