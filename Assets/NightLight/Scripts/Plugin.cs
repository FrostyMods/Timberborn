using FrostMods.NightLight;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

namespace FrostyMods.NightLight {
    // The original DayStage enum is internal
    // so we're defining our own here since it's
    // just a basic enum and we use it frequently
    public enum DayStage {
        Sunrise,
        Day,
        Sunset,
        Night
    }

    [HarmonyPatch]
    public class Plugin : IModEntrypoint {
        public static NightLightConfig Config;
        public static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter) {
            var harmony = new Harmony("com.frostymods.timberborn.nightlight");
            harmony.PatchAll();
            Config = mod.Configs.Get<NightLightConfig>();
            Log = consoleWriter;
        }
    }
}