using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Prioritize;

public class PriorityGameComponent : GameComponent
{
    public PriorityGameComponent(Game game)
    {
        MainMod.save = game.GetComponent<PSaveData>();
    }

    public override void GameComponentTick()
    {
        base.GameComponentTick();
        MainMod.RemoveThingPriorityNow();
    }

    public override void GameComponentOnGUI()
    {
        base.GameComponentOnGUI();
        if (Find.CurrentMap == null || WorldRendererUtility.WorldRendered)
        {
            return;
        }

        if (Find.DesignatorManager == null)
        {
            return;
        }

        switch (Find.DesignatorManager.SelectedDesignator)
        {
            //Logger.Message(Find.DesignatorManager.SelectedDesignator.GetType().ToString());
            case Designator_Priority_Cell:
                MainMod.PriorityDraw = PriorityDrawMode.Cell;
                break;
            case Designator_Priority_Thing:
                MainMod.PriorityDraw = PriorityDrawMode.Thing;
                break;
            default:
                MainMod.PriorityDraw = MainMod.ForcedDrawMode;
                break;
        }

        var map = Find.CurrentMap;

        if (MainMod.PriorityDraw == PriorityDrawMode.None)
        {
            return;
        }

        MainMod.AdjustPriorityMouseControl();

        var rect = MainMod.GetMapRect();
        if (rect.Area >= 10000)
        {
            return;
        }

        foreach (var intVec in rect)
        {
            if (!intVec.InBounds(map))
            {
                continue;
            }

            if (MainMod.PriorityDraw == PriorityDrawMode.Cell)
            {
                Vector3 v = GenMapUI.LabelDrawPosFor(intVec);
                int p = PSaveData.GetPriorityMapData(map).GetPriorityAt(intVec);
                if (p != 0)
                {
                    MainMod.DrawThingLabel(v, p.ToString(), MainMod.GetPriorityDrawColor(true, p));
                }

                continue;
            }

            if (MainMod.PriorityDraw != PriorityDrawMode.Thing)
            {
                continue;
            }

            var th = intVec.GetThingList(map);
            foreach (var thing in th)
            {
                if (MainMod.ThingShowCond(thing) && MainMod.save.TryGetThingPriority(thing, out var pri))
                {
                    MainMod.DrawThingLabel(GenMapUI.LabelDrawPosFor(thing, 0f), pri.ToString(),
                        MainMod.GetPriorityDrawColor(false, pri));
                }
            }
        }
    }
}