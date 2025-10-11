using UnityEngine;
using Verse;

namespace Prioritize
{
    public static class FloatMenu_SelectPriority
    {
        private static string editBuffer;

        public static void Draw(Rect inRect)
        {
            if (Widgets.ButtonText(new Rect(0, 5, 30f, 30), "-1"))
            {
                MainMod.SelectedPriority -= (short)GenUI.CurrentAdjustmentMultiplier();
                editBuffer = ((int)MainMod.SelectedPriority).ToStringCached();
            }

            if (Widgets.ButtonText(new Rect(210, 5, 30f, 30), "+1"))
            {
                MainMod.SelectedPriority += (short)GenUI.CurrentAdjustmentMultiplier();
                editBuffer = ((int)MainMod.SelectedPriority).ToStringCached();
            }

            int pri = MainMod.SelectedPriority;

            Widgets.TextFieldNumeric(new Rect(45, 5, 150, 30), ref pri, ref editBuffer, -32766, 32768);

            MainMod.SelectedPriority = (short)pri;
            var offset = new Rect(0f, 50f, 20f, 20f);
            var spacing = 3f + offset.width;

            for (var i = -5; i <= 5; i++)
            {
                if (Widgets.ButtonText(offset, i.ToString()))
                {
                    MainMod.SelectedPriority = (short)i;
                    editBuffer = i.ToStringCached();
                }

                offset.x += spacing;
            }
        }
    }
}