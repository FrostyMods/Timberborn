using HarmonyLib;
using System;
using System.Reflection;
using Timberborn.SingletonSystem;

namespace FrostyMods.NightLight
{
    // Easier to just redefine this enum,
    // since the original is internal and basic
    public enum DayStage
    {
        Sunrise,
        Day,
        Sunset,
        Night
    }

    [HarmonyPatch]
    public class DayStagePatch : ILoadableSingleton
    {
        public void Load() {}

        [HarmonyTargetMethod]
        public static MethodBase FindPrivateType()
        {
            return AccessTools.Method(AccessTools.TypeByName("Timberborn.SkySystem.Sun"), "DayStageColors", (Type[])null, (Type[])null);
        }

        [HarmonyPostfix]
        public static bool Prefix(DayStage dayStage, ref object __result, ref object ____sunriseColors, ref object ____dayColors, ref object ____sunsetColors, ref object ____nightColors)
        {
            __result = dayStage switch
            {
                DayStage.Sunrise => ____sunsetColors, //___sunriseColors,
                DayStage.Day => ____sunsetColors, //___dayColors,
                DayStage.Sunset => ____sunsetColors,
                DayStage.Night => ____sunsetColors, //___nightColors,
                _ => throw new ArgumentOutOfRangeException("dayStage", dayStage, null),
            };

            return false;
        }
    }
}
