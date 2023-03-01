using System;
using System.Collections.Generic;
using System.Reflection;
using Bindito.Core;
using HarmonyLib;
using TimberApi.AssetSystem;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace FrostyMods.ShamingWheel
{
    /// <summary>
    /// Adds our custom AudioClips to the AudioClipService by patching the ReloadSounds method.
    /// It loads the audio clips from our asset bundle and adds them to the _audioClips field
    /// after ReloadSounds returns.
    /// </summary>
    [HarmonyPatch]
    public class AudioClipServicePatch : ILoadableSingleton
    {
        private static IAssetLoader _assetLoader;

        /*
         * This hopelessly typeless vagabond is the AudioClipService
         * instance we get from our Postfix method. We need it for when
         * we invoke the ReloadSounds method
         */
        private static object _audioClipService;

        /*
         * This feels a bit trashy, but it simplifies the logic surrounding
         * when and when not to log errors. There's no point logging an error
         * when the singleton isn't even ready. We'll just wait for it to
         * call InvokeReloadSounds
        */
        private static bool ready = false;

        /*
         * TimberApi injects the asset loader through this constructor.
         * The ILoadableSingleton interface (along with the AudioClipServicePatchConfigurator
         * class below) is required to actually make this work
         */
        public AudioClipServicePatch(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        /*
         * AudioClipService is pretty much guaranteed to be called before
         * the asset loader is initialized, so we wait for it to load and then
         * we invoke the ReloadSounds method on our AudioClipService instance
         */
        public void Load()
        {
            ready = true;
            InvokeReloadSounds();
        }

        /*
         * AudioClipService is an internal class, so we have to
         * tell it what to patch by using auxilary patch methods.
         * https://harmony.pardeike.net/articles/patching-auxilary.html#targetmethod
         */
        [HarmonyTargetMethod]
        public static MethodBase FindPrivateType()
        {
            return AccessTools.Method(AccessTools.TypeByName("Timberborn.SoundSystem.AudioClipService"), "ReloadSounds", (Type[])null, (Type[])null);
        }

        /*
         * The method we're targeting doesn't actually return anything.
         * It loads AudioClips from a fixed directory and assigns them to
         * the _audioClips field. This method is fired after ReloadSounds
         * returns. It loads our custom clips and adds them to the _audioClips field
         */
        [HarmonyPostfix]
        private static void Postfix(Dictionary<string, AudioClip> ____audioClips, object __instance)
        {
            _audioClipService = __instance;

            if (ready)
            {
                foreach (var customClip in _assetLoader.LoadAll<AudioClip>("frostymods.shamingwheel/shamingwheel"))
                {
                    ____audioClips.Add(customClip.name, customClip);
                    Plugin.Log.LogInfo("Added " + customClip.name + " to the AudioClipService");
                }
            }
        }

        /*
         * Invokes the AudioClipService.ReloadSounds method so our
         * asset injection has more than just a fart's chance in a hurricane
         */
        public static void InvokeReloadSounds()
        {
            if (_audioClipService != null)
            {
                MethodBase methodBase = AudioClipServicePatch.FindPrivateType();
                methodBase.Invoke(_audioClipService, null);
            }
        }

    }

    /*
     * Configurators let us bind classes to the dependency injection system
     * Our patch will still run without this, but it won't do anything because
     * the asset loader won't be given to us
     */
    [Configurator(SceneEntrypoint.InGame)]
    public class AudioClipServicePatchConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<AudioClipServicePatch>().AsSingleton();
        }
    }
}

