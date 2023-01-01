using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using FrostyMods.ShamingWheel.WorkerLinker;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;
using Timberborn.WorkSystem;
using Timberborn.Buildings;
using TimberApi.EntityLinkerSystem;
using FrostyMods.ShamingWheel.ShameableCharacters;

namespace FrostyMods.ShamingWheel.WorkerLinker
{
    [Configurator(SceneEntrypoint.InGame)]
    public class ShamedWorkerLinkConfigurator : IConfigurator
    {
        // Bind our custom classes
        public void Configure(IContainerDefinition containerDefinition)
        {
//            containerDefinition.Bind<EntityLinkObjectSerializer>().AsSingleton();
            containerDefinition.Bind<ShamedWorkerLinkerFragment>().AsSingleton();
            containerDefinition.Bind<ShamedWorkerLinkViewFactory>().AsSingleton();
            containerDefinition.Bind<ShamedWorkerLinkerButton>().AsSingleton();

            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<ShameableCharacter, EntityLinker>();
            return builder.Build();
        }

        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly ShamedWorkerLinkerFragment _linkerFragment;

            public EntityPanelModuleProvider(ShamedWorkerLinkerFragment linkerFragment)
            {
                _linkerFragment = linkerFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_linkerFragment);
                return builder.Build();
            }
        }
    }
}
