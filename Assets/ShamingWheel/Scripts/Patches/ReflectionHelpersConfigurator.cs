using Bindito.Core;
using FrostyMods.Common;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;

/*
 * Credit goes to @TobbyTheBobby
 * https://github.com/TobbyTheBobby/TimberbornModsUnity/blob/master/Assets/ChooChoo/Scripts/Core/ChooChooCore.cs
 */

namespace FrostyMods.ShamingWheel
{
    [Configurator(SceneEntrypoint.InGame)]
    public class HelpersConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ReflectionHelpers>().AsSingleton();
        }
    }
}
