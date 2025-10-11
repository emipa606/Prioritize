using System;
using System.Reflection;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace Prioritize;

public class Designator_SetPriority : Designator
{
    public Designator_SetPriority()
    {
        icon = MainMod.ShowPriority;
        defaultLabel = "P_SetPriority".Translate();
        defaultDesc = "P_SetPriorityDesc".Translate();
    }

    public override void ProcessInput(Event ev)
    {
        if (!CheckCanInteract())
        {
            return;
        }

        if (Find.DesignatorManager.SelectedDesignator is Designator_Priority_Cell ||
            Find.DesignatorManager.SelectedDesignator is Designator_Priority_Thing)
        {
            MainMod.SelectedPriority = 0;
        }
        else
        {
            PriorityWindow.Open();
        }
    }

    public override AcceptanceReport CanDesignateCell(IntVec3 loc)
    {
        return false;
    }
}