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
using static Nautilus.Assets.PrefabTemplates.FabricatorTemplate;
using System.Diagnostics.Eventing.Reader;

namespace Customitems.main
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    public class QPatch : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "Com.Cookay.CustomItems";
        public const string PLUGIN_NAME = "CustomItems";
        public const string PLUGIN_VERSION = "1.0.0.1";
        private static readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        public bool flag { get; private set; }
        public static Texture2D HorizontalWallLockersTexture = RamuneLib.Utils.ImageUtils.GetTexture("submarine_locker_01");
        public static Texture2D HorizontalWallLockersspec = RamuneLib.Utils.ImageUtils.GetTexture("submarine_locker_01_spec");
        public static Texture2D HorizontalWallLockersnorm = RamuneLib.Utils.ImageUtils.GetTexture("submarine_locker_01_normal");
        public void Awake()
        {
            harmony.PatchAll();
            Maketabs();
            LoadEggRequirements();
           
        }
        public void start()
        {

        }
        private void Maketabs()
        {
            Console.Write("Loading Tabs");
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "CIS", "CustomItems", RamuneLib.Utils.ImageUtils.GetSprite("CIS"), new string[]
            {


            });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle", "Poster", SpriteManager.Get(TechType.PosterKitty), new string[]
            {

                "CIS"
            });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle2", "Poster", SpriteManager.Get(TechType.PosterAurora), new string[]
            {

                "CIS"
            });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle1", "Food", SpriteManager.Get(TechType.NutrientBlock), new string[]
            {

                "CIS"
               
            });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle2", "Drink", SpriteManager.Get(TechType.FilteredWater), new string[]
            {

                "CIS"
                
            });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "OX", "Oxygen Tanks", SpriteManager.Get(TechType.Tank), new string[]
            {

                "CIS"
                

            });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "FA", "FirstAid", SpriteManager.Get(TechType.FirstAidKit), new string[]
            {

                "CIS"
                

            });
        }
        private void LoadEggRequirements()
        {
            RequirementsLoader loader = new RequirementsLoader();
            Dictionary<string, EggInfoData> allEggInfo = loader.LoadAllEggInfo();

            if (allEggInfo.Count > 0)
            {
                foreach (var eggEntry in allEggInfo)    
                {
                    try
                    {
                        string eggName = eggEntry.Key;
                        EggInfoData eggData = eggEntry.Value;

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
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                                     {
                                   "CIS",
                                   "FA"
                                     });
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
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                                 {
                                 "CIS",
                                 "OX"
                                 });
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
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                                 {
                                   "CIS",
                                   "FA"
                                 });
                                prefab.SetGameObject(medkit);
                                prefab.Register();
                                SurvivalHandler.GiveHealthOnConsume(prefab.Info.TechType, eggData.Health, true);

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
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                            {
                                   "CIS",
                                   "Noodle1"
                            });
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
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                               {
                                   "CIS",
                                   "Noodle2"
                               });
                                prefab.SetGameObject(food);
                                prefab.Register();


                            }
                            else if (eggData.Placeable == true)
                            {
                                var poster = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                poster.ModifyPrefab += obj =>
                                {
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.Submarine | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Outside | ConstructableFlags.Base | ConstructableFlags.Wall | ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError($"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }
                                    if (eggData.IsPosterV == true)
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
                                    else if (eggData.IsPosterH == true)
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

                                        PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                    };

                                   
                                    
                                };
                                if (eggData.IsPosterH == true)
                                {
                                    prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                            {
                                   "CIS",
                                   "PoolNoodle"
                            });
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
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                            {
                                   "CIS",
                                   "PoolNoodle2"
                            });
                                    prefab.WithAutoUnlock();
                                    prefab.SetGameObject(poster);
                                    prefab.Register();
                                }
                            }
                            else if (eggData.CustomHullPlates == true)
                            {
                                var poster = new CloneTemplate(prefab.Info, TechType.GilathissHullPlate);
                                poster.ModifyPrefab += obj =>
                                {
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.Submarine | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Outside | ConstructableFlags.Base | ConstructableFlags.Wall | ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError($"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }
                                    if (eggData.CustomHullPlates == true)
                                    {
                                        var meshRenderer = obj.FindChild("Icon").GetComponent<MeshRenderer>();
                                        if (meshRenderer != null)
                                        {
                                            Texture2D testPhoto = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                            meshRenderer.material.mainTexture = testPhoto;
                                        }
                                        else
                                        {
                                            Debug.LogWarning("MeshRenderer component not found for setting textures.");
                                        }
                                    }
                                   

                                        PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                };

                               
                                    prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
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
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.Submarine | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Outside | ConstructableFlags.Base | ConstructableFlags.Wall | ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError($"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }
                                    else if (eggData.Hlocker == true)
                                    {

                                        var renderer = obj.FindChild("model").FindChild("submarine_locker_02").FindChild("submarine_locker_02_door").GetComponent<MeshRenderer>();
                                        foreach (var m in renderer.materials)
                                        {
                                            m.mainTexture = QPatch.HorizontalWallLockersTexture;
                                            m.SetTexture("_SpecTex", QPatch.HorizontalWallLockersTexture);
                                            m.SetTexture("_Illum", QPatch.HorizontalWallLockersTexture);
                                            m.SetTexture("_BumpMap", QPatch.HorizontalWallLockersnorm);

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
                                    PrefabUtils.AddStorageContainer(obj, "StorageRoot", eggData.InternalName, eggData.Width, eggData.Height, true);

                                };

                                prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
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
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.Submarine | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Outside | ConstructableFlags.Base | ConstructableFlags.Wall | ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    // Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError($"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
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
                                    else if (eggData.IsBasicArtifact == true)
                                    {
                                        GameObject.DestroyImmediate(obj.GetComponent<SkyApplier>());
                                        Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                                    }
                                    else if (eggData.NothingNeeded == true)
                                    {
                                        // No modifications needed
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
                                        if (eggData.OneExtraB == true)
                                        {
                                            obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                        }
                                       else if (eggData.TwoExtraB == true)
                                        {
                                            obj.transform.Find(eggData.ExtraObjectName).parent = model.transform;
                                            obj.transform.Find(eggData.ExtraObjectName2).parent = model.transform;
                                        }
                                        


                                    }
                                    // Log the GameObject found by the name
                                    Debug.Log($"Found GameObject with name '{eggData.ObjectName}' for Deco '{eggName}': {model.name}");

                                    if(eggData.AddStorage)
                                    {
                                      PrefabUtils.AddStorageContainer(obj, "StorageRoot", eggData.InternalName, eggData.Width, eggData.Height, true);
                                    }

                                    PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                };

                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                                prefab.WithAutoUnlock();
                                prefab.SetGameObject(builditem);
                                prefab.Register();

                            };









                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception processing egg {eggEntry.Key}: {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load egg info data.");
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
}
