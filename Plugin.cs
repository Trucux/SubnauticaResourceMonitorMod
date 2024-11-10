using System;
using HarmonyLib;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using System.IO;
using BepInEx.Logging;
using Nautilus.Handlers;
using System.Security.Cryptography;

namespace ResourceMonitor
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    internal class Plugin : BaseUnityPlugin
    {
        public const string BepInExDir = "./BepInEx/plugins/ResourceMonitor/";
        public const string QModsDir = "./QMods/ResourceMonitor/";

        public static string MOD_FOLDER_LOCATION;
        public static string ASSETS_FOLDER_LOCATION;
        public static string ASSET_BUNDLE_LOCATION;
        public static string DONT_TRACK_LOCATION;

        public static GameObject RESOURCE_MONITOR_DISPLAY_UI_PREFAB { get; private set; }
        public static GameObject RESOURCE_MONITOR_DISPLAY_ITEM_UI_PREFAB { get; private set; }
        public static GameObject RESOURCE_MONITOR_DISPLAY_MODEL { get; private set; }

        public new static ManualLogSource Logger { get; private set; }
        public static Options Options { get; private set; }

        #region[Declarations]
        private const string
            MODNAME = "ResourceMonitor",
            AUTHOR = "BrettTaylor & 0x4b",
            GUID = "taylor.brett.ResourceMonitor.mod",
            VERSION = "2.0.35";
        #endregion

        public void Awake()
        {
            // Determine the folder paths for each file separately
            MOD_FOLDER_LOCATION = BepInExDir;
            ASSETS_FOLDER_LOCATION = Directory.Exists(BepInExDir + "Assets/") ? BepInExDir + "Assets/" : QModsDir + "Assets/";
            ASSET_BUNDLE_LOCATION = File.Exists(BepInExDir + "Assets/resources") ? BepInExDir + "Assets/resources" : QModsDir + "Assets/resources";
            DONT_TRACK_LOCATION = File.Exists(BepInExDir + "DontTrackList.txt") ? BepInExDir + "DontTrackList.txt" : QModsDir + "DontTrackList.txt";

            // Check if required files and folders exist based on the resolved paths
            bool assetsFolderExists = Directory.Exists(ASSETS_FOLDER_LOCATION);
            bool assetBundleExists = File.Exists(ASSET_BUNDLE_LOCATION);
            bool dontTrackListExists = File.Exists(DONT_TRACK_LOCATION);

            // Setup Project Logger
            Logger = base.Logger;

            // Verify Files were found 
            if (!assetsFolderExists || !assetBundleExists || !dontTrackListExists)
            {
                // Log an error message with instructions
                Logger.LogError("ResourceMonitor Unofficial Patch is installed incorrectly!");
                Logger.LogError("Please install the original mod to the QMods folder via Vortex.");
                Logger.LogError("If you chose to install the original to the BepInEx/plugins folder, you must overwrite the DLL with the patch!");

                // Return early to prevent further execution
                return;
            }

            // Load Options from the BepInEx config and setup the options menu
            Options = OptionsPanelHandler.RegisterModOptions<Options>();

            // Run harmony patches
            Logger.LogInfo("ResourceMonitor - Started patching v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
            var harmony = new Harmony(GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            LoadDontTrackList();
            LoadAssets();
            RegisterPrefabs();

            Logger.LogInfo("ResourceMonitor - Finished patching");
        }


        private static void RegisterPrefabs()
        {
            ResourceMonitorLargePrefab.Register();
            ResourceMonitorSmallPrefab.Register();
        }

        private static void LoadAssets()
        {
            var ab = AssetBundle.LoadFromFile(ASSET_BUNDLE_LOCATION);
            RESOURCE_MONITOR_DISPLAY_UI_PREFAB = ab.LoadAsset("ResourceMonitorDisplayUI") as GameObject;
            RESOURCE_MONITOR_DISPLAY_ITEM_UI_PREFAB = ab.LoadAsset("ResourceItem") as GameObject;
            RESOURCE_MONITOR_DISPLAY_MODEL = ab.LoadAsset("ResourceMonitorModel") as GameObject;
        }
        private static void LoadDontTrackList()
        {
            if (File.Exists(DONT_TRACK_LOCATION))
            {
                Logger.LogInfo("[ResourceMonitor] Found the dont track list at location: " + DONT_TRACK_LOCATION);
                using (var reader = new StreamReader(DONT_TRACK_LOCATION))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line) == false)
                        {
                            Components.ResourceMonitorLogic.DONT_TRACK_GAMEOBJECTS.Add(line.ToLower());
                        }
                    }
                }
                Components.ResourceMonitorLogic.DONT_TRACK_GAMEOBJECTS.Sort();
            }
            else
            {
                Logger.LogInfo("[ResourceMonitor] Did not find the dont track list at location: " + DONT_TRACK_LOCATION);
            }
        }
    }
}
