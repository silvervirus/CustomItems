using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Extensions;
using UnityEngine;

namespace CustomItems.Util;

public static class DynamicKnifeRegistrar
{
    public static void RegisterAll()
    {
        var allKnives = KnifeLoader.LoadAllKnifeData();

        foreach (var knifeEntry in allKnives)
        {
            var data = knifeEntry.Value;

            var info = PrefabInfo
                .WithTechType(data.InternalName, data.FriendlyName, data.Tooltip)
                .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite(data.Spritename));

            var customPrefab = new CustomPrefab(info);

            var template = new CloneTemplate(info, TechType.HeatBlade);
            template.ModifyPrefab += obj =>
            {
                var baseHeatBlade = obj.GetComponent<HeatBlade>();
                var yeetKnife = obj.AddComponent<CustomHotKnife>().CopyComponent(baseHeatBlade);
                Object.DestroyImmediate(baseHeatBlade);

                yeetKnife.Configure(data.Hitforce, data.Damage);
            };

            customPrefab.SetGameObject(template);
            customPrefab.SetRecipe(new RecipeData(new CraftData.Ingredient(TechType.Titanium, 2)));
            customPrefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
            customPrefab.Register();
        }
    }
}
