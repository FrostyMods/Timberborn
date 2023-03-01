using HarmonyLib;
using Timberborn.BeaversUI;
using UnityEngine;

namespace FrostyMods.ShamingWheel
{

    /// <summary>
    /// Patch BeaverSelectionSound so shamed beavers always sound sad.
    /// It's far easier to simply patch the GetStateKey method instead of the PlaySound method
    /// </summary>
    [HarmonyPatch(typeof(BeaverSelectionSound), "GetStateKey")]
    public static class ShameableWorkerSelectionSound
    {
        public static bool Prefix(BeaverSelectionSound __instance, ref string __result, ref string ___DiscontentKey)
        {
            ShameableWorker shamedWorker = __instance.GetComponent<ShameableWorker>();
            if ((bool)(Object)(object)shamedWorker && shamedWorker.IsShamed)
            {
                __result = ___DiscontentKey;
                return false;
            }
            return true;
        }
    }
}