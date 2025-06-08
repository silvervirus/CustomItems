using Nautilus.Assets;


namespace CustomItems
{
    public class BasicEggPrefab : CustomPrefab
    {
        public static TechGroup group;
        public static TechCategory category;
        public static EggInfoData eggData;
        private TechType techtype;

        // Instance properties without `required`
        string tooltip = EggInfoData.Instance.Tooltip;
        string displayName = EggInfoData.Instance.FriendlyName;
        string internalName = EggInfoData.Instance.InternalName;
        string spriteName = EggInfoData.Instance.Spritename;
        string resourceId = EggInfoData.Instance.ResourceId;
        string objectname = EggInfoData.Instance.ObjectName;
        
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public BasicEggPrefab(string internalName, string displayName, string tooltip, string objectname, Atlas.Sprite spriteName)
            : base(internalName, displayName, tooltip, spriteName)
        {
           
        }
        
       
    }
}