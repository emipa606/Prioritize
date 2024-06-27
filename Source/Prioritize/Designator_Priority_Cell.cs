using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Prioritize;

public class Designator_Priority_Cell : Designator_Priority_Thing
{
    public Designator_Priority_Cell()
    {
        defaultLabel = "P_DesignatorCellLabel".Translate();
        defaultDesc = "P_DesignatorCellDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("UI/Prioritize/CellPri");
    }

    public override bool DragDrawMeasurements => true;
    protected override DesignationDef Designation => PDefOf.Priortize_Cell;

    public override AcceptanceReport CanDesignateCell(IntVec3 loc)
    {
        return loc.InBounds(Map);
    }

    public override void DesignateSingleCell(IntVec3 c)
    {
        MainMod.save.GetPriorityMapData(Map).SetPriorityAt(c, MainMod.SelectedPriority);
    }

    public override void RenderHighlight(List<IntVec3> dragCells)
    {
        DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
    }
}