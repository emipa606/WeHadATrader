using RimWorld.Planet;
using System;
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
                return PawnsFinder.AllMaps_Spawned.Where<Pawn>((Func<Pawn,bool>)(p =>
                {
                    return (p.trader != null && p.trader.CanTradeNow);
                }));
            }
        }
        
        public override string GetLabel()
        {
            if (this.TraderPawns.Count<Pawn>() > 1)
                return string.Format("PawnTrader".Translate(), "s");
            return string.Format("PawnTrader".Translate(), "");
        }
        public override string GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Pawn pawn in this.TraderPawns)
                stringBuilder.AppendLine("    " + pawn.Faction.Name);

            return string.Format("PawnTraderDesc".Translate(), stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            Pawn pawn = this.TraderPawns.FirstOrDefault();
            if (pawn == null)
                return false;
            return AlertReport.CulpritIs((GlobalTargetInfo)((Thing)pawn));
        }

        
    }
}
