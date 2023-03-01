using System;
using System.Reflection;
using HarmonyLib;

namespace FrostyMods.NightLight
{
    [HarmonyPatch]
    public class DayStageColorsPatch
    {
        [HarmonyTargetMethod]
        public static MethodBase FindPrivateType()
        {
            return AccessTools.Method(AccessTools.TypeByName("Timberborn.SkySystem.Sun"), "DayStageColors", (Type[])null, (Type[])null);
        }

        [HarmonyPostfix]
        public static bool Prefix(DayStage dayStage, ref object __result, ref object ____sunriseColors, ref object ____dayColors, ref object ____sunsetColors, ref object ____nightColors)
        {
            // Grab the preferred DayStage from the config file and use it for the switch instead.
            // e.g. if the user has set Night to be Day, then we return the day colours
            __result = Plugin.Config.GetMappedDayStage(dayStage) switch
            {
                DayStage.Sunrise => ____sunriseColors,
                DayStage.Day => ____dayColors,
                DayStage.Sunset => ____sunriseColors,
                DayStage.Night => ____nightColors,
                _ => throw new ArgumentOutOfRangeException("dayStage", dayStage, null),
            };

            return false;
        }
    }
}
