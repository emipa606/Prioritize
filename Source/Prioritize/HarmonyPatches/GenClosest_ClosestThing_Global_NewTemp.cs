using System;
using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(GenClosest), nameof(GenClosest.ClosestThing_Global_NewTemp))]
public class GenClosest_ClosestThing_Global_NewTemp
{
    public static void Prefix(ref Func<Thing, float> priorityGetter)
    {
        if (!PrioritizeMod.instance.Settings.UseUnsafePatches)
        {
            return;
        }

        var p = priorityGetter;
        if (p == null)
        {
            p = delegate { return 0f; };
        }

        priorityGetter = t => PriorityUtils.GetPriority(t) + p(t);
    }
}