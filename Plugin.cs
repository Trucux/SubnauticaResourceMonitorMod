using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using Nautilus.Handlers;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ResourceMonitor
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    internal class Plugin : BaseUnityPlugin
    {
        public static string MOD_FOLDER_LOCATION { get; private set; }
        public static string ASSETS_FOLDER_LOCATION { get; private set; }
        public static string ASSET_BUNDLE_LOCATION { get; private set; }
        public static string SETTINGS_FILE_LOCATION { get; private set; }
        public static string DONT_TRACK_LOCATION { get; private set; }

        public static GameObject RESOURCE_MONITOR_DISPLAY_UI_PREFAB { get; private set; }
        public static GameObject RESOURCE_MONITOR_DISPLAY_ITEM_UI_PREFAB { get; private set; }
        public static GameObject RESOURCE_MONITOR_DISPLAY_MODEL { get; private set; }

        public new static ManualLogSource Logger { get; private set; }
        public static Options Options { get; private set; }

        private const string MODNAME = "ResourceMonitor";
        private const string AUTHOR = "taylor";
        private const string GUID = "taylor.brett.ResourceMonitor.mod";
        private const string VERSION = "2.0.33";

        public void Awake()
        {
            // Setup Project Logger
            Logger = base.Logger;

            // Determine the folder paths based on file existence
            SetModPaths();

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

        private static void SetModPaths()
        {
            // Primary paths in BepInEx folder
            string bepInExModFolder = "./BepInEx/plugins/ResourceMonitor/";
            string bepInExAssetsFolder = bepInExModFolder + "Assets/";

            // Fallback paths in QMods folder
            string qModsModFolder = "./QMods/ResourceMonitor/";
            string qModsAssetsFolder = qModsModFolder + "Assets/";

            // Check if files exist in BepInEx first, then fall back to QMods if not found
            if (Directory.Exists(bepInExAssetsFolder) && File.Exists(bepInExModFolder + "DontTrackList.txt"))
            {
                MOD_FOLDER_LOCATION = bepInExModFolder;
                ASSETS_FOLDER_LOCATION = bepInExAssetsFolder;
            }
            else if (Directory.Exists(qModsAssetsFolder) && File.Exists(qModsModFolder + "DontTrackList.txt"))
            {
                MOD_FOLDER_LOCATION = qModsModFolder;
                ASSETS_FOLDER_LOCATION = qModsAssetsFolder;
            }
            else
            {
                Logger.LogError("ResourceMonitor - Could not find required folders or files in either BepInEx or QMods locations.");
                return;
            }

            // Set specific file paths based on chosen folder
            ASSET_BUNDLE_LOCATION = ASSETS_FOLDER_LOCATION + "resources";
            SETTINGS_FILE_LOCATION = MOD_FOLDER_LOCATION + "Settings.json";
            DONT_TRACK_LOCATION = MOD_FOLDER_LOCATION + "DontTrackList.txt";

            Logger.LogInfo($"ResourceMonitor - Using MOD_FOLDER_LOCATION: {MOD_FOLDER_LOCATION}");
            Logger.LogInfo($"ResourceMonitor - Using ASSETS_FOLDER_LOCATION: {ASSETS_FOLDER_LOCATION}");
        }

        private static void RegisterPrefabs()
        {
            ResourceMonitorLargePrefab.Register();
            ResourceMonitorSmallPrefab.Register();
        }

        private static void LoadAssets()
        {
            if (File.Exists(ASSET_BUNDLE_LOCATION))
            {
                var ab = AssetBundle.LoadFromFile(ASSET_BUNDLE_LOCATION);
                RESOURCE_MONITOR_DISPLAY_UI_PREFAB = ab.LoadAsset("ResourceMonitorDisplayUI") as GameObject;
                RESOURCE_MONITOR_DISPLAY_ITEM_UI_PREFAB = ab.LoadAsset("ResourceItem") as GameObject;
                RESOURCE_MONITOR_DISPLAY_MODEL = ab.LoadAsset("ResourceMonitorModel") as GameObject;
            }
            else
            {
                Logger.LogError("[ResourceMonitor] Could not find asset bundle at " + ASSET_BUNDLE_LOCATION);
            }
        }

        private static void LoadDontTrackList()
        {
            if (File.Exists(DONT_TRACK_LOCATION))
            {
                Logger.LogInfo("[ResourceMonitor] Found the DontTrack list at location: " + DONT_TRACK_LOCATION);
                using (var reader = new StreamReader(DONT_TRACK_LOCATION))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Components.ResourceMonitorLogic.DONT_TRACK_GAMEOBJECTS.Add(line.ToLower());
                        }
                    }
                }
                Components.ResourceMonitorLogic.DONT_TRACK_GAMEOBJECTS.Sort();
            }
            else
            {
                Logger.LogInfo("[ResourceMonitor] Did not find the DontTrack list at location: " + DONT_TRACK_LOCATION);
            }
        }
    }
}
