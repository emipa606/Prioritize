using HarmonyLib;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(Thing), nameof(Thing.Destroy))]
public class Thing_Destroy
{
    public static void Prefix(Thing __instance)
    {
        if (MainMod.save == null)
        {
            return;
        }

        MainMod.DestroyedThingId.Add(__instance.thingIDNumber);
        if (MainMod.DestroyedThingId.Count <= 50000)
        {
            return;
        }

        Log.Warning("Too many items in DestroyedThingId.");
        MainMod.RemoveThingPriorityNow();
    }
}