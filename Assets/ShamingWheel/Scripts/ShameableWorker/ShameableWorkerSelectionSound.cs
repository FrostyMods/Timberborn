/*using HarmonyLib;
using Timberborn.BeaversUI;
using Timberborn.BotsUI;
using UnityEngine;

namespace TheShamingWheel.ShameableCharacters;

/// <summary>
/// Patch BeaverSelectionSound so shamed beavers always sound sad.
/// It's far easier to simply patch the GetStateKey method instead of the PlaySound method
/// </summary>
[HarmonyPatch(typeof(BeaverSelectionSound), "GetStateKey")]
public static class BeaverSelectionSoundPatch
{
    public static bool Prefix(BeaverSelectionSound __instance, ref string __result, ref string ___DiscontentKey)
    {
        ShameableCharacter character = __instance.GetComponent<ShameableCharacter>();
        if ((bool)(Object)(object)character && character.IsShamed)
        {
            __result = ___DiscontentKey;
            return false;
        }
        return true;
    }
}

/// <summary>
/// Patch BotSelectionSound so shamed bots always sound sad.
/// It's far easier to simply patch the GetKey method instead of the PlaySound method
/// </summary>
[HarmonyPatch(typeof(BotSelectionSound), "GetKey")]
public static class BotSelectionSoundPatch
{
    public static bool Prefix(BotSelectionSound __instance, ref string __result, ref string ___DiscontentKey)
    {
        ShameableCharacter character = __instance.GetComponent<ShameableCharacter>();
        if ((bool)(Object)(object)character && character.IsShamed)
        {
            __result = ___DiscontentKey;
            return false;
        }
        return true;
    }
}*/