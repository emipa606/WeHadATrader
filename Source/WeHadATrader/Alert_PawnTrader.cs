using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld;

public class Alert_PawnTrader : Alert
{
    private List<Building> TraderShips
    {
        get
        {
            var ships = new List<Building>();
            try
            {
                var buildings = Find.CurrentMap?.listerBuildings?.allBuildingsNonColonist;
                if (buildings != null)
                {
                    foreach (var building in buildings)
                    {
                        if (building.def.defName == "TraderShipsShip")
                        {
                            ships.Add(building);
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return ships;
        }
    }

    private IEnumerable<Pawn> TraderPawns
    {
        get { return PawnsFinder.AllMaps_Spawned.Where(p => p.CanTradeNow); }
    }

    public override string GetLabel()
    {
        return TraderPawns.Count() + TraderShips.Count > 1
            ? "PawnTraderMulti".Translate()
            : "PawnTraderSingle".Translate();
    }

    public override TaggedString GetExplanation()
    {
        var listOfTraders = new List<TaggedString>();
        foreach (var pawn in TraderPawns)
        {
            listOfTraders.Add(pawn.NameFullColored + ", " + pawn.Faction.NameColored);
        }

        foreach (var buildning in TraderShips)
        {
            listOfTraders.Add("Shipfrom".Translate() + buildning.Label + Environment.NewLine +
                              buildning.GetInspectString());
        }

        TaggedString returnString = string.Join(Environment.NewLine + Environment.NewLine, listOfTraders);
        return "PawnTraderDesc".Translate().Replace("{0}", "") + returnString;
    }

    public override AlertReport GetReport()
    {
        if (!TraderPawns.Any() && !TraderShips.Any())
        {
            return false;
        }

        var targetList = new List<Thing>();
        foreach (var trader in TraderPawns)
        {
            targetList.Add(trader);
        }

        foreach (var ship in TraderShips)
        {
            targetList.Add(ship);
        }

        return AlertReport.CulpritsAre(targetList);
    }
}