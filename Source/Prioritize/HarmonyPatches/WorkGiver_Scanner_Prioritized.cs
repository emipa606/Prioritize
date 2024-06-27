using HarmonyLib;
using RimWorld;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.Prioritized), MethodType.Getter)]
public class WorkGiver_Scanner_Prioritized
{
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}