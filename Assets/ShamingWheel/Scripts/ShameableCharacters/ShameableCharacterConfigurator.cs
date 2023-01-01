using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.Characters;
using Timberborn.TemplateSystem;

namespace FrostyMods.ShamingWheel.ShameableCharacters
{

	[Configurator(SceneEntrypoint.InGame)]
	public class ShameableCharacterConfigurator : IConfigurator
	{
		public void Configure(IContainerDefinition containerDefinition)
		{
			containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
		}

		private static TemplateModule ProvideTemplateModule()
		{
			TemplateModule.Builder builder = new();
			builder.AddDecorator<Character, ShameableCharacter>();
			return builder.Build();
		}
	}
}