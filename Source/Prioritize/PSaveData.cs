using System.Collections.Generic;
using Verse;

namespace Prioritize;

public class PSaveData : GameComponent
{
    public Dictionary<int, int> ThingPriority = new();

    public PSaveData()
    {
    }

    public PSaveData(Game game)
    {
    }

    public bool TryGetThingPriority(Thing t, out int pri)
    {
        if (t != null)
        {
            return ThingPriority.TryGetValue(t.thingIDNumber, out pri);
        }

        Log.ErrorOnce("TryGetThingPriority called with null Thing.", "P_TGTP".GetHashCode());
        pri = 0;
        return false;
    }

    public void SetThingPriority(Thing t, int p)
    {
        if (t == null)
        {
            Log.ErrorOnce("SetThingPriority called with null Thing.", "P_STP".GetHashCode());
            return;
        }

        if (ThingPriority.ContainsKey(t.thingIDNumber))
        {
            if (p == 0)
            {
                ThingPriority.Remove(t.thingIDNumber);
                return;
            }

            ThingPriority[t.thingIDNumber] = p;
        }
        else if (p == 0)
        {
        }
        else
        {
            ThingPriority.Add(t.thingIDNumber, p);
        }
    }

    public static PriorityMapData GetPriorityMapData(Map m)
    {
        if (m != null)
        {
            return m.GetComponent<PriorityMapData>();
        }

        Log.Error("GetOrCreatePriorityMapData called with null Map.");
        return null;
    }

    public void ClearUnusedThingPriority()
    {
        var newThingPri = new Dictionary<int, int>();
        foreach (var map in Find.Maps)
        {
            var things = map.spawnedThings;
            foreach (var thing in things)
            {
                if (ThingPriority.TryGetValue(thing.thingIDNumber, out var v))
                {
                    newThingPri.Add(thing.thingIDNumber, v);
                }
            }
        }

        ThingPriority = newThingPri;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref ThingPriority, "thingPriority", LookMode.Value, LookMode.Value);
    }
}