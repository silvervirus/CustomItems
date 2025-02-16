using Nautilus.Assets;


namespace CustomItems
{
    public class BasicingredientsPrefab : CustomPrefab
    {
        public static TechGroup group;
        public static TechCategory category;
        public static EggInfoData eggData;
        private TechType techtype;

        // Instance properties without `required`
        string tooltip = EggInfoData.Instance.Tooltip;
        string displayName = EggInfoData.Instance.FriendlyName;
        string internalName = EggInfoData.Instance.InternalName;
        private string SpriteName = EggInfoData.Instance.Spritename;
      
        
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public BasicingredientsPrefab(string internalName, string displayName, string tooltip,Atlas.Sprite SpriteName)
            : base(internalName, displayName, tooltip, SpriteName)
        {
           
        }
        
       
    }
}