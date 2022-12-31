using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.Buildings;
using HarmonyLib;

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
            Log.LogInfo($"The Shaming Wheel is loaded!");
        }
    }

    /// <summary>
    /// Patch BeaverSelectionSound so shamed beavers always sound sad.
    /// It's far easier to simply patch the GetStateKey method instead of the PlaySound method
    /// </summary>
    [HarmonyPatch(typeof(BuildingSelectionSound), "OnSelect")]
    public class BuildingSelectionSoundPatch
    { 

        public static void Prefix(BuildingSelectionSound __instance)
        {
            Plugin.Log.LogInfo("Building's sound base name is " + __instance.GetComponent<Building>().SoundBaseName);
            Plugin.Log.LogInfo("Building's prefab name is " + __instance.GetComponent<Building>().name);
            Plugin.Log.LogInfo("Building's loc key is " + __instance.GetComponent<Building>().DisplayNameLocKey);

            //var clip = _assetLoader.Load<AudioClip>("frost.shamingwheel/shamingwheel/UI.Buildings.Selected.ShamingWheel");

            //Plugin.Log.LogInfo("Loaded clip: " + clip.name);

            // ____soundSystem.PlaySound2D(__instance.gameObject, "UI.Buildings.Selected.ShamingWheel", 10);
            // return false;
        }
    }

    /*
    public class AudioLoader : ILoadableSingleton {
        private static IAssetLoader _assetLoader;

        public AudioLoader(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), (string)null);
            Plugin.Log.LogInfo("AudioLoader Loaded.");

        }

        [HarmonyPatch]
        public static class AudioClipService_Patch
        {
            [HarmonyTargetMethod]
            public static MethodBase FindPrivateType()
            {
                return AccessTools.Method(AccessTools.TypeByName("Timberborn.SoundSystem.AudioClipService"), "ReloadSounds", (Type[])null, (Type[])null);
            }

            [HarmonyPostfix]
            public static void AddOurAudioClips(Dictionary<string, AudioClip> ____audioClips, object __instance)
            {
                AudioClip[] clips = _assetLoader.LoadAll<AudioClip>("frost.shamingwheel/shamingwheel");


                Plugin.Log.LogInfo($"AudioLoader : injecting {clips.Length} clips...");
                _ACSInstance = __instance;


                foreach (AudioClip newClip in clips)
                {
                    if (____audioClips.ContainsKey(newClip.name))
                    {
                        Plugin.Log.LogWarning("Audio Clips dictionary already contains a clip named `" + newClip.name + "`. This extra clip not injected.");
                    }
                    else
                    {
                        ____audioClips.Add(newClip.name, newClip);
                    }
                }
                Plugin.Log.LogInfo("---Injection Complete! Hurray for vaccines!");
                foreach (KeyValuePair<string, AudioClip> ____audioClip in ____audioClips)
                {
                    Plugin.Log.LogInfo(____audioClip.Key);
                }
            }
        }

        [HarmonyPatch]
        public static class AddAudioMixerGroup_Patch
        {
            [HarmonyTargetMethod]
            public static MethodBase FindPrivateType()
            {
                return AccessTools.Method(AccessTools.TypeByName("Timberborn.SoundSystem.AudioMixerGroupRetriever"), "AddAudioMixerGroup", (Type[])null, (Type[])null);
            }

            [HarmonyPrefix]
            public static void AddAudioMixerGroup(string audioMixerGroupName)
            {
                Plugin.Log.LogInfo("AddAudioMixerGroup : " + audioMixerGroupName);
            }
        }

        public static AudioLoader Instance = null;

        private static Dictionary<string, AudioClip> _newClips = new Dictionary<string, AudioClip>();

        private static object _ACSInstance = null;

        public static AudioLoader I => Instance;
        public bool Load(byte[] data, string clipName, AudioType audioType)
        {
            _assetLoader.LoadAll<AudioClip>("frost.shamingwheel/shamingwheel");
            string tempFileName = Path.GetTempFileName();
            try
            {
                Plugin.Log.LogInfo("Loading bytes into $" + tempFileName);
                File.WriteAllBytes(tempFileName, data);
                return Load(tempFileName, clipName, audioType);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError("Exception occured while trying to write temp file: " + tempFileName + "\n        Details follow:");
                Debug.LogException(ex);
                return false;
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }

        public bool Load(string filePath, string clipName, AudioType audioType)
        {
            if (_newClips.ContainsKey(clipName))
            {
                Plugin.Log.LogWarning("We've already imported a clip with the name `" + clipName + "`. Skipping file " + filePath);
            }
            try
            {
                UnityWebRequest audioClip = UnityWebRequestMultimedia.GetAudioClip(filePath, audioType);
                try
                {
                    UnityWebRequestAsyncOperation val = audioClip.SendWebRequest();
                    while (!((AsyncOperation)val).isDone)
                    {
                    }
                    AudioClip audioClip2 = ((DownloadHandlerAudioClip)audioClip.downloadHandler).audioClip;
                    _newClips.Add(clipName, audioClip2);
                }
                finally
                {
                    ((IDisposable)audioClip)?.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError("Exception occured while trying to load file: " + filePath + "\n        Details follow:");
                Debug.LogException(ex);
            }
            return false;
        }

        public void ForceReload()
        {
            if (_ACSInstance == null)
            {
                Plugin.Log.LogWarning("AudioLoader.ForceReload() called, but no ClipService instance has been discovered yet. Ignoring call.");
                return;
            }
            MethodBase methodBase = AudioClipService_Patch.FindPrivateType();
            methodBase.Invoke(_ACSInstance, null);
        }

        public void Load()
        {
            throw new NotImplementedException();
        }
    }*/
}

