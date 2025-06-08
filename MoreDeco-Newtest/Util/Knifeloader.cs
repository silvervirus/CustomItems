using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomItems.Util;

public class KnifeLoader
{
    private static string _pluginDirectory;

    private static readonly string KnifeFolder = Path.Combine(_pluginDirectory, "Knifes");

    public static Dictionary<string, EggData.EggInfoData> LoadAllKnifeData()
    {
        _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var knives = new Dictionary<string, EggData.EggInfoData>();

        if (!Directory.Exists(KnifeFolder))
            Directory.CreateDirectory(KnifeFolder);

        foreach (string file in Directory.GetFiles(KnifeFolder, "*.json"))
        {
            try
            {
                string json = File.ReadAllText(file);
                EggData.EggInfoData data = JsonConvert.DeserializeObject<EggData.EggInfoData>(json);
                if (!string.IsNullOrEmpty(data.InternalName))
                {
                    knives[data.InternalName] = data;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load knife data from {file}: {e}");
            }
        }

        return knives;
    }
}
