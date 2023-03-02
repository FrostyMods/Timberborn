using FrostMods.NightLight.Config;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace FrostyMods.NightLight
{
    [HarmonyPatch]
    public class Plugin : IModEntrypoint
    {
        public static LightingConfig Config;
        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            var harmony = new Harmony("com.frostymods.timberborn.nightlight");
            harmony.PatchAll();
            Config = mod.Configs.Get<LightingConfig>();
            Log = consoleWriter;
        }
    }
}