using Nautilus.Assets;

namespace CustomItems.Util;

public class UtraBasePrefab
{
    public class UnifiedPrefabsCombined : CustomPrefab
    {
        public static TechGroup group;
        public static TechCategory category;
        public static EggData.EggInfoData eggData;
        private TechType techtype;

        // Instance property initialization
        private readonly string tooltip = EggData.EggInfoData.Instance.Tooltip;
        private readonly string displayName = EggData.EggInfoData.Instance.FriendlyName;
        private readonly string internalName = EggData.EggInfoData.Instance.InternalName;
        private readonly string spriteName = EggData.EggInfoData.Instance.Spritename;
        private readonly string resourceId = EggData.EggInfoData.Instance.ResourceId;
        private readonly string objectName = EggData.EggInfoData.Instance.ObjectName;

        // Base constructor for battery/ingredient type prefabs
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public UnifiedPrefabsCombined(string internalName, string displayName, string tooltip)
            : base(internalName, displayName, tooltip)
        {
        }

        // Overloaded constructor for prefabs that use a Sprite (e.g., ingredients or eggs)
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public UnifiedPrefabsCombined(string internalName, string displayName, string tooltip, Atlas.Sprite sprite)
            : base(internalName, displayName, tooltip, sprite)
        {
        }
    
        // Overloaded constructor that includes objectName specifically for egg-prefabs
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public UnifiedPrefabsCombined(string internalName, string displayName, string tooltip, string objectName, Atlas.Sprite sprite)
            : base(internalName, displayName, tooltip, sprite)
        {
            this.objectName = objectName;
        }
    }
}