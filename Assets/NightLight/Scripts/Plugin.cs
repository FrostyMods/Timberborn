using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace FrostyMods.NightLight
{
    [HarmonyPatch]
    public class Plugin : IModEntrypoint {
        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter) {
            var harmony = new Harmony("com.frostymods.timberborn.nightlight");
            harmony.PatchAll();
            Log = consoleWriter;
        }
    }
}