
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Handlers;
using Nautilus.Utility;

namespace ResourceMonitor
{
    public static class ResourceMonitorLargePrefab
    {
        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType("ResourceMonitorBuildableLarge", "Resource Monitor Large",
            "Track how many resources you have stored away in your sea base on one handy large screen.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(Plugin.LARGE_PNG_LOCATION));

        public static void Register()
        {
            // Create a new custom prefab
            var prefab = new CustomPrefab(Info);

            // Setup a class to hold the data for this prefab and modify object creation based on the prefab
            ResourceMonitor monitorLarge = new ResourceMonitor(Info, true);

            // Assign the model to the prefab itself
            prefab.SetGameObject(monitorLarge.CreateObject);

            // Assign it to the correct tab in the builder tool
            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);

            // Unlock it at the start
            KnownTechHandler.UnlockOnStart(Info.TechType);

            // Register it into the game
            prefab.Register();
        }
    }
}
