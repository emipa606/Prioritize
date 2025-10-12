using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Prioritize;

public class Designator_Priority_Thing : Designator
{
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

    public override int DraggableDimensions => 2;

    public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
    {
        foreach (IntVec3 c in cells)
        {
            if (CanDesignateCell(c).Accepted)
            {
                DesignateSingleCell(c);
            }
        }
    }

    public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
    {
        get
        {
            yield return new FloatMenuOption("Priority".Translate(),
                PriorityWindow.Open);

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

    private static void DrawMouseAttachmentReflected(Texture iconTex, string text, float angle, Vector2 offset)
    {
        MethodInfo drawMouseAttachmentMethod = AccessTools.Method(
            typeof(GenUI),
            "DrawMouseAttachment",
            new Type[] { typeof(Texture), typeof(string), typeof(float), typeof(Vector2), typeof(Rect?), typeof(Color?), typeof(bool), typeof(Color), typeof(Color?), typeof(Action<Rect>) }
        );

        // Prepare arguments, filling in default values for optional parameters
        object[] parameters = new object[] {
            iconTex,
            text,
            angle,
            offset,
            null,
            null,
            false,
            Color.white,
            null,
            null
        };

        drawMouseAttachmentMethod.Invoke(null, parameters);
    }

    public override void SelectedUpdate()
    {
        GenUI.RenderMouseoverBracket();
    }

    public override void RenderHighlight(List<IntVec3> dragCells)
    {
        Material highlightMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f)); // A simple white transparent material
        foreach (IntVec3 cell in dragCells)
        {
            Vector3 position = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
            Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, highlightMat, 0);
        }
    }

    public override void DrawMouseAttachments()
    {
        DrawMouseAttachmentReflected(icon, MainMod.SelectedPriority.ToString(), iconAngle, iconOffset);
    }
}