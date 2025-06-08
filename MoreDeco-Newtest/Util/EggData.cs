using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CustomItems.Util;

public class EggData
{
    public class EggInfoData
    {
        //Default Strings
        public string Tooltip { get; set; }
        public string FriendlyName { get; set; }
        public string InternalName { get; set; }
        public string Spritename { get; set; }
        public string ResourceId { get; set; }
        public string ObjectName { get; set; }
        //------------------------------------------
        //Buildable Bools and Ints 
        public bool Buildable { get; set; }
        public bool CustomHullPlates { get; set; }
        public bool Placeable { get; set; }
        public bool Curtains { get; set; }
        public bool Blinds { get; set; }
        public bool Camera { get; set; }
        public bool Tablet { get; set; }
        public bool Stairs { get; set; }
        public bool BasicBuildable { get; set; }
        public bool BuildableAddCollider { get; set; }
        public bool Forceupright { get; set; }
        public bool Force { get; set; }
        public float DefaultPlaceDistance { get; set; }
        public float PlaceMaxDistance { get; set; }
        public bool FramedPhoto { get; set; }
        public bool IsBmultObject { get; set; }
        public bool OneExtraB { get; set; }
        public bool TwoExtraB { get; set; }
        public bool IsArtifact { get; set; }
        public bool IsBasicArtifact { get; set; }
        public bool IsAnArtifact { get; set; }
        public bool IsTable { get; set; }
        public bool HasOtherItems { get; set; }
        public bool NothingNeeded { get; set; }
        public bool IsGunArtifact { get; set; }
        public bool Pen { get; set; }
        public bool AddStorage { get; set; }
        //-------------------------------------
        // Storage bools and ints 
        public bool HasStorage { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool Hlocker { get; set; }
        public bool IsmultObject { get; set; }
        public bool OneExtra { get; set; }
        public bool TwoExtra { get; set; }
        public string ExtraObjectName { get; set; }
        public string ExtraObjectName2 { get; set; }
        //---------------------------------
        //Food and Drink Bools and Ints 
        public int Drink { get; set; }
        public int Food { get; set; }
        public int Cold { get; set; }
        public int Warm { get; set; }
        public bool Isfood { get; set; }
        public bool IsWarm { get; set; }
        public bool IsCold { get; set; }
        public bool IsWarmOrCold { get; set; }
        public bool Isdrink { get; set; }
        //---------------------------------
        //CustomFoods 3 Bools and Ints
        public bool CF3 { get; set; }
        public string TabName { get; set; }    
        //Ingredients
        public bool Ingredients { get; set; }
        public bool Ifood { get; set; }
        public bool IItem { get; set; }

        public bool Idrink { get; set; }

        //-------------------------------------
        //Advanced Ingredients
        public bool PreIngredients { get; set; }
        public bool PIfood { get; set; }
        public bool PIItem { get; set; }
        public bool PIdrink { get; set; }
        public bool AdvancedIngredients { get; set; }
        public bool Advancedfood { get; set; }
        public bool AdvancedItem { get; set; }

        public bool Advanceddrink { get; set; }

        //---------------------------------------
        //Combined Ingredients
        public bool PPIngredients { get; set; }
        public bool PPIfood { get; set; }
        public bool PPIItem { get; set; }
        public bool PPIdrink { get; set; }
        public bool CombinedIngredients { get; set; }
        public bool CombinedFood { get; set; }
        public bool CombinedItem { get; set; }

        public bool Combineddrink { get; set; }

        //-------------------------------------------
        //Basic Ingredients 
        public bool PPPIngredients { get; set; }

        public bool PPPIfood { get; set; }
        public bool PPPIItem { get; set; }
        public bool PPPIdrink { get; set; }
        public bool BasicIngredients { get; set; }
        public bool BasicFood { get; set; }
        public bool BasicDrink { get; set; }

        public bool BasicItem { get; set; }

        //--------------------------------------
        // Heal and Tank Items Bools and Ints 
        public bool ChipItem { get; set; }
        public bool HealItem { get; set; }
        public bool MedKititem { get; set; }
        public bool Heal { get; set; }
        public bool Oxygenitem { get; set; }
        public bool Tankitem { get; set; }
        public float Oxygen { get; set; }
        public bool OyxgenTankitem { get; set; }
        public float TankOxygen { get; set; }
        public float Health { get; set; }
        //-----------------------------------
        public bool IsPosterV { get; set; }
        public bool IsPosterH { get; set; }
        //pings
        public bool Base { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        public bool Flag3 { get; set; }
        public bool Flag4 { get; set; }
        public bool Flag5 { get; set; }
        public bool Flag6 { get; set; }
        public bool Flag7 { get; set; }
        public bool Alien1 { get; set; }
        public bool Alien2 { get; set; }
        public bool Alien3 { get; set; }
        public bool Alien4 { get; set; }
        public bool Alien5 { get; set; }
        public bool Alien6 { get; set; }
        public bool Egg { get; set; }
        public bool Creature { get; set; }
        public bool Drill { get; set; }
        public bool BaseL { get; set; }
        public bool Beacon { get; set; }
        public bool Ping { get; set; }
        public bool Signel { get; set; }
        public bool Fire { get; set; }
        public bool Triangle { get; set; }
        public bool Cross { get; set; }
        public bool Check { get; set; }
        public bool Check2 { get; set; }
        public bool Skull { get; set; }
        public bool Capule { get; set; }
        public bool Chest { get; set; }
        public bool Dish { get; set; }
        public bool Acube { get; set; }
        public bool Ship { get; set; }
        public bool Radio { get; set; }
        public bool Epoint { get; set; }
        public bool LifeSupport { get; set; }
        public bool Arrow { get; set; }
        public bool Power { get; set; }
        public bool Important { get; set; }
        public bool CreepVine { get; set; }
        public bool Custom { get; set; }
        //----------------------------------------
        public bool IsCBP { get; set; }
        public string unlockTechtype { get; set; }
        public string CustomBatteryName { get; set; }
        public string PowerCellTechtype { get; set; }
        public string EggTechtype { get; set; }
        public string EggTechtype1 { get; set; }
        public string EggTechtype2 { get; set; }
        public int EnergyAmountB { get; set; }
        public string BatterySkin  { get; set; }
        public string BatteryTexture  { get; set; }
        public string BatterySpec  { get; set; }
        public string BatteryIllum  { get; set; }
        public string PCSkin  { get; set; }
        public string PCTexture  { get; set; }
        public string PCSpec  { get; set; }
        public string PCIllum  { get; set; }
        public int EnergyAmountPC { get; set; }
        public string EggTechtype3 { get; set; }
        public string EggTechtype4 { get; set; }
        public string EggTechtype5 { get; set; }
        public string EggTechtype6 { get; set; }
        //-------------------------------------------------
        //CustomTabs bools and Strings
        public bool UseCustomTab { get; set; }
        public bool UseDefaultTab { get; set; }
        public bool UseItemTab { get; set; }
        public string TabID { get; set; }
        public bool Maintab { get; set; }
        public bool Subtab { get; set; }
        public bool Sibtab { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CraftTree.Type TreeType { get; set; } = CraftTree.Type.Fabricator;
        
        public string Path { get; set; } // Tab path as a string, e.g. "CIS,Food"
        public string TabMainName { get; set; }
        public string TabSubName { get; set; }

        public string TabSibName { get; set; }

        //--------------------------------------------
        //CustomKnife Strings and Bools 
        public bool IsKnife { get; set; }
        public float Hitforce { get; set; }
        public bool IsHotKnife { get; set; }
        public float Damage { get; set; }

        public float Range { get; set; }

        //-----------------------------------

        private static EggInfoData instance;
        public static EggInfoData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EggInfoData();
                }
                return instance;
            }
        }
    }
}
