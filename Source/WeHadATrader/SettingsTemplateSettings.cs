using Verse;

namespace WeHadATrader;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class WeHadATraderSettings : ModSettings
{
    public bool IgnoreGuests;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref IgnoreGuests, "IgnoreGuests");
    }
}