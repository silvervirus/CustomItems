using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Handlers;
using UnityEngine;

namespace CustomItems.Core;

public class DecoFabricator
{
    public static CraftTree.Type DecoFab;
    public static void LoadDecoFabricator()
    {
        var prefab1 = new CustomPrefab("DecorationFabricator", "More Decoration Fabricator",
            "Enjoy More Decoration Items all placeable plus lockers", RamuneLib.Utils.ImageUtils.GetSprite("MoreDeco"));
        var fabTree1 = prefab1.CreateFabricator(out CraftTree.Type GetGood);
        var model = new FabricatorTemplate(prefab1.Info, GetGood)
        {
            FabricatorModel = FabricatorTemplate.Model.Fabricator,
            ModifyPrefab = go =>
            {
                var renderer = go.GetComponentInChildren<SkinnedMeshRenderer>(true);


            }
        };
        prefab1.SetGameObject(model);
        prefab1.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Titanium, 1), new CraftData.Ingredient(TechType.Peeper, 2),
            new CraftData.Ingredient(TechType.JeweledDiskPiece, 1)));
        prefab1.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule);
        prefab1.SetUnlock(TechType.Peeper);
        prefab1.Register();


        CraftTreeHandler.AddTabNode(GetGood, "DeoLocker", "Decoration Lockers", RamuneLib.Utils.ImageUtils.GetSprite("LockerTab"));
        CraftTreeHandler.AddTabNode(GetGood, "Lab", "Lab Equipment", RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"));
        CraftTreeHandler.AddTabNode(GetGood, "Decegg", "Decoration Eggs", RamuneLib.Utils.ImageUtils.GetSprite("EGG"));
        CraftTreeHandler.AddTabNode(GetGood, "DeoOg", "Default Decoration", RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"));
        CraftTreeHandler.AddTabNode(GetGood, "DeoDolls", "Decoration Dolls", RamuneLib.Utils.ImageUtils.GetSprite(TechType.ArcadeGorgetoy));
        CraftTreeHandler.AddTabNode(GetGood, "DeoBags", "Decoration Bags", RamuneLib.Utils.ImageUtils.GetSprite(TechType.LuggageBag));
        CraftTreeHandler.AddTabNode(GetGood, "DeoFood", "Decoration Food", RamuneLib.Utils.ImageUtils.GetSprite("DeoFoodTab"));
        CraftTreeHandler.AddTabNode(GetGood, "Deo", "Decoration", RamuneLib.Utils.ImageUtils.GetSprite("Ship"));
        CraftTreeHandler.AddTabNode(GetGood, "Deoalien", "Decoration Alien", RamuneLib.Utils.ImageUtils.GetSprite("A"));

        DecoFab = GetGood;
    }
}