using HarmonyLib;
using RimWorld;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.GetPriority), typeof(Pawn), typeof(TargetInfo))]
public class WorkGiver_Scanner_GetPriority
{
    public static void Postfix(Pawn pawn, TargetInfo t, ref float __result, WorkGiver_Scanner __instance)
    {
        if (!pawn.IsPlayerControlled)
        {
            return;
        }

        var m = pawn.Map ?? t.Map;

        var priority = 0f;

        if (t.HasThing)
        {
            priority += MainMod.save.TryGetThingPriority(t.Thing, out var pri) ? pri + 0.1f : 0;
        }

        priority += PSaveData.GetPriorityMapData(m).GetPriorityAt(t.Cell);

        if (PrioritizeMod.Instance.Settings.UseLowerAsHighPriority)
        {
            __result -= priority;
        }
        else
        {
            __result += priority;
        }

        Log.Message($"Overriding priority for {__instance} for {pawn} to {__result}");
    }
}