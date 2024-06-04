using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Customitems.Loader;
using Customitems.info;
using Debug = UnityEngine.Debug;
using Nautilus.Utility;
using static HandReticle;
using PrefabUtils = Nautilus.Utility.PrefabUtils;


[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
public class QPatch : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "Com.Cookay.CustomItems";
    public const string PLUGIN_NAME = "CustomItems";
    public const string PLUGIN_VERSION = "1.0.0.0";
    private static readonly Harmony harmony = new Harmony(PLUGIN_GUID);
    public void Awake()
    {
        harmony.PatchAll();
        
        LoadEggRequirements();
        CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle", "Posters One", RamuneLib.Utils.ImageUtils.GetSprite(TechType.PosterKitty));
        CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle2", "Posters Two", RamuneLib.Utils.ImageUtils.GetSprite(TechType.PosterAurora));
        CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle1", "Food", RamuneLib.Utils.ImageUtils.GetSprite(TechType.NutrientBlock));
        CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle2", "Drink", RamuneLib.Utils.ImageUtils.GetSprite(TechType.FilteredWater));
    }
    private void LoadEggRequirements()
    {
        RequirementsLoader requirementsLoader = new RequirementsLoader();
        Dictionary<string, EggInfoData> eggInfoData = requirementsLoader.LoadEggInfo("CustomItems.json");

        if (eggInfoData != null)
        {
            foreach (var eggEntry in eggInfoData)
            {
                try
                {
                    string eggName = eggEntry.Key;
                    EggInfoData eggData = eggEntry.Value;

                    // Debug output for egg data
                    Debug.Log($"DecoName: {eggName}");
                    Debug.Log($"Friendly Name: {eggData.FriendlyName}");
                    Debug.Log($"Tooltip: {eggData.Tooltip}");
                    Debug.Log($"Internal Name: {eggData.InternalName}");
                    Debug.Log($"Sprite Name: {eggData.Spritename}");
                    Debug.Log($"Object Name: {eggData.ObjectName}");

                    // Extract egg information from eggData
                    string eggDisplayName = eggData.FriendlyName;
                    string eggDescription = eggData.Tooltip;
                    string eggInterName = eggData.InternalName;
                    string eggspriteName = eggData.Spritename;
                    string eggObjectName = eggData.ObjectName;

                    // Convert eggData.TechType to TechType to get the sprite
                    TechType eggTechType = GetTechType(eggspriteName);

                    // Split internal names and create/register the custom egg prefab for each TechType
                    foreach (string internalName in eggInterName.Split(','))
                    {
                        // Fetch the sprite
                        Atlas.Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(eggspriteName);

                        // Create and register the custom egg prefab
                        BasicEggPrefab prefab = new BasicEggPrefab(internalName.Trim(), eggDisplayName, eggDescription, eggObjectName, eggSprite);

                        // Create CloneTemplate using ResourceId
                        CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);

                        // Modify the CloneTemplate if needed
                        cloneTemplate.ModifyPrefab += obj =>
                        {
                            ConstructableFlags constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.Submarine | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Outside | ConstructableFlags.Base | ConstructableFlags.Wall |ConstructableFlags.Ceiling;

                            ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                            // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                            GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                            if (model == null)
                            {
                                Debug.LogError($"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                return;
                            }

                            // Log the GameObject found by the name
                            Debug.Log($"Found GameObject with name '{eggData.ObjectName}' for Deco '{eggName}': {model.name}");

                            // Handle different cases based on eggData properties
                            if (eggData.IsAnArtifact)
                            {
                                var animator = obj.GetComponentInChildren<Animator>();
                                if (animator != null)
                                {
                                    animator.enabled = false;
                                }
                                else
                                {
                                    Debug.LogWarning("Animator component not found.");
                                }

                                var skyApplier = obj.GetComponent<SkyApplier>();
                                if (skyApplier != null)
                                {
                                    GameObject.DestroyImmediate(skyApplier);
                                }

                                Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);

                                var capsuleCollider = obj.GetComponent<CapsuleCollider>();
                                if (capsuleCollider != null)
                                {
                                    GameObject.DestroyImmediate(capsuleCollider);
                                }

                                var collider = obj.AddComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f, 0.6f, 0.5f);
                                collider.center = new Vector3(0f, 0.3f, 0f);
                                collider.isTrigger = true;
                            }
                            else if (eggData.IsGunArtifact)
                            {
                                obj.transform.localPosition = new Vector3(0f, 1.85f, 0f);
                                var animator = obj.GetComponentInChildren<Animator>();
                                if (animator != null)
                                {
                                    animator.enabled = false;
                                }
                                else
                                {
                                    Debug.LogWarning("Animator component not found.");
                                }

                                GameObject.DestroyImmediate(obj.GetComponent<EntityTag>());
                                GameObject.DestroyImmediate(obj.GetComponent<CapsuleCollider>());
                                GameObject.DestroyImmediate(obj.GetComponent<SkyApplier>());

                                Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);

                                var collider = obj.AddComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f, 0.6f, 0.5f);
                                collider.center = new Vector3(0f, 0.3f, 0f);
                                collider.isTrigger = true;

                                obj.AddComponent<SkyApplier>();
                            }
                            else if (eggData.IsArtifact)
                            {
                                GameObject.DestroyImmediate(obj.GetComponent<SkyApplier>());
                                Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);

                                var capsuleCollider = obj.GetComponent<CapsuleCollider>();
                                if (capsuleCollider != null)
                                {
                                    GameObject.DestroyImmediate(capsuleCollider);
                                }

                                var collider = obj.AddComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f, 0.6f, 0.5f);
                                collider.center = new Vector3(0f, 0.3f, 0f);
                                collider.isTrigger = true;
                            }
                            else if (eggData.IsTable)
                            {
                                var cube = obj.transform.Find("Cube");
                                if (cube != null)
                                {
                                    cube.localScale = Vector3.one / 0.02f;
                                    model.transform.localScale = Vector3.one / 0.02f;
                                }
                                else
                                {
                                    Debug.LogWarning("Cube object not found for scaling.");
                                }
                            }
                            else if (eggData.IsBasicArtifact)
                            {
                                GameObject.DestroyImmediate(obj.GetComponent<SkyApplier>());
                                Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                            }
                            else if (eggData.NothingNeeded)
                            {
                                // No modifications needed
                            }
                            else if (eggData.Pen)
                            {
                                Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                                BoxCollider mainModelCollider = model.EnsureComponent<BoxCollider>();
                                mainModelCollider.center = new Vector3(0f, 0f, 0.14f);
                                mainModelCollider.size = new Vector3(0.2f, 0.2f, 0.2f);
                                model.layer = LayerMask.NameToLayer("Default");
                            }
                            else if (eggData.HealItem)
                            {
                               
                               
                                obj.EnsureComponent<Pickupable>();
                                WaitForPrefabRegistration(internalName,eggData.Health,true);
                                //WaitForPrefabModifyEatables(internalName,eggData.Food,eggData.Drink,false);
                                Eatable eatable = obj.EnsureComponent<Eatable>();
                                eatable.foodValue = eggData.Food;
                                eatable.waterValue = eggData.Drink;
                            }                            
                            else if (eggData.Isfood)
                            {
                                Eatable eatable = obj.EnsureComponent<Eatable>();
                                eatable.foodValue = eggData.Food;
                                eatable.waterValue = eggData.Drink;
                                obj.EnsureComponent<Pickupable>();
                            }                           
                            else if (eggData.Isdrink)
                            {
                                Eatable eatable = obj.EnsureComponent<Eatable>();
                                eatable.foodValue = eggData.Food;
                                eatable.waterValue = eggData.Drink;
                                obj.EnsureComponent<Pickupable>();
                            }
                            else if (eggData.IsPosterV)
                            {
                                var renderer = obj.GetComponentInChildren<MeshRenderer>();
                                if (renderer != null)
                                {
                                    Texture2D testPhoto = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                    var material = renderer.materials[1];
                                    material.SetTexture("_MainTex", testPhoto);
                                    material.SetTexture("_SpecTex", testPhoto);
                                }
                                else
                                {
                                    Debug.LogWarning("MeshRenderer component not found for setting textures.");
                                }
                            }
                            else if (eggData.IsPosterH)
                            {
                                var renderer = obj.GetComponentInChildren<MeshRenderer>();
                                if (renderer != null)
                                {
                                    Texture2D testPhoto = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                    var material = renderer.materials[1];
                                    material.SetTexture("_MainTex", testPhoto);
                                    material.SetTexture("_SpecTex", testPhoto);
                                }
                                else
                                {
                                    Debug.LogWarning("MeshRenderer component not found for setting textures.");
                                }
                            }
                            PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                        };
                        prefab.SetGameObject(cloneTemplate);
                        // Check eggData properties to determine the registration path
                        if (eggData.IsGunArtifact == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.IsAnArtifact == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.IsArtifact == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.IsTable == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.IsBasicArtifact == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.Pen == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.NothingNeeded == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.HasStorage == true)
                        {
                            prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText);
                            prefab.WithAutoUnlock();
                        }
                        else if (eggData.Isfood == true)
                        {
                            prefab.SetUnlock(TechType.Peeper);
                            prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.CookedFood);
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText)
                                  .WithFabricatorType(CraftTree.Type.Fabricator)
                                  .WithStepsToFabricatorTab("Noodle1");
                        }
                       
                        else if (eggData.HealItem == true)
                        {
                            prefab.SetUnlock(TechType.Peeper);
                            prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.CookedFood);
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText)
                                  .WithFabricatorType(CraftTree.Type.Fabricator)
                                  .WithStepsToFabricatorTab("Noodle1");
                        }
                        else if (eggData.Isdrink == true)
                        {
                            prefab.SetUnlock(TechType.Peeper);
                            prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.CookedFood);
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText)
                                  .WithFabricatorType(CraftTree.Type.Fabricator)
                                  .WithStepsToFabricatorTab("Noodle2");
                        }
                        else if (eggData.IsPosterH == true)
                        {
                            prefab.WithAutoUnlock();
                            prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText)
                                  .WithFabricatorType(CraftTree.Type.Fabricator)
                                  .WithStepsToFabricatorTab("PoolNoodle");
                        }
                        else if (eggData.IsPosterV == true)
                        {
                            prefab.WithAutoUnlock();
                            prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                            string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                            prefab.SetRecipeFromJson(recipeText)
                                  .WithFabricatorType(CraftTree.Type.Fabricator)
                                  .WithStepsToFabricatorTab("PoolNoodle2");
                        }

                        prefab.Register();
                        TechType techType = prefab.Info.TechType;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error creating CustomItems {eggEntry.Key}: {ex.Message}");
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load CustomItems requirements from JSON.");
        }
    }
    private IEnumerator WaitForPrefabRegistration(string internalName, float healthBack, bool isEdible)
    {
        TechType techType = TechType.None;
        while (techType == TechType.None)
        {
            techType = GetTechType(internalName);
            if (techType != TechType.None)
            {
                SurvivalHandler.GiveHealthOnConsume(techType, healthBack, isEdible);
                
                Debug.Log($"Successfully registered TechType for InternalName: {internalName}");
            }
            else
            {
                Debug.LogWarning($"Waiting for TechType registration for InternalName: {internalName}");
                yield return new WaitForSeconds(1f); // Wait for 1 second before checking again
            }
        }
    }
  
    private TechType GetTechType(string techTypeStr)
    {
        if (Enum.TryParse(techTypeStr, out TechType result))
        {
            return result;
        }
        // Handle error case, maybe return a default TechType
        return TechType.None;
    }

}
