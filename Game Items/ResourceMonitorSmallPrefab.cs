
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using UnityEngine;

namespace ResourceMonitor
{
    public static class ResourceMonitorSmallPrefab
    {
        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType("ResourceMonitorBuildableSmall", "Resource Monitor Small",
            "Track how many resources you have stored away in your sea base on one handy small screen.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(Plugin.ASSETS_FOLDER_LOCATION, "ResourceMonitorSmall.png")));

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

            // Set recipe:
            prefab.SetRecipe(monitorSmall.GetRecipe());

            // Register it into the game:
            prefab.Register();
        }
    }

}
