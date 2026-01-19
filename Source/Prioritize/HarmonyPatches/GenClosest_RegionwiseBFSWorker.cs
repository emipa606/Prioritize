using System;
using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(GenClosest), nameof(GenClosest.RegionwiseBFSWorker))]
public class GenClosest_RegionwiseBFSWorker
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