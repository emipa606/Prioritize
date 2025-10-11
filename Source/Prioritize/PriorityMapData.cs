using System;
using Verse;
using HarmonyLib;

namespace Prioritize;

public class PriorityMapData : MapComponent
{
    private byte[] griddata;
    private int numCells;
    private ushort[] priorityGrid;

    private static int GetCellIndex(Map map, IntVec3 loc)
    {
        var cellIndices = AccessTools.Field(typeof(Map), "cellIndices").GetValue(map);
        var cellToIndexMethod = AccessTools.Method(cellIndices.GetType(), "CellToIndex", new Type[] { typeof(IntVec3) });
        return (int)cellToIndexMethod.Invoke(cellIndices, new object[] { loc });
    }

    public PriorityMapData(Map map)
        : base(map)
    {
        priorityGrid = new ushort[map.Size.x * map.Size.z];
        for (var i = 0; i < priorityGrid.Length; i++)
        {
            priorityGrid[i] = 32768;
        }
    }

    public short GetPriorityAt(IntVec3 loc)
    {
        var retval = priorityGrid[GetCellIndex(map, loc)];
        if (retval != 0)
        {
            return (short)(priorityGrid[GetCellIndex(map, loc)] - 32768);
        }

        Log.ErrorOnce($"Priority grid {loc} priority is -32767, Resetting to 0..",
            "PG32767Error".GetHashCode());
        priorityGrid[GetCellIndex(map, loc)] = 32768;

        return (short)(priorityGrid[GetCellIndex(map, loc)] - 32768);
    }

    public void SetPriorityAt(IntVec3 loc, short pri)
    {
        priorityGrid[GetCellIndex(map, loc)] = (ushort)(pri + 32768);
    }

    public override void ExposeData()
    {
        if (map != null)
        {
            numCells = map.Size.x * map.Size.z;
        }

        Scribe_Values.Look(ref numCells, "numCells");
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            MapExposeUtility.ExposeUshort(map, c => priorityGrid[GetCellIndex(map, c)],
                delegate(IntVec3 c, ushort val)
                {
                    priorityGrid[GetCellIndex(map, c)] = val;
                }, "priorityGrid");
        }
        else if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            priorityGrid = new ushort[numCells];
            PrioritizeDataExposeUtility.LookByteArray(ref griddata, "priorityGrid");
            DataSerializeUtility.LoadUshort(griddata, numCells, delegate(int c, ushort val) { priorityGrid[c] = val; });
            griddata = null;
        }
    }

    public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter,
        string label)
    {
        byte[] arr = null;
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            arr = MapSerializeUtility.SerializeUshort(map, shortReader);
        }

        PrioritizeDataExposeUtility.LookByteArray(ref arr, label);
        if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            MapSerializeUtility.LoadUshort(arr, map, shortWriter);
        }
    }
}