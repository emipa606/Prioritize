using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Prioritize.HarmonyPatches;

[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
public class PlaySettings_DoPlaySettingsGlobalControls
{
    public static void Postfix(WidgetRow row, bool worldView)
    {
        if (worldView)
        {
            return;
        }

        if (!row.ButtonIcon(MainMod.ShowPriority, "P_ShowPriority".Translate()))
        {
            return;
        }

        var listOptions = new List<FloatMenuOption>
        {
            new FloatMenuOption("None".Translate(),
                delegate { MainMod.ForcedDrawMode = PriorityDrawMode.None; }),
            new FloatMenuOption("P_Cell".Translate(),
                delegate { MainMod.ForcedDrawMode = PriorityDrawMode.Cell; }),
            new FloatMenuOption("P_Thing".Translate(),
                delegate { MainMod.ForcedDrawMode = PriorityDrawMode.Thing; })
        };
        Find.WindowStack.Add(new FloatMenu(listOptions));
    }
}