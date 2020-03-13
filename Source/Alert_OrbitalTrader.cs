using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
    public class Alert_OrbitalTrader : Alert
    {
        /// <summary>
        /// Returns ticks in human readable time left
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private string ticksToHumanTime(int ticks)
        {
            if (ticks < 2500)
                return "less than an hour";
            var hours = Math.Ceiling((decimal)(ticks / 2500));
            if (hours == 1) return "about an hour";
            return hours + " hours";
        }

        public override string GetLabel()
        {
            foreach (Map map in Find.Maps)
                if (map.passingShipManager.passingShips.Count > 1)
                    return string.Format("OrbitalTrader".Translate(), "s");
            return string.Format("OrbitalTrader".Translate(), "");
        }

        public override TaggedString GetExplanation()
        {
            var stringBuilder = new StringBuilder();
            foreach (Map map in Find.Maps)
                foreach (PassingShip ship in map.passingShipManager.passingShips)
                {
                    stringBuilder.AppendLine("    " + ship.FullTitle);
                    stringBuilder.AppendLine("    Leaves in " + ticksToHumanTime(ship.ticksUntilDeparture));
                }
            return string.Format("OrbitalTraderDesc".Translate(), stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            foreach (Map map in Find.Maps)
            {
                if (map.passingShipManager.passingShips.Count > 0)
                {
                    Building_CommsConsole console = map.listerBuildings.AllBuildingsColonistOfClass<Building_CommsConsole>().FirstOrDefault();
                    if (console != null)
                        return AlertReport.CulpritIs(console);
                }
            }
            return false;
        }
    }
}
