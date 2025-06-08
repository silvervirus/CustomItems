using System;
using System.Collections.Generic;
using CustomBatteries.API;
using CustomItems.Util;
using Nautilus.Handlers;
using UnityEngine;
using static CustomItems.Util.Cookay_sUtil;
using static CustomItems.Util.UtraBasePrefab;

namespace CustomItems.Core;

public class CustomBatteries
{
    
    public static readonly List<TechType> typesToMakePickupables = new();
    public static readonly List<TechType> customBatterynames = new();
      public static void LoadBatteryRequirements()
        {
            RequirementsLoaders loader = new RequirementsLoaders();
            Dictionary<string, EggData.EggInfoData> allEggInfo = loader.LoadAllEggInfo();

            if (allEggInfo.Count > 0)
            {
                foreach (var eggEntry in allEggInfo)
                {
                    try
                    {
                        // string eggName = eggEntry.Key;
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
                                LanguageHandler.SetLanguageLine(customBatteryName.ID,
                                    customBatteryName.Name + " " + customBatteryName.FlavorText);
                                LanguageHandler.SetLanguageLine(customBatteryPowercell.ID,
                                    customBatteryPowercell.Name + " " + customBatteryPowercell.FlavorText);
                                LanguageHandler.SetTechTypeTooltip(customBatteryPowercell.TechType, "");
                                LanguageHandler.SetTechTypeTooltip(customBatteryName.TechType, "");


                            }

                            UnifiedPrefabsCombined prefab = new UnifiedPrefabsCombined(internalName.Trim(), eggDisplayName,
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
}