using System;
using System.Collections.Generic;
using CustomItems.Util;
using static CustomItems.Util.Cookay_sUtil;
using Nautilus.Handlers;

namespace CustomItems.Core;

public class CustomTabs
{
  public static void RegisterCustomTabs()
     {
         LoadCustomTabsJson.RequirementsTabsLoaders loaders = new LoadCustomTabsJson.RequirementsTabsLoaders();
         Dictionary<string, EggData.EggInfoData> tabDefs = loaders.LoadAllEggInfo();

            foreach (var entry in tabDefs)
            {
                EggData.EggInfoData tab = entry.Value;

                if (string.IsNullOrEmpty(tab.TabID))
                {
                    Console.WriteLine($"[TabLoader] Skipped tab (missing TabID): {entry.Key}");
                    continue;
                }

                var sprite = RamuneLib.Utils.ImageUtils.GetSprite(tab.Spritename ?? tab.Spritename ?? "DefaultTabIcon");
                var path = ParsePath(tab.Path); // <-- Leave your ParsePath logging in place

                if (tab.Maintab && !string.IsNullOrEmpty(tab.TabName))
                {
                    Console.WriteLine($"[MainTab] Registering '{tab.TabName}' (ID: {tab.TabID}) at {tab.TreeType} > {tab.Path}");
                    CraftTreeHandler.AddTabNode(tab.TreeType, tab.TabID, tab.TabName, sprite, path);
                }
                else if (tab.Subtab && !string.IsNullOrEmpty(tab.TabName))
                {
                    Console.WriteLine($"[SubTab] Registering '{tab.TabName}' (ID: {tab.TabID}) at {tab.TreeType} > {tab.Path}");
                    CraftTreeHandler.AddTabNode(tab.TreeType, tab.TabID, tab.TabName, sprite, path);
                }
                else if (tab.Sibtab && !string.IsNullOrEmpty(tab.TabName))
                {
                    Console.WriteLine($"[SibTab] Registering '{tab.TabName}' (ID: {tab.TabID}) at {tab.TreeType} > {tab.Path}");
                    CraftTreeHandler.AddTabNode(tab.TreeType, tab.TabID, tab.TabName, sprite, path);
                }
                else
                {
                    Console.WriteLine($"[TabLoader] Skipped '{tab.TabID}': no valid tab type flag set or missing name.");
                }

                // ⬇️ Moved this to bottom, as requested
                Console.WriteLine(path.Length == 0
                    ? "[TabLoader] No path specified — registering tab at root level."
                    : $"[TabLoader] Parsed path: [{string.Join(" > ", path)}]");
            }
     }
}