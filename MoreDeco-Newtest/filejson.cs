using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Customitems.Loader
{
    public class RequirementsLoader
    {
        private readonly string _pluginDirectory;

        public RequirementsLoader()
        {
            _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public Dictionary<string, EggInfoData> LoadAllEggInfo()
        {
            var eggInfoData = new Dictionary<string, EggInfoData>();
            try
            {
                string itemsDirectory = Path.Combine(_pluginDirectory, "Items");

                // Ensure the directory exists
                if (!Directory.Exists(itemsDirectory))
                {
                    Console.WriteLine($"Directory '{itemsDirectory}' does not exist.");
                    return eggInfoData;
                }

                // Get all JSON files in the directory
                string[] jsonFiles = Directory.GetFiles(itemsDirectory, "*.json");

                foreach (string jsonFilePath in jsonFiles)
                {
                    try
                    {
                        string jsonData = File.ReadAllText(jsonFilePath);
                        EggInfoData eggData = JsonConvert.DeserializeObject<EggInfoData>(jsonData);

                        // Use the internal name as the key
                        string internalName = Path.GetFileNameWithoutExtension(jsonFilePath);
                        eggInfoData.Add(internalName, eggData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception loading JSON file '{jsonFilePath}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception accessing JSON directory: {ex.Message}");
            }

            return eggInfoData;
        }
    }

    public class EggInfoData
    {
        public bool Buildable { get; set; }
        public bool CustomHullPlates { get; set; }
        public bool Placeable { get; set; }
        public string Tooltip { get; set; }
        public string FriendlyName { get; set; }
        public string InternalName { get; set; }
        public string Spritename { get; set; }
        public string ResourceId { get; set; }
        public string ObjectName { get; set; }
        public string ExtraObjectName { get; set; }
        public string ExtraObjectName2 { get; set; }
        public bool IsArtifact { get; set; }
        public bool IsBasicArtifact { get; set; }
        public bool IsAnArtifact { get; set; }
        public bool IsTable { get; set; }
        public bool HasOtherItems { get; set; }
        public bool NothingNeeded { get; set; }
        public bool IsGunArtifact { get; set; }
        public bool Pen { get; set; }
        public bool HasStorage { get; set; }
        public bool AddStorage { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Drink { get; set; }
        public int Food { get; set; }
        public bool Isfood { get; set; }
        public bool Isdrink { get; set; }
        public bool ChipItem { get; set; }
        public bool HealItem { get; set; }
        public bool MedKititem { get; set; }
        public bool Tankitem { get; set; }
        public float Oxygen { get; set; }
        public float Health { get; set; }
        public bool IsPosterV { get; set; }
        public bool IsPosterH { get; set; }
        public bool IsmultObject { get; set; }
        public bool IsBmultObject { get; set; }
        public bool OneExtraB { get; set; }
        public bool TwoExtraB { get; set; }
        public bool OneExtra { get; set; }
        public bool TwoExtra { get; set; }
        public bool Hlocker { get; set; }

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
