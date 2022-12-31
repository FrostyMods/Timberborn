using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace Frost.ShamingWheel
{
    [HarmonyPatch]
    public class Plugin : IModEntrypoint
    {
        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            var harmony = new Harmony("com.frostymods.timberborn.shamingwheel");
            harmony.PatchAll();
            Log = consoleWriter;
        }
    }
}

