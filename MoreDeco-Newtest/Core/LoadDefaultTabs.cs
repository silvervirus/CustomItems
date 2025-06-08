using System;
using Nautilus.Handlers;

namespace CustomItems.Core;

public class LoadDefaultTabs
{
     public static void Maketabs()
        {
            Console.Write("Loading Tabs");
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "CIS", "CustomItems",
                RamuneLib.Utils.ImageUtils.GetSprite("CIS"), new string[]
                {


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle", "Poster",
                SpriteManager.Get(TechType.PosterKitty), new string[]
                {

                    "CIS"
                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "PoolNoodle2", "Poster",
                SpriteManager.Get(TechType.PosterAurora), new string[]
                {

                    "CIS"
                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle1", "Food",
                SpriteManager.Get(TechType.NutrientBlock), new string[]
                {

                    "CIS"

                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Noodle2", "Drink",
                SpriteManager.Get(TechType.FilteredWater), new string[]
                {

                    "CIS"

                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "OX", "Oxygen Tanks",
                SpriteManager.Get(TechType.Tank), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "FA", "FirstAid",
                SpriteManager.Get(TechType.FirstAidKit), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "EQP", "Equipment",
                SpriteManager.Get(TechType.DoubleTank), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "CF3", "CustomFoods3",
                RamuneLib.Utils.ImageUtils.GetSprite("CF3mod"), new string[]
                {

                    "CIS"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "FIDT", "Food Ingredients",
                RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "MIDT", "More Ingredients",
                RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "IDT", "Ingredients",
                RamuneLib.Utils.ImageUtils.GetSprite("Ingredients"), new string[]
                {

                    "CIS",
                    "CF3"


                });

            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "ETACO", "Mexican",
                RamuneLib.Utils.ImageUtils.GetSprite("taco"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Rollerd", "American",
                RamuneLib.Utils.ImageUtils.GetSprite("Peeperburger"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Beka", "Japanese",
                RamuneLib.Utils.ImageUtils.GetSprite("rice"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Drinks", "Drinks",
                RamuneLib.Utils.ImageUtils.GetSprite("Tea"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "ELJuice", "Juice",
                RamuneLib.Utils.ImageUtils.GetSprite("juice1"), new string[]
                {

                    "CIS",
                    "CF3"


                });
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "Yum", "Sweets",
                RamuneLib.Utils.ImageUtils.GetSprite("cake1"), new string[]
                {

                    "CIS",
                    "CF3"


                });

        }
}