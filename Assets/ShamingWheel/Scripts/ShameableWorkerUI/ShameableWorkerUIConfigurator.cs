using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;

namespace FrostyMods.ShamingWheel {
    [Configurator(SceneEntrypoint.InGame)]
    public class ShamingSystemUIConfigurator : IConfigurator {
        private class EntityPanelModuleProvider : IProvider<EntityPanelModule> {
            private readonly ShameButtonFragment _shameButtonFragment;

            public EntityPanelModuleProvider(ShameButtonFragment shamingSystemFragment) {
                _shameButtonFragment = shamingSystemFragment;
            }

            public EntityPanelModule Get() {
                EntityPanelModule.Builder builder = new();
                builder.AddBottomFragment(_shameButtonFragment);
                return builder.Build();
            }
        }

        public void Configure(IContainerDefinition containerDefinition) {
            containerDefinition.Bind<ShameButtonFragment>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }
    }
}