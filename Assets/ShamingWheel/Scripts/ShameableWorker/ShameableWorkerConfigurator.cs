using Bindito.Core;
using Bindito.Unity;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.TemplateSystem;
using Timberborn.WorkSystem;

namespace FrostyMods.ShamingWheel {

	[Configurator(SceneEntrypoint.InGame)]
	public class ShameableWorkerConfigurator : PrefabConfigurator {
		public override void Configure(IContainerDefinition containerDefinition) {
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule() {
			TemplateModule.Builder builder = new();
			builder.AddDecorator<Worker, ShameableWorker>();
			return builder.Build();
		}
    }
}