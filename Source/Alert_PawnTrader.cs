using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
    public class Alert_PawnTrader : Alert
    {

        private IEnumerable<Pawn> TraderPawns
        {
            get
            {
                return PawnsFinder.AllMaps_Spawned.Where(p =>
                {
                    return (p.CanTradeNow);
                });
            }
        }

        public override string GetLabel()
        {
            if (TraderPawns.Count() > 1)
                return "PawnTraderMulti".Translate();
            return "PawnTraderSingle".Translate();
        }
        public override TaggedString GetExplanation()
        {
            TaggedString returnString = new TaggedString();
            foreach (Pawn pawn in TraderPawns)
                returnString += "    " + pawn.NameFullColored + ", " + pawn.Faction.NameColored + Environment.NewLine;

            return "PawnTraderDesc".Translate().Replace("{0}", "") + returnString;
        }

        public override AlertReport GetReport()
        {
            if (TraderPawns.Count() == 0) { return false; }
            var selectedPawn = TraderPawns.FirstOrDefault();
            var someoneSelected = false;
            foreach (Pawn p in TraderPawns)
            {
                if(Find.Selector.IsSelected(p)) {
                    someoneSelected = true;
                    continue;
                }
                if(someoneSelected && !Find.Selector.IsSelected(p))
                {
                    selectedPawn = p;
                    break;
                }
            }
            return AlertReport.CulpritIs(selectedPawn);
        }


    }
}
