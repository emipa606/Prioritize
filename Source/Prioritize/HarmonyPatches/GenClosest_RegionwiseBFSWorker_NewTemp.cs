using System;
using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(GenClosest), nameof(GenClosest.RegionwiseBFSWorker))]
public class GenClosest_RegionwiseBFSWorker_NewTemp
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