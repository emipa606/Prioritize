using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Prioritize.HarmonyPatches
{
    [HarmonyPatch(typeof(DesignatorManager), "ProcessInputEvents")]
    public static class DesignatorManager_ProcessInputEvents_Patch
    {
        private static bool isDragging = false;
        private static IntVec3 startDragCell = IntVec3.Invalid;

        private static bool Prefix(DesignatorManager __instance)
        {
            Designator selectedDesignator = __instance.SelectedDesignator;
            if (selectedDesignator == null || (selectedDesignator.GetType() != typeof(Designator_Priority_Thing) && selectedDesignator.GetType() != typeof(Designator_Priority_Cell)))
            {
                return true; // Let original method run for other designators
            }

            // Handle drag for our designators
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                isDragging = true;
                startDragCell = UI.MouseCell();
                Event.current.Use();
                return false; // Prevent original method from running
            }
            else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                if (isDragging && startDragCell.IsValid)
                {
                    IntVec3 endDragCell = UI.MouseCell();
                    CellRect rect = CellRect.FromLimits(startDragCell, endDragCell);
                    foreach (IntVec3 c in rect)
                    {
                        if (selectedDesignator.CanDesignateCell(c).Accepted)
                        {
                            selectedDesignator.DesignateSingleCell(c);
                        }
                    }
                    isDragging = false;
                    startDragCell = IntVec3.Invalid;
                    Event.current.Use();
                }
                return false; // Prevent original method from running
            }
            
            return true; // Let original method run for other events (e.g., right-click, other mouse buttons)
        }
    }
}