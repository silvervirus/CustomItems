using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CustomItems.BatteryPrefab;
using CustomItems.BatteriesLoader;
using CustomItems.Loader;
using Customitems.info;
using Debug = UnityEngine.Debug;
using Nautilus.Utility;
using static HandReticle;
using PrefabUtils = Nautilus.Utility.PrefabUtils;
using static Nautilus.Assets.PrefabTemplates.FabricatorTemplate;
using System.Diagnostics.Eventing.Reader;
using BepInEx;
using HarmonyLib;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using rail;
using RamuneLib.Extensions;
using static RootMotion.FinalIK.RagdollUtility;
using CustomBatteries.API;
using CustomItems;

namespace Customitems.main
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.mrpurple6411.CustomBatteries", BepInDependency.DependencyFlags.SoftDependency)]

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
        internal static PingType BasePing { get; private set; }
        internal static PingType Flag1Ping { get; private set; }
        internal static PingType Flag2Ping { get; private set; }
        internal static PingType Flag3Ping { get; private set; }
        internal static PingType Flag4Ping { get; private set; }
        internal static PingType Flag5Ping { get; private set; }
        internal static PingType Flag6Ping { get; private set; }
        internal static PingType Flag7Ping { get; private set; }
        internal static PingType Alien1Ping { get; private set; }
        internal static PingType Alien2Ping { get; private set; }
        internal static PingType Alien3Ping { get; private set; }
        internal static PingType Alien4Ping { get; private set; }
        internal static PingType Alien5Ping { get; private set; }
        internal static PingType Alien6Ping { get; private set; }
        internal static PingType Alien7Ping { get; private set; }
        internal static PingType EggPing { get; private set; }
        internal static PingType DrillPing { get; private set; }
        internal static PingType CreaturePing { get; private set; }
        internal static PingType BaseLargePing { get; private set; }
        internal static PingType BeaconPing { get; private set; }
        internal static PingType Signalping { get; private set; }
        internal static PingType testping { get; private set; }

        internal static PingType Ship { get; private set; }
        internal static PingType Check { get; private set; }
        internal static PingType Check2 { get; private set; }
        internal static PingType Creepvine { get; private set; }
        internal static PingType Death { get; private set; }
        internal static PingType Fire { get; private set; }
        internal static PingType TriA { get; private set; }
        internal static PingType Epoint { get; private set; }

        internal static PingType Cross { get; private set; }

        internal static PingType Chest { get; private set; }

        internal static PingType Acube { get; private set; }

        internal static PingType LifeSupport { get; private set; }

        internal static PingType Arrow { get; private set; }

        internal static PingType Power { get; private set; }

        internal static PingType Important { get; private set; }


        public static readonly List<TechType> typesToMakePickupables = new();
        public static readonly List<TechType> customBatterynames = new();
        internal static PingInstance ping { get; private set; }
        public void Awake()
        {

            BaseLargePing = EnumHandler.AddEntry<PingType>("Customping1")
          .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("BLR"));

            BasePing = EnumHandler.AddEntry<PingType>("Customping5")
         .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("MPR"));
            Flag1Ping = EnumHandler.AddEntry<PingType>("Customping6")
         .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag1"));
            Flag2Ping = EnumHandler.AddEntry<PingType>("Customping7")
         .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag2"));
            Flag3Ping = EnumHandler.AddEntry<PingType>("Customping8")
         .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag3"));
            Flag4Ping = EnumHandler.AddEntry<PingType>("Customping9")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag4"));
            Flag5Ping = EnumHandler.AddEntry<PingType>("Customping10")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag5"));
            Flag6Ping = EnumHandler.AddEntry<PingType>("Customping11")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag6"));
            Flag7Ping = EnumHandler.AddEntry<PingType>("Customping12")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag7"));
            Alien1Ping = EnumHandler.AddEntry<PingType>("Customping16")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("R"));
            Alien2Ping = EnumHandler.AddEntry<PingType>("Customping17")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("P"));
            Alien3Ping = EnumHandler.AddEntry<PingType>("Customping18")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("A"));
            Alien4Ping = EnumHandler.AddEntry<PingType>("Customping19")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("T"));
            Alien5Ping = EnumHandler.AddEntry<PingType>("Customping20")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("U"));
            Alien6Ping = EnumHandler.AddEntry<PingType>("Customping21")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("EAS"));
            EggPing = EnumHandler.AddEntry<PingType>("Customping13")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("EGG"));
            DrillPing = EnumHandler.AddEntry<PingType>("Customping15")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("DR"));
            CreaturePing = EnumHandler.AddEntry<PingType>("Customping14")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("DR"));
            BeaconPing = EnumHandler.AddEntry<PingType>("Customping22")
        .WithIcon(SpriteManager.Get(SpriteManager.Group.Pings, PingType.Beacon.ToString()));
            Signalping = EnumHandler.AddEntry<PingType>("Customping3")
        .WithIcon(SpriteManager.Get(SpriteManager.Group.Pings, PingType.Signal.ToString()));
            testping = EnumHandler.AddEntry<PingType>("Customping4")
        .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("BVping"));
            Ship = EnumHandler.AddEntry<PingType>("Ship")
      .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("AS"));
            Check = EnumHandler.AddEntry<PingType>("Check")
     .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("MMRO"));
            Check2 = EnumHandler.AddEntry<PingType>("Check2")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("CHK"));
            Creepvine = EnumHandler.AddEntry<PingType>("CreepLocation")
   .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("CR"));
            Death = EnumHandler.AddEntry<PingType>("Dangerous")
 .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("MMD"));
            Fire = EnumHandler.AddEntry<PingType>("Fire Location")
 .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("WF"));
            TriA = EnumHandler.AddEntry<PingType>("Marked Location")
 .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("SWO"));
            Epoint = EnumHandler.AddEntry<PingType>("Caution Location")
 .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("PWW"));
            Cross = EnumHandler.AddEntry<PingType>("Heal Item Location")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("SP"));
            Chest = EnumHandler.AddEntry<PingType>("Chest Location")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("ExtraChests"));
            Acube = EnumHandler.AddEntry<PingType>("Alien Location")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Ion"));
            LifeSupport = EnumHandler.AddEntry<PingType>("breathable Location")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("IconLS"));
            Arrow = EnumHandler.AddEntry<PingType>("Thisway")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("IconArrow"));
            Power = EnumHandler.AddEntry<PingType>("Power")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("IP"));
            Important = EnumHandler.AddEntry<PingType>("Important Location")
.WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Extrapoint"));
            harmony.PatchAll();
            Maketabs();
            LoadPrePrePreIngredientsRequirements();
            LoadPrePreIngredientsRequirements();
            LoadPreIngredientsRequirements();
            LoadIngredientsRequirements();
            LoadEggRequirements();
            LoadBatteryRequirements();
           
        }
        public void Start()
        {

        }
        private void Maketabs()
        {
            Console.Write("Loading Tabs");
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "CIS", "CustomItems",
                RamuneLib.Utils.ImageUtils.GetSprite("CIS"), new string[]
                {


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle", "Poster",
                SpriteManager.Get(TechType.PosterKitty), new string[]
                {

                    "CIS"
                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle2", "Poster",
                SpriteManager.Get(TechType.PosterAurora), new string[]
                {

                    "CIS"
                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle3", "FramedPhoto",
                SpriteManager.Get(TechType.PictureSamPotato), new string[]
                {

                    "CIS"
                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle1", "Food",
                SpriteManager.Get(TechType.NutrientBlock), new string[]
                {

                    "CIS"

                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle2", "Drink",
                SpriteManager.Get(TechType.FilteredWater), new string[]
                {

                    "CIS"

                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "OX", "Oxygen Tanks",
                SpriteManager.Get(TechType.Tank), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "FA", "FirstAid",
                SpriteManager.Get(TechType.FirstAidKit), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "EQP", "Equipment",
                SpriteManager.Get(TechType.DoubleTank), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "CF3", "CustomFoods3",
                RamuneLib.Utils.ImageUtils.GetSprite("CF3mod"), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "FIDT", "Food Ingredients",
                RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "MIDT", "More Ingredients",
                RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "IDT", "Ingredients",
                RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"), new string[]
                {

                    "CIS",
                    "CF3"


                });
           
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "ETACO", "Mexican",
                RamuneLib.Utils.ImageUtils.GetSprite("taco"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Rollerd", "American",
                RamuneLib.Utils.ImageUtils.GetSprite("Peeperburger"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Beka", "Japanese",
                RamuneLib.Utils.ImageUtils.GetSprite("rice"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Drinks", "Drinks",
                RamuneLib.Utils.ImageUtils.GetSprite("Tea"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "ELJuice", "Juice",
                RamuneLib.Utils.ImageUtils.GetSprite("juice1"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Yum", "Sweets",
                RamuneLib.Utils.ImageUtils.GetSprite("cake1"), new string[]
                {

                    "CIS",
                    "CF3"


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
                            Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(eggspriteName);

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
                                    if (eggData.IsWarmOrCold == true)
                                    {
                                        if (eggData.IsWarm == true)
                                        {
                                            eatable.coldMeterValue = -eggData.Warm;
                                        }
                                        else if (eggData.IsCold == true)
                                        {
                                            eatable.coldMeterValue = eggData.Cold;
                                        }
                                    }

                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.ArcticPeeper);
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
                           
                            else if (eggData.MedKititem == true || eggData.Tankitem == true)
                            {
                                var medkit = new CloneTemplate(prefab.Info, TechType.FirstAidKit);
                                medkit.ModifyPrefab += obj =>
                                {
                                    Eatable eatable = obj.EnsureComponent<Eatable>();
                                    eatable.foodValue = eggData.Food;
                                    eatable.waterValue = eggData.Drink;
                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.ArcticPeeper);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);

                                if (eggData.MedKititem == true)
                                {
                                   
                                        

                                       
                                        CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                                         {
                                   "CIS",
                                   "FA"
                                         });
                                        prefab.SetGameObject(medkit);
                                        prefab.Register();
                                        if (eggData.Heal == true)
                                        {
                                            SurvivalHandler.GiveHealthOnConsume(prefab.Info.TechType, eggData.Health, true);
                                        }
                                    
                                }
                                else if (eggData.Oxygenitem == true)
                                {

                                    
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                                 {
                                              "CIS",
                                                "OX"
                                 });
                                    prefab.SetGameObject(medkit);
                                    prefab.Register();
                                    
                                        SurvivalHandler.GiveOxygenOnConsume(prefab.Info.TechType, eggData.Oxygen, true);
                                    
                                }
                            }
                            else if (eggData.OyxgenTankitem == true)
                            {
                                var tankitem = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                tankitem.ModifyPrefab += obj =>
                                {
                                    obj.EnsureComponent<Pickupable>();
                                    obj.GetAllComponentsInChildren<Oxygen>().Do(o => o.oxygenCapacity = eggData.TankOxygen);



                                };
                                prefab.SetEquipment(EquipmentType.Tank);
                                prefab.SetUnlock(TechType.ArcticPeeper);
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Equipment);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                                 {
                                 "CIS",
                                 "EQP"
                                 });
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
                                    if (eggData.IsWarmOrCold)
                                    {
                                        if (eggData.IsWarm == true)
                                        {
                                            eatable.coldMeterValue = -eggData.Warm;
                                        }
                                        else if (eggData.IsCold == true)
                                        {
                                            eatable.coldMeterValue = eggData.Cold;
                                        }
                                    }
                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.ArcticPeeper);
                                prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                            {
                                "CIS",
                                "CF3",
                                eggData.TabName
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
                                    if (eggData.IsWarmOrCold== true)
                                    {
                                        if (eggData.IsWarm == true)
                                        {
                                            eatable.coldMeterValue = -eggData.Warm;
                                        }
                                        else if (eggData.IsCold == true)
                                        {
                                            eatable.coldMeterValue = eggData.Cold;
                                        }
                                    }
                                    obj.EnsureComponent<Pickupable>();

                                };

                                prefab.SetUnlock(TechType.ArcticPeeper);
                                prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetRecipeFromJson(recipeText);
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                               {
                                   "CIS",
                                   "CF3",
                                   eggData.TabName
                               });
                                prefab.SetGameObject(food);
                                prefab.Register();


                            }
                            if (eggData.FramedPhoto == true)
                            {

                                var fphoto = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                fphoto.ModifyPrefab += obj =>
                                {
                                    ConstructableFlags constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Rotatable | ConstructableFlags.Ground | ConstructableFlags.Submarine | ConstructableFlags.Rotatable | ConstructableFlags.AllowedOnConstructable | ConstructableFlags.Outside | ConstructableFlags.Base | ConstructableFlags.Wall | ConstructableFlags.Ceiling;

                                    ConstructableBounds constructableBounds = obj.AddComponent<ConstructableBounds>();
                                    //Allow it to be placed inside bases and submarines on the ground, and can be rotated:
                                    GameObject model = obj.transform.Find(eggData.ObjectName)?.gameObject;

                                    if (model == null)
                                    {
                                        Debug.LogError($"Failed to find GameObject with name '{eggData.ObjectName}' for Deco '{eggName}'.");
                                        return;
                                    }
                                    if (eggData.FramedPhoto == true)
                                    {
                                      
                                        obj.EnsureComponent<Pickupable>();
                                        var renderer = obj.FindChild("posterframe_01_b_LOD0").FindChild("picture").GetComponent<MeshRenderer>();
                                        foreach (var m in renderer.materials)
                                        {
                                            Texture2D testPhotos = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                            m.mainTexture = testPhotos;
                                           // m.SetTexture("_SpecTex", testPhotos);


                                        }
                                    }
                                    PrefabUtils.AddConstructable(obj, prefab.Info.TechType, constructableFlags, model);
                                };
                                prefab.SetPdaGroupCategory(TechGroup.Personal, TechCategory.Tools);
                                string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                prefab.SetEquipment(EquipmentType.Hand).WithQuickSlotType(QuickSlotType.Selectable);
                                prefab.SetRecipeFromJson(recipeText);
                                CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType, new string[]
                        {
                                   "CIS",
                                   "PoolNoodle3"
                        });
                                prefab.WithAutoUnlock();
                                prefab.SetGameObject(fphoto);
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
                            else  if (eggData.HasStorage == true)
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
                                    if (eggData.Hlocker == true)
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
                            else if (eggData.IsBed)
                            {
                                var bed1 = new CloneTemplate(prefab.Info, TechType.BedEmmanuel);
                                bed1.ModifyPrefab += obj =>
                                {

                                    if (eggData.Bedone)
                                    {
                                        var TestPhoto = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                        var renderer = obj.FindChild("bed_01")
                                            .FindChild("bed_covers_delta_emmanuel")
                                            .FindChild("bed_covers_delta_emmanuel_LOD0")
                                            .GetComponent<MeshRenderer>();
                                        foreach (var m in renderer.materials)
                                        {
                                            m.mainTexture = TestPhoto;
                                            m.SetTexture("_SpecTex", TestPhoto);


                                        }
                                    }
                                    if (eggData.Bedthree)
                                    {
                                        var TestPhoto = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                        var renderer = obj.FindChild("bed_01")
                                            .FindChild("bed_covers_delta_emmanuel")
                                            .FindChild("bed_covers_delta_emmanuel_LOD0")
                                            .GetComponent<MeshRenderer>();
                                        foreach (var m in renderer.materials)
                                        {
                                            m.mainTexture = TestPhoto;
                                            m.SetTexture("_SpecTex", TestPhoto);


                                        }
                                    }
                                };
                                if (eggData.Bedone)
                                {
                                    prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);                                
                                    prefab.SetRecipeFromJson(recipeText);        
                                    prefab.WithAutoUnlock();
                                    prefab.SetGameObject(bed1);
                                    prefab.Register();
                                }
                                if (eggData.Bedthree)
                                {
                                    prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);                                
                                    prefab.SetRecipeFromJson(recipeText);        
                                    prefab.WithAutoUnlock();
                                    prefab.SetGameObject(bed1);
                                    prefab.Register();
                                }

                            }
                            else if (eggData.Bed)
                            {
                                var bed2 = new CloneTemplate(prefab.Info, TechType.BedFred);
                                bed2.ModifyPrefab += obj =>
                                {
                                    
                                    
                                   
                                   

                                    if (eggData.Bedtwo)
                                    {
                                        var TestPhoto = RamuneLib.Utils.ImageUtils.GetTexture(eggData.InternalName);
                                        var renderer = obj.FindChild("bed_narrow")
                                            .FindChild("bed_covers_outpostzero_lil")
                                            .FindChild("bed_covers_outpostzero_lil_LOD0")
                                            .GetComponent<MeshRenderer>();
                                        foreach (var m in renderer.materials)
                                        {
                                            m.mainTexture = TestPhoto;
                                            m.SetTexture("_SpecTex", TestPhoto);


                                        }
                                    }
                                    
                                    

                                   


                                };
                                if (eggData.Bedtwo)
                                {
                                    prefab.SetPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule).SetBuildable();
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);                                
                                    prefab.SetRecipeFromJson(recipeText);        
                                    prefab.WithAutoUnlock();
                                    prefab.SetGameObject(bed2);
                                    prefab.Register();
                                }
                                
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
                                        var cude = obj.transform.Find("Cube");
                                        cude.localScale = Vector3.one / 0002;
                                        model.transform.localScale = Vector3.one / 0002;
                                    }
                                    else if (eggData.IsBasicArtifact == true)
                                    {
                                        GameObject.DestroyImmediate(obj.GetComponent<SkyApplier>());
                                        Nautilus.Utility.MaterialUtils.ApplySNShaders(obj);
                                    }
                                    else if (eggData.Stairs == true)
                                    {
                                       
                                        foreach (Transform child in obj.transform)
                                        {
                                            BoxCollider collider = child.gameObject.EnsureComponent<BoxCollider>();
                                            collider.center = Vector3.zero;
                                            collider.size = Vector3.one;
                                        }
                                        // Find the main model of the curtains
                                       

                                        // Add BoxCollider to the main model to allow placement detection


                                        // Change the layer of the main model to the Default layer to avoid red placement indicators
                                        model.layer = LayerMask.NameToLayer("Default");
                                    }
                                    else if (eggData.Curtains == true)
                                    {
                                       
                                        obj.EnsureComponent<Constructable>().forceUpright = true;
                                        
                                        constructableBounds.bounds.size *= 0.09f;

                                        

                                        // Add BoxCollider to the main model to allow placement detection
                                        BoxCollider mainModelCollider = model.EnsureComponent<BoxCollider>();
                                        mainModelCollider.center = new Vector3(0f, 0f, 0.14f); // Adjust the center to the desired position
                                        mainModelCollider.size = new Vector3(3.0f, 0.1f, 3.0f);

                                        // Change the layer of the main model to the Default layer to avoid red placement indicators
                                        model.layer = LayerMask.NameToLayer("Default");

                                       
                                    }
                                    
                                    else if (eggData.Blinds == true)
                                    {
                                        // Loop through all the child objects of the blinds and add BoxColliders to them
                                        foreach (Transform child in obj.transform)
                                        {
                                            BoxCollider collider = child.gameObject.EnsureComponent<BoxCollider>();
                                            collider.center = Vector3.zero;
                                            collider.size = Vector3.one;
                                        }


                                        BoxCollider mainModelCollider = model.EnsureComponent<BoxCollider>();
                                        mainModelCollider.center = new Vector3(0f, 0.18f, 0.28f);
                                        mainModelCollider.size = new Vector3(3.0f, 0.1f, 3.0f);

                                        // Change the layer of the main model to the Default layer to avoid red placement indicators
                                        model.layer = LayerMask.NameToLayer("Default");
                                    }
                                    else if (eggData.Camera == true)
                                    {
                                        foreach (Transform child in obj.transform)
                                        {
                                            BoxCollider collider = child.gameObject.EnsureComponent<BoxCollider>();
                                            collider.center = Vector3.zero;
                                            collider.size = Vector3.one;
                                        }
                                        
                                        // Change the layer of the main model to the Default layer to avoid red placement indicators
                                        model.layer = LayerMask.NameToLayer("Default");
                                    }
                                    else if (eggData.Tablet == true)
                                    {
                                        GameObject.DestroyImmediate(obj.GetComponent<Pickupable>());



                                        // Change the layer of the main model to the Default layer to avoid red placement indicators
                                        model.layer = LayerMask.NameToLayer("Default");
                                    }
                                    else if (eggData.BuildableAddCollider == true)
                                    {
                                        foreach (Transform child in obj.transform)
                                        {
                                            BoxCollider collider = child.gameObject.EnsureComponent<BoxCollider>();
                                        collider.center = Vector3.zero;
                                        collider.size = Vector3.one;
                                        }

                                        BoxCollider mainModelCollider = model.EnsureComponent<BoxCollider>();
                                        mainModelCollider.center = new Vector3(0f, 0.18f, 0.28f);
                                        mainModelCollider.size = new Vector3(3.0f, 0.1f, 3.0f);
                                        obj.EnsureComponent<Constructable>().placeDefaultDistance = eggData.DefaultPlaceDistance;
                                        obj.EnsureComponent<Constructable>().placeMaxDistance = eggData.PlaceMaxDistance;

                                        model.layer = LayerMask.NameToLayer("Default");
                                    }
                                    else if (eggData.BasicBuildable == true)
                                    {
                                       
                                        obj.EnsureComponent<Constructable>().placeDefaultDistance = eggData.DefaultPlaceDistance;
                                        obj.EnsureComponent<Constructable>().placeMaxDistance = eggData.PlaceMaxDistance;

                                        if(eggData.Force == true)
                                        {
                                            obj.EnsureComponent<Constructable>().forceUpright = eggData.Forceupright;
                                        }
                                    }
                                    else if (eggData.NothingNeeded == true)
                                    {
                                       

                                        // No modifications needed
                                        if (eggData.Ping)
                                        {
                                            ping = obj.AddComponent<PingInstance>();

                                            ping.colorIndex = 0;
                                            ping._label = "None";
                                            ping.range = 10;
                                            ping.origin = model.transform;

                                        }
                                        if (eggData.Base == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = BasePing;
                                            obj.GetComponent<PingInstance>().SetLabel("Base");

                                        }
                                        else if (eggData.BaseL == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = BaseLargePing;
                                            obj.GetComponent<PingInstance>().SetLabel("LargeBase");

                                        }
                                        else if (eggData.Beacon == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = BeaconPing;
                                            obj.GetComponent<PingInstance>().SetLabel("Beacon");

                                        }
                                        else if (eggData.Signel == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Signalping;
                                            obj.GetComponent<PingInstance>().SetLabel("Signal");

                                        }
                                        else if (eggData.Custom == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = testping;
                                            obj.GetComponent<PingInstance>().SetLabel("Place Of Intrest");

                                        }
                                        else if (eggData.Egg == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = EggPing;
                                            obj.GetComponent<PingInstance>().SetLabel("Egg Location");

                                        }
                                        else if (eggData.Creature == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = CreaturePing;
                                            obj.GetComponent<PingInstance>().SetLabel("Creature");

                                        }
                                        else if (eggData.Flag1 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag1Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Flag1");

                                        }
                                        else if (eggData.Flag2 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag2Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Flag2");

                                        }
                                        else if (eggData.Flag3 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag3Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Flag3");

                                        }
                                        else if (eggData.Flag4 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag4Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("flag4");

                                        }
                                        else if (eggData.Flag5 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag5Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("flag5");

                                        }
                                        else if (eggData.Flag6 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag6Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Flag6");

                                        }
                                        else if (eggData.Flag7 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Flag7Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Flag7");

                                        }
                                        else if (eggData.Alien1 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Alien1Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien1");

                                        }
                                        else if (eggData.Alien2 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Alien2Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien2");
                                        }
                                        else if (eggData.Alien3 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Alien3Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien3");

                                        }
                                        else if (eggData.Alien4 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Alien4Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien4");

                                        }
                                        else if (eggData.Alien5 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Alien5Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien5");

                                        }
                                        else if (eggData.Alien6 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Alien6Ping;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien6");

                                        }
                                        else if (eggData.Drill == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = DrillPing;
                                            obj.GetComponent<PingInstance>().SetLabel("Drillable");

                                        }
                                        else if (eggData.Ship == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Ship;
                                            obj.GetComponent<PingInstance>().SetLabel("Ship");

                                        }
                                        else if (eggData.Check == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Check;
                                            obj.GetComponent<PingInstance>().SetLabel("Checked");

                                        }
                                        else if (eggData.Check2 == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Check2;
                                            obj.GetComponent<PingInstance>().SetLabel("Checked");

                                        }
                                        else if (eggData.CreepVine == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Creepvine;
                                            obj.GetComponent<PingInstance>().SetLabel("CreepVine Location");

                                        }
                                        else if (eggData.Cross == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Cross;
                                            obj.GetComponent<PingInstance>().SetLabel("Heal Item Location");

                                        }
                                        else if (eggData.Chest == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Chest;
                                            obj.GetComponent<PingInstance>().SetLabel("Chest Location");

                                        }
                                        else if (eggData.Arrow == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Arrow;
                                            obj.GetComponent<PingInstance>().SetLabel("ThisWay");

                                        }
                                        else if (eggData.Fire == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Fire;
                                            obj.GetComponent<PingInstance>().SetLabel("FIRE");

                                        }
                                        else if (eggData.Acube == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Acube;
                                            obj.GetComponent<PingInstance>().SetLabel("Alien Location");

                                        }
                                        else if (eggData.Triangle == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = TriA;
                                            obj.GetComponent<PingInstance>().SetLabel("Marked Location");

                                        }
                                       
                                        else if (eggData.LifeSupport == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = LifeSupport;
                                            obj.GetComponent<PingInstance>().SetLabel("Breathable Location");

                                        }
                                        else if (eggData.Power == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Power;
                                            obj.GetComponent<PingInstance>().SetLabel("Power Active");

                                        }
                                        else if (eggData.Epoint == true)
                                        {


                                            obj.GetComponent<PingInstance>().pingType = Epoint;
                                            obj.GetComponent<PingInstance>().SetLabel("Location");

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

        private void LoadBatteryRequirements()
        {
            RequirementsLoaders loader = new RequirementsLoaders();
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
                        string eggTechtype1 = eggData.EggTechtype;
                        string eggTechtype2 = eggData.EggTechtype2;
                        string eggTechtype3 = eggData.EggTechtype3;
                        string eggTechtype4 = eggData.EggTechtype4;
                        string eggTechtype5 = eggData.EggTechtype5;
                        string eggTechtype6 = eggData.EggTechtype6;
                        string pcTechtype = eggData.PowerCellTechtype;
                        string unlockTechtype = eggData.unlockTechtype;
                        // Convert eggData.TechType to TechType to get the sprite
                        TechType eggTechTypes = GetTechType(eggTechtype1);
                        TechType eggTechTypes1 = GetTechType(eggTechtype2);
                        TechType eggTechTypes2 = GetTechType(eggTechtype3);
                        TechType eggTechTypes3 = GetTechType(eggTechtype4);
                        TechType eggTechTypes4 = GetTechType(eggTechtype5);
                        TechType eggTechTypes5 = GetTechType(eggTechtype6);
                        TechType powercellTechType = GetTechType(pcTechtype);
                        TechType setUnlockTechType = GetTechType(unlockTechtype);



                        // Split internal names and create/register the custom egg prefab for each TechType
                        foreach (string internalName in eggInterName.Split(','))
                        {
                            if (eggData.IsCBP == true)
                            {

                                var customBatteryName = new CbBattery()
                                {
                                    ID = eggData.InternalName + "Battery",
                                    Name = eggData.FriendlyName + " Battery",
                                    FlavorText = "A Battery " + eggData.Tooltip,
                                    EnergyCapacity = eggData.EnergyAmountB,
                                    CraftingMaterials = new List<TechType>()
                                    {
                                        eggTechTypes,
                                        eggTechTypes1,
                                        eggTechTypes2,
                                        eggTechTypes3,
                                        eggTechTypes4,
                                        eggTechTypes5
                                    },
                                    UnlocksWith = setUnlockTechType,




                                    CustomIcon = RamuneLib.Utils.ImageUtils.GetSprite(eggData.BatterySkin),
                                    CBModelData = new CBModelData()
                                    {
                                        CustomTexture = RamuneLib.Utils.ImageUtils.GetTexture(eggData.BatteryTexture),
                                        //CustomNormalMap = ImageUtils.LoadTextureFromFile(Path.Combine(AssetsFolder, "battery_normal.png")),
                                        CustomSpecMap = RamuneLib.Utils.ImageUtils.GetTexture(eggData.BatterySpec),
                                        CustomIllumMap = RamuneLib.Utils.ImageUtils.GetTexture(eggData.BatteryIllum),
                                        CustomIllumStrength = 1f,
                                        UseIonModelsAsBase = true,
                                    },


                                };
                                customBatteryName.Patch();
                                customBatterynames.Add(customBatteryName.TechType);
                                var skinPath = RamuneLib.Utils.ImageUtils.GetTexture(eggData.PCTexture);

                                var specPath = RamuneLib.Utils.ImageUtils.GetTexture(eggData.PCSpec);
                                var illumPath = RamuneLib.Utils.ImageUtils.GetTexture(eggData.PCIllum);

                                var skin = skinPath;
                                //var normal = ImageUtils.LoadTextureFromFile(normalPath);
                                var spec = specPath;
                                var illum = illumPath;


                                var customBatteryPowercell = new CbPowerCell()
                                {
                                    EnergyCapacity = eggData.EnergyAmountPC,
                                    ID = eggData.InternalName + "Powercell",
                                    Name = eggData.FriendlyName + " PowerCell",
                                    FlavorText = "A Power Cell " + eggData.Tooltip,


                                    CraftingMaterials = new List<TechType>()
                                        { customBatteryName.TechType, customBatteryName.TechType, powercellTechType },
                                    UnlocksWith = setUnlockTechType,

                                    CustomIcon = RamuneLib.Utils.ImageUtils.GetSprite(eggData.PCSkin),
                                    CBModelData = new CBModelData()
                                    {
                                        CustomTexture = skin,
                                        //CustomNormalMap = normal,
                                        CustomSpecMap = spec,
                                        CustomIllumMap = illum,
                                        CustomIllumStrength = 1f,
                                        UseIonModelsAsBase = true,
                                    },


                                };

                                customBatteryPowercell.Patch();
                                customBatterynames.Add(customBatteryPowercell.TechType);
                            }
                            BasicBatteryPrefab prefab = new BasicBatteryPrefab(internalName.Trim(), eggDisplayName,
                                eggDescription);
                        }
                    }

                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception processing Battery {eggEntry.Key}: {ex.Message}");
                    }
                }
            }
            else

            {
                Debug.LogError("Failed to load Battery info data.");
            }
        }
         private void LoadIngredientsRequirements()
        {
            IRequirementsLoaders loader = new IRequirementsLoaders();
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
                        
                        string eggObjectName = eggData.ObjectName;
                        string eggTechtype1 = eggData.EggTechtype;
                        string eggTechtype2 = eggData.EggTechtype2;
                        string eggTechtype3 = eggData.EggTechtype3;
                        string eggTechtype4 = eggData.EggTechtype4;
                        string eggTechtype5 = eggData.EggTechtype5;
                        string eggTechtype6 = eggData.EggTechtype6;
                        string SpriteName = eggData.Spritename;
                        string unlockTechtype = eggData.unlockTechtype;
                        // Convert eggData.TechType to TechType to get the sprite
                        TechType eggTechTypes = GetTechType(eggTechtype1);
                        TechType eggTechTypes1 = GetTechType(eggTechtype2);
                        TechType eggTechTypes2 = GetTechType(eggTechtype3);
                        TechType eggTechTypes3 = GetTechType(eggTechtype4);
                        TechType eggTechTypes4 = GetTechType(eggTechtype5);
                        TechType eggTechTypes5 = GetTechType(eggTechtype6);
                       
                        TechType setUnlockTechType = GetTechType(unlockTechtype);
                        
                        foreach (string internalName in eggInterName.Split(','))
                        {
                            
                            Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(SpriteName);

                            // Create and register the custom egg prefab
                            BasicingredientsPrefab prefab = new BasicingredientsPrefab(internalName.Trim(),eggDisplayName,eggDescription
                                ,eggSprite);

                            // Create CloneTemplate using ResourceId
                            CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);
                            if (eggData.Ingredients == true)
                            {
                                if (eggData.Ifood == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        if (eggData.IsWarmOrCold == true)
                                        {
                                            if (eggData.IsWarm == true)
                                            {
                                                eatable.coldMeterValue = -eggData.Warm;
                                            }
                                            else if (eggData.IsCold == true)
                                            {
                                                eatable.coldMeterValue = eggData.Cold;
                                            }
                                        }
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.Idrink == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        if (eggData.IsWarmOrCold == true)
                                        {
                                            if (eggData.IsWarm == true)
                                            {
                                                eatable.coldMeterValue = -eggData.Warm;
                                            }
                                            else if (eggData.IsCold == true)
                                            {
                                                eatable.coldMeterValue = eggData.Cold;
                                            }
                                        }
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.IItem == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                       
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "IDT"
                                        });
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
         private void LoadPreIngredientsRequirements()
        {
            IRequirementsLoaderss loader = new IRequirementsLoaderss();
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
                        
                        string eggObjectName = eggData.ObjectName;
                        string eggTechtype1 = eggData.EggTechtype;
                        string eggTechtype2 = eggData.EggTechtype2;
                        string eggTechtype3 = eggData.EggTechtype3;
                        string eggTechtype4 = eggData.EggTechtype4;
                        string eggTechtype5 = eggData.EggTechtype5;
                        string eggTechtype6 = eggData.EggTechtype6;
                        string SpriteName = eggData.Spritename;
                        string unlockTechtype = eggData.unlockTechtype;
                        // Convert eggData.TechType to TechType to get the sprite
                        TechType eggTechTypes = GetTechType(eggTechtype1);
                        TechType eggTechTypes1 = GetTechType(eggTechtype2);
                        TechType eggTechTypes2 = GetTechType(eggTechtype3);
                        TechType eggTechTypes3 = GetTechType(eggTechtype4);
                        TechType eggTechTypes4 = GetTechType(eggTechtype5);
                        TechType eggTechTypes5 = GetTechType(eggTechtype6);
                       
                        TechType setUnlockTechType = GetTechType(unlockTechtype);
                        
                        foreach (string internalName in eggInterName.Split(','))
                        {
                            
                            Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(SpriteName);

                            // Create and register the custom egg prefab
                            BasicingredientsPrefab prefab = new BasicingredientsPrefab(internalName.Trim(),eggDisplayName,eggDescription
                                ,eggSprite);

                            // Create CloneTemplate using ResourceId
                            CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);
                            if (eggData.PreIngredients == true)
                            {
                                if (eggData.PIfood == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        if (eggData.IsWarmOrCold == true)
                                        {
                                            if (eggData.IsWarm == true)
                                            {
                                                eatable.coldMeterValue = -eggData.Warm;
                                            }
                                            else if (eggData.IsCold == true)
                                            {
                                                eatable.coldMeterValue = eggData.Cold;
                                            }
                                        }
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PIdrink == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PIItem == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                       
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "IDT"
                                        });
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
          private void LoadPrePreIngredientsRequirements()
        {
            IRequirementsLoadersss loader = new IRequirementsLoadersss();
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
                        
                        string eggObjectName = eggData.ObjectName;
                        string eggTechtype1 = eggData.EggTechtype;
                        string eggTechtype2 = eggData.EggTechtype2;
                        string eggTechtype3 = eggData.EggTechtype3;
                        string eggTechtype4 = eggData.EggTechtype4;
                        string eggTechtype5 = eggData.EggTechtype5;
                        string eggTechtype6 = eggData.EggTechtype6;
                        string SpriteName = eggData.Spritename;
                        string unlockTechtype = eggData.unlockTechtype;
                        // Convert eggData.TechType to TechType to get the sprite
                        TechType eggTechTypes = GetTechType(eggTechtype1);
                        TechType eggTechTypes1 = GetTechType(eggTechtype2);
                        TechType eggTechTypes2 = GetTechType(eggTechtype3);
                        TechType eggTechTypes3 = GetTechType(eggTechtype4);
                        TechType eggTechTypes4 = GetTechType(eggTechtype5);
                        TechType eggTechTypes5 = GetTechType(eggTechtype6);
                       
                        TechType setUnlockTechType = GetTechType(unlockTechtype);
                        
                        foreach (string internalName in eggInterName.Split(','))
                        {
                            
                            Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(SpriteName);

                            // Create and register the custom egg prefab
                            BasicingredientsPrefab prefab = new BasicingredientsPrefab(internalName.Trim(),eggDisplayName,eggDescription
                                ,eggSprite);

                            // Create CloneTemplate using ResourceId
                            CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);
                            if (eggData.PPIngredients == true)
                            {
                                if (eggData.PPIfood == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PPIdrink == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PPIItem == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                       
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "IDT"
                                        });
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
            private void LoadPrePrePreIngredientsRequirements()
        {
            IRequirementsLoaderssss loader = new IRequirementsLoaderssss();
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
                        
                        string eggObjectName = eggData.ObjectName;
                        string eggTechtype1 = eggData.EggTechtype;
                        string eggTechtype2 = eggData.EggTechtype2;
                        string eggTechtype3 = eggData.EggTechtype3;
                        string eggTechtype4 = eggData.EggTechtype4;
                        string eggTechtype5 = eggData.EggTechtype5;
                        string eggTechtype6 = eggData.EggTechtype6;
                        string SpriteName = eggData.Spritename;
                        string unlockTechtype = eggData.unlockTechtype;
                        // Convert eggData.TechType to TechType to get the sprite
                        TechType eggTechTypes = GetTechType(eggTechtype1);
                        TechType eggTechTypes1 = GetTechType(eggTechtype2);
                        TechType eggTechTypes2 = GetTechType(eggTechtype3);
                        TechType eggTechTypes3 = GetTechType(eggTechtype4);
                        TechType eggTechTypes4 = GetTechType(eggTechtype5);
                        TechType eggTechTypes5 = GetTechType(eggTechtype6);
                       
                        TechType setUnlockTechType = GetTechType(unlockTechtype);
                        
                        foreach (string internalName in eggInterName.Split(','))
                        {
                            
                            Sprite eggSprite = RamuneLib.Utils.ImageUtils.GetSprite(SpriteName);

                            // Create and register the custom egg prefab
                            BasicingredientsPrefab prefab = new BasicingredientsPrefab(internalName.Trim(),eggDisplayName,eggDescription
                                ,eggSprite);

                            // Create CloneTemplate using ResourceId
                            CloneTemplate cloneTemplate = new CloneTemplate(prefab.Info, eggData.ResourceId);
                            if (eggData.PPPIngredients == true)
                            {
                                if (eggData.PPPIfood == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PPPIdrink == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                        Eatable eatable = obj.EnsureComponent<Eatable>();
                                        eatable.foodValue = eggData.Food;
                                        eatable.waterValue = eggData.Drink;
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "CF3",
                                            eggData.TabName
                                        });
                                    prefab.SetGameObject(food);
                                    prefab.Register();


                                }
                                else if (eggData.PPPIItem == true)
                                {
                                    var food = new CloneTemplate(prefab.Info, eggData.ResourceId);
                                    food.ModifyPrefab += obj =>
                                    {
                                       
                                        obj.EnsureComponent<Pickupable>();

                                    };

                                    prefab.SetUnlock(TechType.ArcticPeeper);
                                    prefab.SetPdaGroupCategory(TechGroup.Survival, TechCategory.FoodAndDrinks);
                                    string recipeText = RamuneLib.Utils.JsonUtils.GetJsonRecipe(eggData.InternalName);
                                    prefab.SetRecipeFromJson(recipeText);
                                    CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, prefab.Info.TechType,
                                        new string[]
                                        {
                                            "CIS",
                                            "IDT"
                                        });
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
