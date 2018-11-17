using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                return string.Format("PawnTrader".Translate(), "s");
            return string.Format("PawnTrader".Translate(), "");
        }
        public override string GetExplanation()
        {
            var stringBuilder = new StringBuilder();
            foreach (Pawn pawn in TraderPawns)
                stringBuilder.AppendLine("    " + pawn.Faction.Name);

            return string.Format("PawnTraderDesc".Translate(), stringBuilder.ToString());
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
