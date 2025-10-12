using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Prioritize;

[StaticConstructorOnStartup]
public static class MainMod
{
    public static short SelectedPriority;
    public static PSaveData save;

    public static readonly Texture2D ShowPriority = ContentFinder<Texture2D>.Get("UI/Prioritize/ShowPriority");


    public static Func<Thing, bool> ThingShowCond = PriorityShowConditions.DefaultCondition.Cond;

    public static PriorityDrawMode PriorityDraw = PriorityDrawMode.None;

    public static PriorityDrawMode ForcedDrawMode = PriorityDrawMode.None;

    public static readonly HashSet<int> DestroyedThingId = new HashSet<int>();

    public static void RemoveThingPriorityNow()
    {
        if (save == null)
        {
            return;
        }

        foreach (var pair in DestroyedThingId)
        {
            save.ThingPriority.Remove(pair);
        }

        DestroyedThingId.Clear();
    }

    public static void AdjustPriorityMouseControl()
    {
        if (Event.current.type != EventType.ScrollWheel || !Input.GetKey(KeyCode.LeftControl))
        {
            return;
        }

        SelectedPriority -= (short)(Event.current.delta.y >= 0 ? 1 : -1);
        SoundDefOf.Tick_High.PlayOneShotOnCamera();
        Event.current.Use();
    }

    public static Color GetPriorityDrawColor(bool IsCell, float pri)
    {
        var CellColorUpper = new Color(0, 0, 1); //Blue
        var CellColorDown = new Color(1, 0.5f, 0); //Orange

        var ThingColorUpper = new Color(0, 1, 0); //Green
        var ThingColorDown = new Color(1, 0, 0); //Red


        var ColorUpper = IsCell ? CellColorUpper : ThingColorUpper;
        var ColorDown = IsCell ? CellColorDown : ThingColorDown;

        var ThresholdPri = 6.25f;
        if (PrioritizeMod.instance.Settings.UseLowerAsHighPriority)
        {
            pri = -pri;
        }

        var res = Color.white;
        if (pri > 0)
        {
            res = Color.Lerp(res, ColorUpper, pri / ThresholdPri);
        }

        if (pri < 0)
        {
            res = Color.Lerp(res, ColorDown, -pri / ThresholdPri);
        }

        return res;
    }

    public static CellRect GetMapRect()
    {
        var rect = new Rect(0f, 0f, UI.screenWidth, UI.screenHeight);
        var screenLoc = new Vector2(rect.x, UI.screenHeight - rect.y);
        var screenLoc2 = new Vector2(rect.x + rect.width, UI.screenHeight - (rect.y + rect.height));
        var vector = UI.UIToMapPosition(screenLoc);
        var vector2 = UI.UIToMapPosition(screenLoc2);
        return new CellRect
        {
            minX = Mathf.FloorToInt(vector.x),
            minZ = Mathf.FloorToInt(vector2.z),
            maxX = Mathf.FloorToInt(vector2.x),
            maxZ = Mathf.FloorToInt(vector.z)
        };
    }

    public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
    {
        SetProperDrawSize();
        var x = Text.CalcSize(text).x;
        GUI.color = textColor;
        Text.Anchor = TextAnchor.UpperCenter;
        var rect = new Rect(screenPos.x - (x / 2f), screenPos.y - 3f, x, 999f);
        Widgets.Label(rect, text);
        GUI.color = Color.white;
        Text.Anchor = TextAnchor.UpperLeft;
        Text.Font = GameFont.Small;
    }

    private static void SetProperDrawSize()
    {
        if (GetMapRect().Area > 10000)
        {
            Text.Font = GameFont.Tiny;
        }
        else if (GetMapRect().Area > 5000)
        {
            Text.Font = GameFont.Small;
        }
        else
        {
            Text.Font = GameFont.Medium;
        }
    }
}