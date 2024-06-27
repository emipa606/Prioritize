using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(Game), nameof(Game.FinalizeInit))]
public class Game_FinalizeInit
{
    public static void Prefix()
    {
        if (MainMod.save == null)
        {
            Log.Message(
                "FinalizeInit called but no Prioritize mod save loaded, Probably new game start, or bug. (Should be harmless message)");
            MainMod.save = new PSaveData(Current.Game);
            return;
        }

        MainMod.save.ClearUnusedThingPriority();
    }
}