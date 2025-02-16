using Nautilus.Assets;


namespace CustomItems
{
    public class BasicBatteryPrefab : CustomPrefab
    {
        public static TechGroup group;
        public static TechCategory category;
        public static EggInfoData eggData;
        private TechType techtype;

        // Instance properties without `required`
        string tooltip = EggInfoData.Instance.Tooltip;
        string displayName = EggInfoData.Instance.FriendlyName;
        string internalName = EggInfoData.Instance.InternalName;
       
      
        
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public BasicBatteryPrefab(string internalName, string displayName, string tooltip)
            : base(internalName, displayName, tooltip)
        {
           
        }
        
       
    }
}