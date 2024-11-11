
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using Nautilus.Utility;

namespace ResourceMonitor
{
    public static class ResourceMonitorSmallPrefab
    {
        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType("ResourceMonitorBuildableSmall", "Resource Monitor Small",
            "Track how many resources you have stored away in your sea base on one handy small screen.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(Plugin.SMALL_PNG_LOCATION));

        public static void Register()
        {
            // Create a new custom prefab:
            var prefab = new CustomPrefab(Info);

            // Setup a class to hold the data for this prefab and modify object creation based on the prefab
            ResourceMonitor monitorSmall = new ResourceMonitor(Info, false);

            // Assign the model to the prefab itself:
            prefab.SetGameObject(monitorSmall.CreateObject);

            // Assign it to the correct tab in the builder tool:
            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);

            // Unlock it at the start
            KnownTechHandler.UnlockOnStart(Info.TechType);

            // Register it into the game:
            prefab.Register();
        }
    }

}
