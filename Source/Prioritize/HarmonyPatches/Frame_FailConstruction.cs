using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(Frame), nameof(Frame.FailConstruction))]
public class Frame_FailConstruction
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var patchphase = 0;
        foreach (var inst in instructions)
        {
            yield return inst;
            if (patchphase == 1 && inst.opcode == OpCodes.Pop)
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldloc_1);
                yield return new CodeInstruction(OpCodes.Call,
                    typeof(Frame_FailConstruction).GetMethod(nameof(FixPriority)));
                patchphase = 2;
            }

            if (inst.operand.Equals( typeof(GenSpawn).GetMethod(nameof(GenSpawn.Spawn),
                [
                    typeof(Thing), typeof(IntVec3), typeof(Map), typeof(Rot4), typeof(WipeMode), typeof(bool)
                ])))
            {
                patchphase = 1;
            }
        }
    }

    public static void FixPriority(Thing fromFrame, Thing toBlueprint)
    {
        if (MainMod.save.TryGetThingPriority(fromFrame, out var pri))
        {
            MainMod.save.SetThingPriority(toBlueprint, pri);
        }
    }
}