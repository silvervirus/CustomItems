using System;
using System.Linq;
using UnityEngine;

namespace CustomItems.Util;

public static class Cookay_sUtil
{
      public static TechType GetTechType(string techTypeStr)
        {
            // Check if the TechType exists as a modded enum
            if (ModdedEnumExists<TechType>(techTypeStr))
            {
                Debug.Log($"Modded enum found: {techTypeStr}");

                // If the modded enum exists, try to parse it using TechTypeExtensions
                if (TechTypeExtensions.FromString(techTypeStr, out TechType moddedTechType, true))
                {
                    Debug.Log($"Successfully parsed modded TechType: {moddedTechType}");
                    return moddedTechType; // Return the modded TechType
                }
                else
                {
                    Debug.LogError($"Failed to parse modded TechType: {techTypeStr}");
                }
            }
    
            // Try parsing using Enum.TryParse first
            if (Enum.TryParse(techTypeStr, out TechType result))
            {
                Debug.Log($"Successfully parsed standard Enum: {result}");
                return result; // Successfully parsed using Enum
            }

            // If Enum parsing fails, try TechTypeExtensions.FromString
            else if (TechTypeExtensions.FromString(techTypeStr, out TechType result1, true))
            {
                Debug.Log($"Successfully parsed TechType using FromString: {result1}");
                return result1; // Successfully parsed using FromString
            }

           
           

           
            Debug.LogError($"Failed to parse TechType for: {techTypeStr}");
            return TechType.None;
        }

     
        public static string[] ParsePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("[TabLoader] No path specified — registering tab at root level.");
                return new string[] { };
            }

            var parts = path.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToArray();

            Console.WriteLine($"[TabLoader] Parsed path: [{string.Join(" > ", parts)}]");

            return parts;
        }


            public static bool ModdedEnumExists<TEnum>(string name) where TEnum : Enum
        {
            // Get all enum values for the specified Enum type
            Array enumValues = Enum.GetValues(typeof(TEnum));

            // Check if the name exists in the enum values
            foreach (var value in enumValues)
            {
                if (value.ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // If the name does not match any of the enum values, return false
            return false;
        }
}