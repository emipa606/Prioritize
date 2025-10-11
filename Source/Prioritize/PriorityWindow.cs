using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace Prioritize
{
    [StaticConstructorOnStartup]
    public static class PriorityWindow
    {
        private static string editBuffer;
        private static bool open;
        private static Rect windowRect;

        static PriorityWindow()
        {
            windowRect = new Rect(((float)UI.screenWidth - 300) / 2f, ((float)UI.screenHeight - 110) / 2f, 300, 110);
        }

        public static void Open()
        {
            open = true;
        }

        public static void Close()
        {
            open = false;
        }

        public static void Draw()
        {
            if (!open)
            {
                return;
            }

            MethodInfo immediateWindowMethod = typeof(WindowStack).GetMethod(
                "ImmediateWindow",
                new Type[] { typeof(int), typeof(Rect), typeof(WindowLayer), typeof(Action), typeof(bool), typeof(bool), typeof(float), typeof(Action), typeof(bool) }
            );

            Action doWindowContents = () =>
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

                if (Widgets.ButtonText(new Rect(windowRect.width / 2 - 50, windowRect.height - 35, 100, 30), "Close"))
                {
                    Close();
                }
            };

            immediateWindowMethod.Invoke(Find.WindowStack, new object[] { 123456, windowRect, WindowLayer.Dialog, doWindowContents, true, true, 1f, null, false });
        }
    }
}