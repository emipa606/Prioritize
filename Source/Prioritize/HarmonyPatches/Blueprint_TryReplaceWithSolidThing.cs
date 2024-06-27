using HarmonyLib;
using RimWorld;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(Blueprint), nameof(Blueprint.TryReplaceWithSolidThing))]
public class Blueprint_TryReplaceWithSolidThing
{
    public static void Postfix(Blueprint __instance, Thing createdThing)
    {
        if (MainMod.save.TryGetThingPriority(__instance, out var pri))
        {
            MainMod.save.SetThingPriority(createdThing, pri);
        }
    }
}