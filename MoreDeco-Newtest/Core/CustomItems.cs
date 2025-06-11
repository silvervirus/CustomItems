using System;
using System.Collections.Generic;
using CustomItems.Util;
using HarmonyLib;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Extensions;
using Nautilus.Handlers;
using Nautilus.Utility;
using RamuneLib.Extensions;
using UnityEngine;
using static CustomItems.Util.Pings;
using static CustomItems.Util.Cookay_sUtil;
using static CustomItems.Core.DecoFabricator;
using Object = UnityEngine.Object;

namespace CustomItems.Core;

public class CustomItems
{
    public static Texture2D HorizontalWallLockersTexture =
        RamuneLib.Utils.ImageUtils.GetTexture("submarine_locker_01");

    public static Texture2D HorizontalWallLockersspec =
        RamuneLib.Utils.ImageUtils.GetTexture("submarine_locker_01_spec");

    public static Texture2D HorizontalWallLockersnorm =
        RamuneLib.Utils.ImageUtils.GetTexture("submarine_locker_01_normal");

    public static void LoadCustomItems()
    {
        
        
            RequirementsLoader loader = new RequirementsLoader();
            Dictionary<string, EggData.EggInfoData> allEggInfo = loader.LoadAllEggInfo();

            if (allEggInfo.Count > 0)
            {
                foreach (var eggEntry in allEggInfo)
                {
                    try
                    {
                        string eggName = eggEntry.Key;
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
                        string eggspriteName = eggData.Spritename;
                        string eggObjectName = eggData.ObjectName;

                        // Convert eggData.TechType to TechType to get the sprite




                        // Split internal names and create/register the custom egg prefab for each TechType
                        foreach (string internalName in eggInterName.Split(','))
                        {


                            // Fetch the sprite
                            Atlas.Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(eggspriteName);

                            // Create and register the custom egg prefab
                            UtraBasePrefab.UnifiedPrefabsCombined prefab = new UtraBasePrefab.UnifiedPrefabsCombined(internalName.Trim(), eggDisplayName,
                                eggDescription, eggObjectName, eggSprite);

                            // Create CloneTemplate using ResourceId
                            CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);

                            if (eggData.HealItem == true)
                            {
                                var setitem = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                setitem.ModifyPrefab += obj =>
                                {
                                    Eatable eatable = obj.EnsureComponent<Eatable>();
                                    eatable.foodValue = eggData.Food;
                                    eatable.waterValue = eggData.Drink;
                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.Peeper);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetRecipeFromJson(recipeText);
                                if (eggData.UseDefaultTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "FA"
                                                
                                        });
                                }
                                else if (eggData.UseCustomTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        ParsePath(eggData.Path));
                                }

                                prefab.SetGameObject(setitem);
                                prefab.Register();
                                SurvivalHandler.GiveHealthOnConsume(prefab.Info.TechType, eggData.Health, true);

                            }

                            if (eggData.Tankitem == true)
                            {
                                var tankitem = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                tankitem.ModifyPrefab += obj =>
                                {
                                    Eatable eatable = obj.EnsureComponent<Eatable>();

                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.Peeper);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetRecipeFromJson(recipeText);
                                if (eggData.UseDefaultTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "OX"
                                                
                                        });
                                }
                                else if (eggData.UseCustomTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        ParsePath(eggData.Path));
                                }

                                prefab.SetGameObject(tankitem);
                                prefab.Register();
                                SurvivalHandler.GiveOxygenOnConsume(prefab.Info.TechType, eggData.Oxygen, true);

                            }
                            else if (eggData.MedKititem == true)
                            {
                                var medkit = new CloneTemplate(prefab.Info, TechType.FirstAidKit);
                                medkit.ModifyPrefab += obj =>
                                {
                                    Eatable eatable = obj.EnsureComponent<Eatable>();

                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.Peeper);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetRecipeFromJson(recipeText);
                                if (eggData.UseDefaultTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "FA"
                                                
                                        });
                                }
                                else if (eggData.UseCustomTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        ParsePath(eggData.Path));
                                }

                                prefab.SetGameObject(medkit);
                                prefab.Register();
                                SurvivalHandler.GiveHealthOnConsume(prefab.Info.TechType, eggData.Health, true);

                            }
                            else if (eggData.OyxgenTankitem == true)
                            {
                                var tankitem = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                tankitem.ModifyPrefab += obj =>
                                {
                                    obj.EnsureComponent<Pickupable>();
                                    obj.GetAllComponentsInChildren<Oxygen>()
                                        .Do(o => o.oxygenCapacity = eggData.TankOxygen);



                                };
                                prefab.SetEquipment(EquipmentType.Tank);
                                prefab.SetUnlock(TechType.Peeper);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetRecipeFromJson(recipeText);
                                if (eggData.UseDefaultTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "EQP"
                                                
                                        });
                                }
                                else if (eggData.UseCustomTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        ParsePath(eggData.Path));
                                }

                                prefab.SetGameObject(tankitem);
                                prefab.Register();


                            }
                            else if (eggData.IsKnife)
                            {
                                var tankitem = new CloneTemplate(prefab.Info, TechType.Knife);
                                tankitem.ModifyPrefab += obj =>
                                {
                                    var renderer = obj.transform.Find("knife_01")?.GetComponent<MeshRenderer>();
                                    if (renderer != null)
                                    {
                                        renderer.material.mainTexture = RamuneLib.Utils.ImageUtils.GetTexture(internalName);
                                        renderer.material.SetTexture("_SpecTex", RamuneLib.Utils.ImageUtils.GetTexture(internalName));
                                        renderer.material.SetTexture("_Illum", RamuneLib.Utils.ImageUtils.GetTexture(internalName + "_illum"));
                                       // renderer.material.SetTexture("_BumpMap", RamuneLib.Utils.ImageUtils.GetTexture(internalName + "_normal"));
                                    }

                                    var oldBlade = obj.GetComponent<Knife>();
                                    var newKnife = obj.AddComponent<CustomKnife>().CopyComponent(oldBlade);
                                    Object.DestroyImmediate(oldBlade);

                                    newKnife.Configure(eggData.Hitforce, 1f + eggData.Damage);
                                    newKnife.attackDist = 1f + eggData.Range;
                                    
                                };
                                prefab.SetUnlock(TechType.Peeper);
                                prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
                                if (eggData.UseDefaultTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "OX"
                                                
                                        });
                                }
                                else if (eggData.UseCustomTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                         ParsePath(eggData.Path));
                                }

                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetGameObject(tankitem);
                                prefab.Register();
                            }
                            else if (eggData.IsHotKnife)
                            {
                                var tankitem = new CloneTemplate(prefab.Info, TechType.HeatBlade);
                                tankitem.ModifyPrefab += obj =>
                                {
                                    var renderer = obj.transform.Find("knife_01_hot")?.GetComponent<Renderer>();
                                    if (renderer != null)
                                    {
                                        renderer.material.mainTexture = RamuneLib.Utils.ImageUtils.GetTexture(internalName);
                                        renderer.material.SetTexture("_SpecTex", RamuneLib.Utils.ImageUtils.GetTexture(internalName));
                                        renderer.material.SetTexture("_Illum", RamuneLib.Utils.ImageUtils.GetTexture(internalName + "_illum"));
                                        //renderer.material.SetTexture("_BumpMap", RamuneLib.Utils.ImageUtils.GetTexture(internalName + "_normal"));
                                    }

                                    var oldBlade = obj.GetComponent<HeatBlade>();
                                    var newKnife = obj.AddComponent<CustomHotKnife>().CopyComponent(oldBlade);
                                    Object.DestroyImmediate(oldBlade);

                                    newKnife.Configure(eggData.Hitforce, 1f + eggData.Damage);
                                    newKnife.attackDist = 1f + eggData.Range;




                                };
                                prefab.SetUnlock(TechType.Peeper);
                                prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
                                if (eggData.UseDefaultTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "OX"
                                                
                                        });
                                }
                                else if (eggData.UseCustomTab)
                                {
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                        prefab.Info.TechType,
                                         ParsePath(eggData.Path));
                                }

                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetGameObject(tankitem);
                                prefab.Register();
                            }
                            else if (eggData.Isfood == true)
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
                            else if (eggData.Isdrink == true)
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
                            else if (eggData.Placeable == true)
                            {
                                var poster = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                poster.ModifyPrefab += obj =>
                                {
                                    

                                    
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError(
                                            $"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }


                                    if (eggData.IsPosterV == true)
                                    {
                                        var renderer = obj.GetComponentInChildren<MeshRenderer>();
                                        if (renderer != null)
                                        {
                                            Texture2D testPhoto =
                                                RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                            var material = renderer.materials[1];
                                            material.SetTexture("_MainTex", testPhoto);
                                            material.SetTexture("_SpecTex", testPhoto);



                                        }

                                        else
                                        {
                                            Debug.LogWarning("MeshRenderer component not found for setting textures.");
                                        }
                                    }
                                    else if (eggData.IsPosterH == true)
                                    {
                                        var renderer = obj.GetComponentInChildren<MeshRenderer>();
                                        if (renderer != null)
                                        {
                                            Texture2D testPhoto =
                                                RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                            var material = renderer.materials[1];
                                            material.SetTexture("_MainTex", testPhoto);
                                            material.SetTexture("_SpecTex", testPhoto);


                                        }
                                        

                                       

                                    }
                                    else if (eggData.ItemPlaceable)
                                    {
                                        obj.EnsureComponent<Rigidbody>().isKinematic = true;
                                        obj.EnsureComponent<Pickupable>();
                                        obj.EnsureComponent<TechTag>().type = prefab.Info.TechType;
                                        obj.EnsureComponent<LargeWorldEntity>().cellLevel =
                                            LargeWorldEntity.CellLevel.Near;

                                      
                                        var rb = obj.GetComponent<Rigidbody>();
                                        rb.useGravity = false;
                                        rb.constraints   = RigidbodyConstraints.FreezeAll;   

                                        // Set up the PlaceTool behavior
                                        var placeTool = obj.EnsureComponent<PlaceTool>();
                                        placeTool.allowedOnGround = true;
                                        placeTool.allowedOutside = true;
                                        placeTool.allowedInBase = true;
                                        placeTool.alignWithSurface = false;
                                        placeTool.allowedOnWalls = true;
                                        placeTool.allowedOnCeiling = true;
                                        placeTool.allowedOnConstructable = true;
                                        placeTool.rotationEnabled = true;
                                        placeTool.allowedOnRigidBody = true;
                                        placeTool.alignWithSurface = false;
                                        placeTool.ghostModelPrefab = obj;
                                        placeTool.hideInvalidGhostModel = false;
                                      
                                        placeTool.hasAnimations = false;
                                        placeTool.hasBashAnimation = false;
                                        placeTool.drawTime = 0f;
                                        obj.EnsureComponent<OpenStorageOnClick>();
                                      
                                        GameObject viewModel = new GameObject("FPViewModel");
                                        viewModel.transform.SetParent(obj.transform, false);

                                        GameObject propModel = new GameObject("FPPropModel");
                                        propModel.transform.SetParent(obj.transform, false);


                                        var model2 = obj.transform.Find("model"); 
                                        if (model2 != null)
                                        {
                                            model2.SetParent(viewModel.transform, false);

                                            // 3. Duplicate for propModel (what shows when dropped)
                                            GameObject modelClone = GameObject.Instantiate(model.gameObject, propModel.transform);
                                        }


                                        var fpModel = obj.EnsureComponent<FPModel>();
                                        fpModel.viewModel = viewModel;
                                        fpModel.propModel = propModel;


                                        
                                        if (eggData.AddStorage)
                                        {
                                            PrefabUtils.AddStorageContainer(obj, "StorageRoot", eggData.InternalName,
                                                eggData.Width, eggData.Height, true);
                                            
                                        }
                                        else if (eggData.IsTable == true)
                                        {

                                            var cude = obj.transform.Find("Cube");
                                            cude.localScale = Vector3.one / 0002;
                                            model.transform.localScale = Vector3.one / 0002;
                                        }
                                        else if (eggData.Pen == true)
                                        {
                                            Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                                            BoxCollider mainModelCollider = model.EnsureComponent<BoxCollider>();
                                            mainModelCollider.center = new Vector3(0f, 0f, 0.14f);
                                            mainModelCollider.size = new Vector3(0.2f, 0.2f, 0.2f);
                                            model.layer = LayerMask.NameToLayer("Default");
                                        }
                                        if (eggData.IsAnArtifact == true)
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
                                        else if (eggData.IsGunArtifact == true)
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
                                        else if (eggData.IsArtifact == true)
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
                                        
                                    }
                                    else if (eggData.IsmultObject == true)
                                    {
                                        if (eggData.OneExtra == true)
                                        {
                                            obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                        }
                                        else if (eggData.TwoExtra == true)
                                        {
                                            obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                            obj.transform.Find(eggData.ExtraObjectName2).parent = model.transform;
                                        }
                                        
                                        
                                    }
                                    else
                                    {
                                        Debug.LogWarning("MeshRenderer component not found for setting textures.");
                                    }


                                };
                                if (eggData.IsPosterH == true)
                                {
                                    prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                    prefab.SetRecipeFromJson(recipeText);
                                    if (eggData.UseDefaultTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            new string[]
                                            {
                                                "CIS",
                                                "PoolNoodle"
                                                
                                            });
                                    }
                                    else if (eggData.UseCustomTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            ParsePath(eggData.Path));
                                    }
                                    prefab.WithAutoUnlock();
                                    prefab.SetGameObject(poster);
                                    prefab.Register();
                                }

                                else if (eggData.IsPosterV == true)
                                {
                                    prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                    prefab.SetRecipeFromJson(recipeText);
                                    if (eggData.UseDefaultTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            new string[]
                                            {
                                                "CIS",
                                                "PoolNoodle2"
                                                
                                            });
                                    }
                                    else if (eggData.UseCustomTab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator,
                                            prefab.Info.TechType,
                                            ParsePath(eggData.Path));
                                    }
                                    prefab.WithAutoUnlock();
                                    prefab.SetGameObject(poster);
                                    prefab.Register();
                                }
                                else if (eggData.ItemPlaceable)
                                {   
                                    prefab.SetGameObject(poster);
                                    prefab.SetUnlock(TechType.Peeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                    prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    if (eggData.DecoLocker)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "DeoLocker",
                                               

                                            });
                                    }
                                    else if (eggData.Decoration)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "Deo",
                                                

                                            });
                                    }
                                    else if (eggData.DecoEgg)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "Deoegg",
                                                

                                            });
                                    }
                                    else if (eggData.DecoOG)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "DeoOg",
                                                

                                            });
                                    }
                                    else if (eggData.DecoFood)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "DeoFood",
                                                

                                            });
                                    }
                                    else if (eggData.DecoBag)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "DeoBags",
                                                

                                            });
                                    }
                                    else if (eggData.DecoLab)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "Lab",
                                                

                                            });
                                    }
                                    else if (eggData.DecoAlien)
                                    {
                                        CraftTreeHandler.AddCraftingNode(DecoFabricator.DecoFab,
                                            prefab.Info.TechType,
                                            new string[]
                                            {

                                                "Deoalien",
                                                

                                            });
                                    }
                                    prefab.Register();
                                }
                            }
                            else if (eggData.CustomHullPlates == true)
                            {
                                var poster = new CloneTemplate(prefab.Info, TechType.GilathissHullPlate);
                                poster.ModifyPrefab += obj =>
                                {
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside |
                                                                            ConstructableFlags.Rotatable |
                                                                            ConstructableFlags.Ground |
                                                                            ConstructableFlags.Submarine |
                                                                            ConstructableFlags.Rotatable |
                                                                            ConstructableFlags.AllowedOnConstructable |
                                                                            ConstructableFlags.Outside |
                                                                            ConstructableFlags.Base |
                                                                            ConstructableFlags.Wall |
                                                                            ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError(
                                            $"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }

                                    if (eggData.CustomHullPlates == true)
                                    {
                                        var meshRenderer = obj.FindChild("Icon").GetComponent<MeshRenderer>();
                                        if (meshRenderer != null)
                                        {
                                            Texture2D testPhoto =
                                                RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                            meshRenderer.material.mainTexture = testPhoto;
                                        }
                                        else
                                        {
                                            Debug.LogWarning("MeshRenderer component not found for setting textures.");
                                        }
                                    }


                                    PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                };


                                prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                                    .SetBuildable();
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.WithAutoUnlock();
                                prefab.SetGameObject(poster);
                                prefab.Register();


                            }
                            else if (eggData.HasStorage == true)
                            {
                                var poster = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                poster.ModifyPrefab += obj =>
                                {
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside |
                                                                            ConstructableFlags.Rotatable |
                                                                            ConstructableFlags.Ground |
                                                                            ConstructableFlags.Submarine |
                                                                            ConstructableFlags.Rotatable |
                                                                            ConstructableFlags.AllowedOnConstructable |
                                                                            ConstructableFlags.Outside |
                                                                            ConstructableFlags.Base |
                                                                            ConstructableFlags.Wall |
                                                                            ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError(
                                            $"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }
                                    else if (eggData.Hlocker == true)
                                    {

                                        var renderer = obj.FindChild("model").FindChild("submarine_locker_02")
                                            .FindChild("submarine_locker_02_door").GetComponent<MeshRenderer>();
                                        foreach (var m in renderer.materials)
                                        {
                                            m.mainTexture =
                                                HorizontalWallLockersTexture;
                                            m.SetTexture("_SpecTex", HorizontalWallLockersTexture);
                                            m.SetTexture("_Illum", HorizontalWallLockersTexture);
                                            m.SetTexture("_BumpMap", HorizontalWallLockersnorm);

                                        }

                                        obj.FindChild("model").transform.rotation = Quaternion.Euler(0, 0, 90);
                                        obj.FindChild("Collider").transform.rotation = Quaternion.Euler(0, 0, 90);
                                        obj.FindChild("Build Trigger").transform.rotation = Quaternion.Euler(0, 0, 90);
                                        obj.FindChild("TriggerCull").transform.rotation = Quaternion.Euler(0, 0, 90);
                                    }
                                    else if (eggData.IsmultObject == true)
                                    {
                                        if (eggData.OneExtra == true)
                                        {
                                            obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                        }
                                        else if (eggData.TwoExtra == true)
                                        {
                                            obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                            obj.transform.Find(eggData.ExtraObjectName2).parent = model.transform;
                                        }


                                    }






                                    PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                    PrefabUtils.AddStorageContainer(obj, "StorageRoot", eggData.InternalName,
                                        eggData.Width, eggData.Height, true);

                                };

                                prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                                    .SetBuildable();
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.WithAutoUnlock();
                                prefab.SetGameObject(poster);
                                prefab.Register();



                            }
                            else if (eggData.Buildable == true)
                            {
                                var builditem = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                builditem.ModifyPrefab += obj =>
                                {
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside |
                                                                            ConstructableFlags.Rotatable |
                                                                            ConstructableFlags.Ground |
                                                                            ConstructableFlags.Submarine |
                                                                            ConstructableFlags.Rotatable |
                                                                            ConstructableFlags.AllowedOnConstructable |
                                                                            ConstructableFlags.Outside |
                                                                            ConstructableFlags.Base |
                                                                            ConstructableFlags.Wall |
                                                                            ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError(
                                            $"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }

                                    if (eggData.IsAnArtifact == true)
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
                                    else if (eggData.IsGunArtifact == true)
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
                                    else if (eggData.IsArtifact == true)
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
                                    else if (eggData.IsTable == true)
                                    {

                                        var cude = obj.transform.Find("Cube");
                                        cude.localScale = Vector3.one / 0002;
                                        model.transform.localScale = Vector3.one / 0002;
                                    }
                                    else if (eggData.IsBasicArtifact == true)
                                    {
                                        GameObject.DestroyImmediate(obj.GetComponent<SkyApplier>());
                                        Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                                    }
                                    else if (eggData.NothingNeeded == true)
                                    {
                                        // No modifications needed
                                        if (eggData.Ping)
                                        {
                                            ping = obj.AddComponent<PingInstance>();
                                            ping._label = eggDisplayName;
                                            ping.colorIndex = 0;
                                            ping.range = 10;
                                            ping.origin = model.transform;
                                        }
    
                                        PingInstance pingInstance = obj.GetComponent<PingInstance>();
                                        if (pingInstance != null)
                                        {
                                            pingInstance._label = eggData.FriendlyName;
        
                                            if (eggData.Base)         pingInstance.pingType = BasePing;
                                            else if (eggData.BaseL)   pingInstance.pingType = BaseLargePing;
                                            else if (eggData.Beacon)  pingInstance.pingType = BeaconPing;
                                            else if (eggData.Signel)  pingInstance.pingType = Signalping;
                                            else if (eggData.Custom)  pingInstance.pingType = testping;
                                            else if (eggData.Egg)     pingInstance.pingType = EggPing;
                                            else if (eggData.Creature) pingInstance.pingType = CreaturePing;
                                            else if (eggData.Flag1)   pingInstance.pingType = Flag1Ping;
                                            else if (eggData.Flag2)   pingInstance.pingType = Flag2Ping;
                                            else if (eggData.Flag3)   pingInstance.pingType = Flag3Ping;
                                            else if (eggData.Flag4)   pingInstance.pingType = Flag4Ping;
                                            else if (eggData.Flag5)   pingInstance.pingType = Flag5Ping;
                                            else if (eggData.Flag6)   pingInstance.pingType = Flag6Ping;
                                            else if (eggData.Flag7)   pingInstance.pingType = Flag7Ping;
                                            else if (eggData.Alien1)  pingInstance.pingType = Alien1Ping;
                                            else if (eggData.Alien2)  pingInstance.pingType = Alien2Ping;
                                            else if (eggData.Alien3)  pingInstance.pingType = Alien3Ping;
                                            else if (eggData.Alien4)  pingInstance.pingType = Alien4Ping;
                                            else if (eggData.Alien5)  pingInstance.pingType = Alien5Ping;
                                            else if (eggData.Alien6)  pingInstance.pingType = Alien6Ping;
                                            else if (eggData.Drill)   pingInstance.pingType = DrillPing;
                                            else if (eggData.Ship)    pingInstance.pingType = Ship;
                                            else if (eggData.Check)   pingInstance.pingType = Check;
                                            else if (eggData.Check2)  pingInstance.pingType = Check2;
                                            else if (eggData.CreepVine) pingInstance.pingType = Pings.Creepvine;
                                            else if (eggData.Cross)   pingInstance.pingType = Cross;
                                            else if (eggData.Chest)   pingInstance.pingType = Chest;
                                            else if (eggData.Arrow)   pingInstance.pingType = Arrow;
                                            else if (eggData.Fire)    pingInstance.pingType = Pings.Fire;
                                            else if (eggData.Acube)   pingInstance.pingType = Acube;
                                            else if (eggData.Triangle) pingInstance.pingType = TriA;
                                            else if (eggData.LifeSupport) pingInstance.pingType = LifeSupport;
                                            else if (eggData.Power)   pingInstance.pingType = Power;
                                            else if (eggData.Epoint)
                                            {
                                                pingInstance.pingType = Epoint;
                                                pingInstance.SetLabel(eggDisplayName);
                                            }

                                            pingInstance.SetLabel(eggDisplayName);
                                        }
                                    }

                                    else if (eggData.Pen == true)
                                    {
                                        Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                                        BoxCollider mainModelCollider = model.EnsureComponent<BoxCollider>();
                                        mainModelCollider.center = new Vector3(0f, 0f, 0.14f);
                                        mainModelCollider.size = new Vector3(0.2f, 0.2f, 0.2f);
                                        model.layer = LayerMask.NameToLayer("Default");
                                    }
                                    else if (eggData.IsBmultObject == true)
                                    {
                                        switch (eggData.OneExtraB)
                                        {
                                            case true:
                                                obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                                break;
                                            default:
                                            {
                                                if (eggData.TwoExtraB == true)
                                                {
                                                    obj.transform.Find(eggData.ExtraObjectName).parent =
                                                        model.transform;
                                                    obj.transform.Find(eggData.ExtraObjectName2).parent =
                                                        model.transform;
                                                }

                                                break;
                                            }
                                        }
                                    }

                                    // Log the GameObject found by the name
                                    Debug.Log(
                                        $"Found GameObject with name '{eggData.ObjectName}' for Deco '{eggName}': {model.name}");

                                    if (eggData.AddStorage)
                                    {
                                        PrefabUtils.AddStorageContainer(obj, "StorageRoot", eggData.InternalName,
                                            eggData.Width, eggData.Height, true);
                                    }

                                    PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                };

                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                                    .SetBuildable();
                                prefab.WithAutoUnlock();
                                prefab.SetGameObject(builditem);
                                prefab.Register();

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception processing CustomItem {eggEntry.Key}: {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load CustomItem info data.");
            }
            
    }

}
