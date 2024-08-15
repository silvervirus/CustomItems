using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Diagnostics.CodeAnalysis;
using Customitems.Loader;
using Customitems.info;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Nautilus.Utility;
using static UWE.FreezeTime;
using PrefabUtils = Nautilus.Utility.PrefabUtils;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Customitems.main;
namespace Customitems.info
{
    public class BasicEggPrefab : CustomPrefab
    {
        public static TechGroup group;
        public static TechCategory category;
        public static EggInfoData eggData;
        TechType techtype;

        // Remove static keyword from the properties
        string tooltip = EggInfoData.Instance.Tooltip;
        string displayName = EggInfoData.Instance.FriendlyName;
        string internalName = EggInfoData.Instance.InternalName;
        string spriteName = EggInfoData.Instance.Spritename;
        string resourceId = EggInfoData.Instance.ResourceId;
        string objectname = EggInfoData.Instance.ObjectName;
        [SetsRequiredMembers]
        public BasicEggPrefab(string internalName, string displayName, string tooltip, string objectname, Atlas.Sprite spriteName)
            : base(internalName, displayName, tooltip, spriteName)
        {
            


        }
    }
}