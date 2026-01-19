using System;
using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(GenClosest), nameof(GenClosest.ClosestThing_Global_Reachable))]
public class GenClosest_ClosestThing_Global_Reachable
{
    public static void Prefix(ref Func<Thing, float> priorityGetter)
    {
        if (!PrioritizeMod.Instance.Settings.UseUnsafePatches)
        {
            return;
        }

        var p = priorityGetter ?? delegate { return 0f; };

        priorityGetter = t => PriorityUtils.GetPriority(t) + p(t);
    }
}