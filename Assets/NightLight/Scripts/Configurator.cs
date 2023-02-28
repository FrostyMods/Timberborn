using Bindito.Core;
using FrostyMods.NightLight;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

namespace FrostyMods.NightLight
{
    public class DayCycleUIConfigurator
    {
        [Configurator(SceneEntrypoint.InGame)]
        public class ScheduleSystemConfigurator : IConfigurator
        {
            public void Configure(IContainerDefinition containerDefinition)
            {
                containerDefinition.Bind<DayCycleUI>().AsSingleton();
            }
        }
    }
}