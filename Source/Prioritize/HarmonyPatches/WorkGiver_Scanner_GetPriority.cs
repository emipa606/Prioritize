using HarmonyLib;
using RimWorld;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.GetPriority), typeof(Pawn), typeof(TargetInfo))]
public class WorkGiver_Scanner_GetPriority
{
    public static void Postfix(Pawn pawn, TargetInfo t, ref float __result)
    {
        //if (__result < 0)
        //{
        //    return;
        //}

        if (!pawn.IsColonist)
        {
            return;
        }

        var m = pawn.Map;
        if (m == null)
        {
            m = t.Map;
        }

        var priority = 0f;

        if (t.HasThing)
        {
            priority += MainMod.save.TryGetThingPriority(t.Thing, out var pri) ? pri + 0.1f : 0;
        }

        priority += MainMod.save.GetPriorityMapData(m).GetPriorityAt(t.Cell);

        if (PrioritizeMod.instance.Settings.UseLowerAsHighPriority)
        {
            __result -= priority;
        }
        else
        {
            __result += priority;
        }
    }
}