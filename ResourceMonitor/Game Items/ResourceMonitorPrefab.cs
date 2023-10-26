using System.Collections.Generic;
using System.IO;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;

namespace ResourceMonitor
{
    public class MonitorBase
    {
        public static string AssetsFolder => Plugin.ASSETS_FOLDER_LOCATION;
        public static TechGroup GroupForPDA => TechGroup.InteriorModules;
        public static TechCategory CategoryForPDA => TechCategory.InteriorModule;
        public static RecipeData GetRecipe(int numIngredients)
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<CraftData.Ingredient>()
                {
                    new CraftData.Ingredient(TechType.Glass, numIngredients),
                    new CraftData.Ingredient(TechType.ComputerChip, numIngredients),
                    new CraftData.Ingredient(TechType.AdvancedWiringKit, numIngredients)
                }
            };
        }
    }

    public class MonitorLarge : MonitorBase
    {
        public static readonly Vector3 SCALE = new Vector3(2.3f, 2.3f, 1f);
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType(
            "ResourceMonitorBuildableLarge",
            "Resource Monitor Large",
            "Track how many resources you have stored away in your sea base on one handy large screen.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "ResourceMonitorLarge.png"))
        );
        public static GameObject GetGameObject()
        {
            // Create game object from scratch:
            var screen = Object.Instantiate(Plugin.RESOURCE_MONITOR_DISPLAY_MODEL);
            var screenModel = screen.transform.GetChild(0).gameObject;


            var shader = Shader.Find("MarmosetUBER");
            var renderers = screen.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.shader = shader;
            }

            var skyApplier = screen.AddComponent<SkyApplier>();
            skyApplier.renderers = renderers;
            skyApplier.anchorSky = Skies.Auto;

            // Make it Buildable:
            var constructable = screen.AddComponent<Constructable>();
            constructable.allowedOnWall = true;
            constructable.allowedInSub = true;
            constructable.allowedOnGround = false;
            constructable.allowedOutside = false;
            constructable.model = screenModel;
            constructable.techType = Info.TechType;

            screen.AddComponent<ConstructableBounds>().bounds = new OrientedBounds(new Vector3(-0.1f, -0.1f, 0f), new Quaternion(0, 0, 0, 0), new Vector3(0.9f, 0.5f, 0f));
            screen.AddComponent<TechTag>().type = Info.TechType;
            screen.AddComponent<PrefabIdentifier>().ClassId = Info.ClassID;
            screen.AddComponent<VFXSurface>();
            screen.AddComponent<Components.ResourceMonitorLogic>();
            // Modify scale of the model:
            screen.transform.localScale = SCALE;
            return screen;
        }

        public static void Register()
        {
            // Create a new custom prefab:
            var prefab = new CustomPrefab(Info);

            // Assign the model to the prefab itself:
            prefab.SetGameObject(GetGameObject);

            // Assign it to the correct tab in the builder tool:
            prefab.SetPdaGroupCategory(GroupForPDA, CategoryForPDA);

            // Set recipe:
            prefab.SetRecipe(GetRecipe(2));

            // Register it into the game:
            prefab.Register();
        }
    }

    public class MonitorSmall : MonitorBase
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType(
            "ResourceMonitorBuildableSmall",
            "Resource Monitor Small",
            "Track how many resources you have stored away in your sea base on one handy small screen.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(Path.Combine(AssetsFolder, "ResourceMonitorSmall.png"))
        );

        public static GameObject GetGameObject()
        {
            var screen = Object.Instantiate(Plugin.RESOURCE_MONITOR_DISPLAY_MODEL);
            var screenModel = screen.transform.GetChild(0).gameObject;

            var shader = Shader.Find("MarmosetUBER");
            var renderers = screen.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.shader = shader;
            }

            var skyApplier = screen.AddComponent<SkyApplier>();
            skyApplier.renderers = renderers;
            skyApplier.anchorSky = Skies.Auto;

            var constructable = screen.AddComponent<Constructable>();
            constructable.allowedOnWall = true;
            constructable.allowedInSub = true;
            constructable.allowedOnGround = false;
            constructable.allowedOutside = false;
            constructable.model = screenModel;
            constructable.techType = Info.TechType;

            screen.AddComponent<ConstructableBounds>().bounds = new OrientedBounds(new Vector3(-0.1f, -0.1f, 0f), new Quaternion(0, 0, 0, 0), new Vector3(0.9f, 0.5f, 0f));
            screen.AddComponent<TechTag>().type = Info.TechType;
            screen.AddComponent<PrefabIdentifier>().ClassId = Info.ClassID;
            screen.AddComponent<VFXSurface>();
            screen.AddComponent<Components.ResourceMonitorLogic>();
            return screen;
        }

        public static void Register()
        {
            // Create a new custom prefab:
            var prefab = new CustomPrefab(Info);

            // Assign the custom game object to the prefab:
            prefab.SetGameObject(GetGameObject);

            // Assign it to the correct tab in the builder tool:
            prefab.SetPdaGroupCategory(GroupForPDA, CategoryForPDA);

            // Set recipe:
            prefab.SetRecipe(GetRecipe(1));

            // Register it into the game:
            prefab.Register();
        }

    }
}