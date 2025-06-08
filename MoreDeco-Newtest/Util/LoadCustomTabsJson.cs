using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace CustomItems.Util;

public class LoadCustomTabsJson
{
    public class RequirementsTabsLoaders
    {
        private readonly string _pluginDirectory;

        public RequirementsTabsLoaders()
        {
            _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public Dictionary<string, EggData.EggInfoData> LoadAllEggInfo()
        {
            var eggInfoData = new Dictionary<string, EggData.EggInfoData>();
            try
            {
                string itemsDirectory = Path.Combine(_pluginDirectory, "Tabs");

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
                        EggData.EggInfoData eggData = JsonConvert.DeserializeObject<EggData.EggInfoData>(jsonData);

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
}