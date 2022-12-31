using HarmonyLib;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;
using TimberApi.AssetSystem;
using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.SingletonSystem;

namespace Frost.ShamingWheel
{
    /// <summary>
    /// Adds our custom AudioClips to the AudioClipService by patching the ReloadSounds
    /// method, loading the audio clips from our asset bundle, and injecting them into the
    /// list of available AudioClips.
    /// </summary>
    [HarmonyPatch]
    public class AudioClipServicePatch : ILoadableSingleton
    {
        public static string Path = "frostymods.shamingwheel/shamingwheel";

        private static IAssetLoader _assetLoader;

        private static object _audioClipService;

        private static bool ready = false;

        public AudioClipServicePatch(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public void Load()
        {
            ready = true;
            TriggerReload();
        }

        [HarmonyTargetMethod]
        public static MethodBase FindPrivateType()
        {
            return AccessTools.Method(AccessTools.TypeByName("Timberborn.SoundSystem.AudioClipService"), "ReloadSounds", (Type[])null, (Type[])null);
        }

        [HarmonyPostfix]
        private static void Postfix(Dictionary<string, AudioClip> ____audioClips, object __instance)
        {
            _audioClipService = __instance;

            if (ready)
            {
                try
                {
                    foreach (var customClip in _assetLoader.LoadAll<AudioClip>(Path))
                    {
                        ____audioClips.Add(customClip.name, customClip);
                        Plugin.Log.LogInfo("Added " + customClip.name + " to the AudioClipService");
                    }

                }
                catch (Exception e)
                {
                    Plugin.Log.LogWarning("Encountered an exception while loading the custom audio clips. Reason: " + e.Message);
                }
            }
        }

        public static void TriggerReload()
        {
            if (_audioClipService != null)
            {
                MethodBase methodBase = AudioClipServicePatch.FindPrivateType();
                methodBase.Invoke(_audioClipService, null);
            }
        }

    }

    [Configurator(SceneEntrypoint.InGame)]
    public class AudioClipServicePatchConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<AudioClipServicePatch>().AsSingleton();
        }
    }
}

