using Nautilus.Handlers;

namespace CustomItems.Util;

public class Pings
{
    public static PingType BasePing { get; private set; }
    public static PingType Flag1Ping { get; private set; }
    public static PingType Flag2Ping { get; private set; }
    public static PingType Flag3Ping { get; private set; }
    public static PingType Flag4Ping { get; private set; }
    public static PingType Flag5Ping { get; private set; }
    public static PingType Flag6Ping { get; private set; }
    public static PingType Flag7Ping { get; private set; }
    public static PingType Alien1Ping { get; private set; }
    public static PingType Alien2Ping { get; private set; }
    public static PingType Alien3Ping { get; private set; }
    public static PingType Alien4Ping { get; private set; }
    public static PingType Alien5Ping { get; private set; }
    public static PingType Alien6Ping { get; private set; }
    public static PingType Alien7Ping { get; private set; }
    public static PingType EggPing { get; private set; }
    public static PingType DrillPing { get; private set; }
    public static PingType CreaturePing { get; private set; }
    public static PingType BaseLargePing { get; private set; }
    public static PingType BeaconPing { get; private set; }
    public static PingType Signalping { get; private set; }
    public static PingType testping { get; private set; }
    public static PingType Ship { get; private set; }
    public static PingType Check { get; private set; }
    public static PingType Check2 { get; private set; }
    public static PingType Creepvine { get; private set; }
    public static PingType Death { get; private set; }
    public static PingType Fire { get; private set; }
    public static PingType TriA { get; private set; }
    public static PingType Epoint { get; private set; }

    public static PingType Cross { get; private set; }

    public static PingType Chest { get; private set; }

    public static PingType Acube { get; private set; }

    public static PingType LifeSupport { get; private set; }

    public static PingType Arrow { get; private set; }

    public static PingType Power { get; private set; }
    public static string label { get; private set; }

    public static PingType Important { get; private set; }



    public static PingInstance ping { get; set; }

    public bool flag { get; private set; }
    public static void LoadPings()
    {

   

    BaseLargePing = EnumHandler.AddEntry<PingType>("Customping1")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("BLR"));

    BasePing = EnumHandler.AddEntry<PingType>("Customping5")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("MPR"));
    Flag1Ping = EnumHandler.AddEntry<PingType>("Customping6")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag1"));
    Flag2Ping = EnumHandler.AddEntry<PingType>("Customping7")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag2"));
    Flag3Ping = EnumHandler.AddEntry<PingType>("Customping8")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag3"));
    Flag4Ping = EnumHandler.AddEntry<PingType>("Customping9")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag4"));
    Flag5Ping = EnumHandler.AddEntry<PingType>("Customping10")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag5"));
    Flag6Ping = EnumHandler.AddEntry<PingType>("Customping11")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag6"));
    Flag7Ping = EnumHandler.AddEntry<PingType>("Customping12")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Flag7"));
    Alien1Ping = EnumHandler.AddEntry<PingType>("Customping16")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("R"));
    Alien2Ping = EnumHandler.AddEntry<PingType>("Customping17")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("P"));
    Alien3Ping = EnumHandler.AddEntry<PingType>("Customping18")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("A"));
    Alien4Ping = EnumHandler.AddEntry<PingType>("Customping19")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("T"));
    Alien5Ping = EnumHandler.AddEntry<PingType>("Customping20")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("U"));
    Alien6Ping = EnumHandler.AddEntry<PingType>("Customping21")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("EAS"));
    EggPing = EnumHandler.AddEntry<PingType>("Customping13")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("EGG"));
    DrillPing = EnumHandler.AddEntry<PingType>("Customping15")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("DR"));
    CreaturePing = EnumHandler.AddEntry<PingType>("Customping14")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("DR"));
    BeaconPing = EnumHandler.AddEntry<PingType>("Customping22")
    .WithIcon(SpriteManager.Get(SpriteManager.Group.Pings, PingType.Beacon.ToString()));
    Signalping = EnumHandler.AddEntry<PingType>("Customping3")
    .WithIcon(SpriteManager.Get(SpriteManager.Group.Pings, PingType.Signal.ToString()));
    testping = EnumHandler.AddEntry<PingType>("Customping4")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("BVping"));
    Ship = EnumHandler.AddEntry<PingType>("Ship")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("AS"));
    Check = EnumHandler.AddEntry<PingType>("Check")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("MMRO"));
    Check2 = EnumHandler.AddEntry<PingType>("Check2")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("CHK"));
    Creepvine = EnumHandler.AddEntry<PingType>("CreepLocation")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("CR"));
    Death = EnumHandler.AddEntry<PingType>("Dangerous")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("MMD"));
    Fire = EnumHandler.AddEntry<PingType>("Fire Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("WF"));
    TriA = EnumHandler.AddEntry<PingType>("Marked Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("SWO"));
    Epoint = EnumHandler.AddEntry<PingType>("Caution Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("PWW"));
    Cross = EnumHandler.AddEntry<PingType>("Heal Item Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("SP"));
    Chest = EnumHandler.AddEntry<PingType>("Chest Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("ExtraChests"));
    Acube = EnumHandler.AddEntry<PingType>("Alien Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Ion"));
    LifeSupport = EnumHandler.AddEntry<PingType>("breathable Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("IconLS"));
    Arrow = EnumHandler.AddEntry<PingType>("Thisway")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("IconArrow"));
    Power = EnumHandler.AddEntry<PingType>("Power")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("IP"));
    Important = EnumHandler.AddEntry<PingType>("Important Location")
    .WithIcon(RamuneLib.Utils.ImageUtils.GetSprite("Extrapoint"));
}

}