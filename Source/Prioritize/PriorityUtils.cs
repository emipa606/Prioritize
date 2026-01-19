using Verse;

namespace Prioritize;

public static class PriorityUtils
{
    public static float GetPriority(Thing t)
    {
        float pr = MainMod.save.TryGetThingPriority(t, out var pri) ? pri : 0;
        if (t.Map != null && t.Position.InBounds(t.Map))
        {
            pr += PSaveData.GetPriorityMapData(t.Map).GetPriorityAt(t.Position);
        }

        if (PrioritizeMod.Instance.Settings.UseLowerAsHighPriority)
        {
            pr = -pr;
        }

        return pr;
    }
}