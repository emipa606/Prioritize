using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(Designation), "Notify_Removing")]
public class Designation_Notify_Removing
{
    public static void Prefix(Designation __instance)
    {
        if (__instance.target == null)
        {
            return;
        }

        if (MainMod.save == null)
        {
            return;
        }

        if (__instance.target.HasThing)
        {
            MainMod.save.SetThingPriority(__instance.target.Thing, 0);
        }
    }
}