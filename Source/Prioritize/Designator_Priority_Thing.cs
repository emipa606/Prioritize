using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Prioritize;

public class Designator_Priority_Thing : Designator
{
    // ReSharper disable once MemberCanBeProtected.Global
    public Designator_Priority_Thing()
    {
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        icon = ContentFinder<Texture2D>.Get("UI/Prioritize/ThingPri");
        useMouseIcon = true;
        defaultLabel = "P_DesignatorThingLabel".Translate();
        defaultDesc = "P_DesignatorThingDesc".Translate();
        soundSucceeded = SoundDefOf.Designate_PlanAdd;
    }

    public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Orders;

    public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
    {
        get
        {
            yield return new FloatMenuOption("Priority".Translate(),
                delegate { Find.WindowStack.Add(new Dialog_SelectPriority()); });

            yield return new FloatMenuOption("Options".Translate(),
                PriorityShowConditions.ShowConditionsMenuBox, MenuOptionPriority.High);
        }
    }

    protected override DesignationDef Designation => PDefOf.Priortize_Thing;

    public override AcceptanceReport CanDesignateCell(IntVec3 loc)
    {
        if (loc.Fogged(Map) || !loc.InBounds(Map))
        {
            return false;
        }

        foreach (var t in Map.thingGrid.ThingsAt(loc))
        {
            if (CanDesignateThing(t).Accepted)
            {
                return true;
            }
        }

        return false;
    }

    public override AcceptanceReport CanDesignateThing(Thing t)
    {
        return MainMod.ThingShowCond(t) && (t.Faction == null || t.Faction.IsPlayer);
    }

    public override void DesignateSingleCell(IntVec3 c)
    {
        foreach (var t in Map.thingGrid.ThingsAt(c))
        {
            if (CanDesignateThing(t).Accepted)
            {
                DesignateThing(t);
            }
        }
    }

    public override void DesignateThing(Thing t)
    {
        MainMod.save.SetThingPriority(t, MainMod.SelectedPriority);
    }

    public override void SelectedUpdate()
    {
        GenUI.RenderMouseoverBracket();
    }

    public override void RenderHighlight(List<IntVec3> dragCells)
    {
        DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
    }

    public override void DrawMouseAttachments()
    {
        GenUI.DrawMouseAttachment(icon, MainMod.SelectedPriority.ToString(), iconAngle, iconOffset);
    }
}