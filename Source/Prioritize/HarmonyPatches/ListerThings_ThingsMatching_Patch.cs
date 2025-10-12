using HarmonyLib;
using Verse;
using System.Collections.Generic;

namespace Prioritize.HarmonyPatches
{
    [HarmonyPatch(typeof(ListerThings), "ThingsMatching")]
    public static class ListerThings_ThingsMatching_Patch
    {
        private static bool Prefix(ThingRequest req, ref List<Thing> __result)
        {
            if (req.group == ThingRequestGroup.Undefined)
            {
                __result = new List<Thing>();
                return false; 
            }
            return true;
        }
    }
}