using System;
using System.Collections.Generic;
using CustomItems.Util;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using UnityEngine;
using static CustomItems.Util.UtraBasePrefab;
using static CustomItems.Util.Cookay_sUtil;

namespace CustomItems.Core;

public class BasicIngredents
{
    public static void LoadBasicIngredientsRequirements()
        {
            IRequirementsLoaderssss loader = new IRequirementsLoaderssss();
            Dictionary<string, EggData.EggInfoData> allEggInfo = loader.LoadAllEggInfo();

            if (allEggInfo.Count > 0)
            {
                foreach (var eggEntry in allEggInfo)
                {
                    try
                    {
                        //string eggName = eggEntry.Key;
                        EggData.EggInfoData eggData = eggEntry.Value;


                        // Debug output for egg data
                        //Debug.Log($"DecoName: {eggName}");
                        // Debug.Log($"Friendly Name: {eggData.FriendlyName}");
                        // Debug.Log($"Tooltip: {eggData.Tooltip}");
                        Debug.Log($"Internal Name: {eggData.InternalName}");
                        // Debug.Log($"Sprite Name: {eggData.Spritename}");
                        //  Debug.Log($"Object Name: {eggData.ObjectName}");

                        // Extract egg information from eggData
                        string eggDisplayName = eggData.FriendlyName;
                        string eggDescription = eggData.Tooltip;
                        string eggInterName = eggData.InternalName;

                        string eggObjectName = eggData.ObjectName;

                        string SpriteName = eggData.Spritename;
                        //string unlockTechtype = eggData.unlockTechtype;
                        // Convert eggData.TechType to TechType to get the sprite

                        //TechType setUnlockTechType = GetTechType(unlockTechtype);

                        foreach (string internalName in eggInterName.Split(','))
                        {

                            Atlas.Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(SpriteName);

                            // Create and register the custom egg prefab
                            UnifiedPrefabsCombined prefab = new UnifiedPrefabsCombined(internalName.Trim(),
                                eggDisplayName, eggDescription
                                , eggSprite);

                            // Create CloneTemplate using ResourceId
                            CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);

                            if (eggData.PPPIngredients||eggData.BasicIngredients)
                            {
                                if (eggData.PPPIfood || eggData.BasicFood)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.Peeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.CookedFood);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    if (eggData.UseDefaultTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            new string[]
                                            {
                                                "CIS",
                                                "CF3",
                                                eggData.TabName
                                                
                                            });
                                    }
                                    else if (eggData.UseCustomTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            ParsePath(eggData.Path));
                                    }
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PPPIdrink || eggData.BasicDrink)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.Peeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.CookedFood);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    if (eggData.UseDefaultTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            new string[]
                                            {
                                                "CIS",
                                                "CF3",
                                                eggData.TabName
                                                
                                            });
                                    }
                                    else if (eggData.UseCustomTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            ParsePath(eggData.Path));
                                    }
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PPPIItem || eggData.BasicItem)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj => { obj.EnsureComponent<Pickupable>(); };

                                    prefab.SetUnlock(TechType.Peeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.CookedFood);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    if (eggData.UseDefaultTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            new string[]
                                            {
                                                "CIS",
                                                "IDT"
                                                
                                            });
                                    }
                                    else if (eggData.UseCustomTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            ParsePath(eggData.Path));
                                    }
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                            }
                        }




                        // Split internal names and create/register the custom egg prefab for each TechType


                    }

                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception processing Ingredent {eggEntry.Key}: {ex.Message}");
                    }
                }
            }
            else

            {
                Debug.LogError("Failed to load Ingredient info data.");
            }
        }
}