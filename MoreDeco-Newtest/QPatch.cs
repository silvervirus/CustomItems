using BepInEx;
using HarmonyLib;
using static CustomItems.Core.BasicIngredents;
using static CustomItems.Core.AdvancedIngredents;
using static CustomItems.Core.CombinedIngredents;
using static CustomItems.Core.Ingredents;
using static CustomItems.Core.LoadDefaultTabs;
using static CustomItems.Core.CustomBatteries;
using static CustomItems.Core.CustomItems;
using static CustomItems.Core.CustomTabs;


namespace CustomItems
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.mrpurple6411.CustomBatteries", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.anytypemod", BepInDependency.DependencyFlags.SoftDependency)]

    public class QPatch : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "Com.Cookay.CustomItems";
        public const string PLUGIN_NAME = "CustomItems";
        public const string PLUGIN_VERSION = "1.0.0.3";
        private static readonly Harmony harmony = new Harmony(PLUGIN_GUID);
        
        public void Awake()
        {
            
            harmony.PatchAll();
            Maketabs();
            RegisterCustomTabs();
            LoadBasicIngredientsRequirements();
            LoadCombinedIngredientsRequirements();
            LoadAdvancedIngredientsRequirements();
            LoadIngredientsRequirements();
            LoadCustomItems();
            LoadBatteryRequirements();
            
        }
        
    }
}

                    
                
