﻿using System;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace WeHadATrader;

public class Alert_OrbitalTrader : Alert
{
    /// <summary>
    ///     Returns ticks in human-readable time left
    /// </summary>
    /// <param name="ticks"></param>
    /// <returns></returns>
    private static string ticksToHumanTime(int ticks)
    {
        if (ticks < 2500)
        {
            return "LessThanAnHour".Translate();
        }

        var hours = Math.Ceiling((decimal)ticks / 2500);
        return hours == 1 ? "AboutAnHour".Translate() : "AmountOfHours".Translate(hours.ToString());
    }

    public override string GetLabel()
    {
        foreach (var map in Find.Maps)
        {
            if (map.passingShipManager.passingShips.Count > 1)
            {
                return "OrbitalTraderMulti".Translate();
            }
        }

        return "OrbitalTraderSingle".Translate();
    }

    public override TaggedString GetExplanation()
    {
        var stringBuilder = new StringBuilder();
        foreach (var map in Find.Maps)
        {
            foreach (var ship in map.passingShipManager.passingShips)
            {
                stringBuilder.AppendLine(ship.FullTitle);
                stringBuilder.AppendLine("PawnTraderLeavesIn".Translate(ticksToHumanTime(ship.ticksUntilDeparture)));
            }
        }

        return string.Format("OrbitalTraderDesc".Translate(), stringBuilder);
    }

    public override AlertReport GetReport()
    {
        foreach (var map in Find.Maps)
        {
            if (map.passingShipManager.passingShips.Count <= 0)
            {
                continue;
            }

            var console = map.listerBuildings.AllBuildingsColonistOfClass<Building_CommsConsole>()
                .FirstOrDefault();
            if (console != null)
            {
                return AlertReport.CulpritIs(console);
            }
        }

        return false;
    }
}