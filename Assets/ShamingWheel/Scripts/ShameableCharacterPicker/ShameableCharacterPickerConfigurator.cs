using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.TemplateSystem;
using TimberApi.EntityLinkerSystem;
using FrostyMods.ShamingWheel.ShameableCharacters;

namespace FrostyMods.ShamingWheel.ShameableCharacterPicker
{
    [Configurator(SceneEntrypoint.InGame)]
    public class ShameableCharacterPickerConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ShameableCharacterPickerFragment>().AsSingleton();
            containerDefinition.Bind<ShameableCharacterPickerViewFactory>().AsSingleton();
            containerDefinition.Bind<ShameableCharacterPickerButton>().AsSingleton();

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
            private readonly ShameableCharacterPickerFragment _pickerFragment;

            public EntityPanelModuleProvider(ShameableCharacterPickerFragment pickerFragment)
            {
                _pickerFragment = pickerFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddTopFragment(_pickerFragment);
                return builder.Build();
            }
        }
    }
}
