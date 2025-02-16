using System.Collections.Generic;
using Nautilus.Json;
using Debug = UnityEngine.Debug;


namespace CustomItems
{
    public class MyConfig : ConfigFile
    {
        // Define properties for storing tooltips for each egg
        public Dictionary<string, string> EggTooltips { get; set; } = new Dictionary<string, string>();

        // Singleton instance for easy access
        private static MyConfig instance;
        public static MyConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyConfig();
                    instance.Load(); // Optionally load the configuration here
                }
                return instance;
            }
        }

        // Define a TryGet method to retrieve tooltip values
        public bool TryGet(string eggName, out string tooltip)
        {
            return EggTooltips.TryGetValue(eggName, out tooltip);
        }

        // Method to load configuration and patch the tooltips
        public static void PatchMethod()
        {
            Instance.Load();

            // Load egg tooltips and requirements from JSON file
            var requirementsLoader = new RequirementsLoader();
            var eggInfoData = requirementsLoader.LoadAllEggInfo();

            if (eggInfoData == null)
            {
                Debug.LogError("Failed to load configuration data.");
                return;
            }

            // Assign loaded egg tooltips to the EggTooltips dictionary
            foreach (var pair in eggInfoData)
            {
                if (pair.Value != null)
                {
                    Instance.EggTooltips[pair.Key] = pair.Value.Tooltip;
                }
                else
                {
                    Debug.LogWarning($"EggInfoData for {pair.Key} is null.");
                }
            }

            // Print egg tooltips for debugging purposes
            foreach (var pair in Instance.EggTooltips)
            {
                string eggName = pair.Key;
                string eggTooltip = pair.Value;
                Debug.Log($"{eggName} tooltip: {eggTooltip}");
            }
           
            
        }
      
       
    }
}
