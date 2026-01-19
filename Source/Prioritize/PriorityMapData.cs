using Verse;

namespace Prioritize;

public class PriorityMapData : MapComponent
{
    private byte[] griddata;
    private int numCells;
    private ushort[] priorityGrid;

    public PriorityMapData(Map map)
        : base(map)
    {
        priorityGrid = new ushort[map.cellIndices.NumGridCells];
        for (var i = 0; i < priorityGrid.Length; i++)
        {
            priorityGrid[i] = 32768;
        }
    }

    public short GetPriorityAt(IntVec3 loc)
    {
        var returnValue = priorityGrid[map.cellIndices.CellToIndex(loc)];
        if (returnValue != 0)
        {
            return (short)(priorityGrid[map.cellIndices.CellToIndex(loc)] - 32768);
        }

        Log.ErrorOnce($"Priority grid {loc} priority is -32767, Resetting to 0..",
            "PG32767Error".GetHashCode());
        priorityGrid[map.cellIndices.CellToIndex(loc)] = 32768;

        return (short)(priorityGrid[map.cellIndices.CellToIndex(loc)] - 32768);
    }

    public void SetPriorityAt(IntVec3 loc, short pri)
    {
        priorityGrid[map.cellIndices.CellToIndex(loc)] = (ushort)(pri + 32768);
    }

    public override void ExposeData()
    {
        if (map != null)
        {
            numCells = map.cellIndices.NumGridCells;
        }

        Scribe_Values.Look(ref numCells, "numCells");
        switch (Scribe.mode)
        {
            case LoadSaveMode.Saving:
                MapExposeUtility.ExposeUshort(map, c => priorityGrid[map.cellIndices.CellToIndex(c)],
                    delegate(IntVec3 c, ushort val) { priorityGrid[map.cellIndices.CellToIndex(c)] = val; },
                    "priorityGrid");
                break;
            case LoadSaveMode.LoadingVars:
                priorityGrid = new ushort[numCells];
                DataExposeUtility.LookByteArray(ref griddata, "priorityGrid");
                DataSerializeUtility.LoadUshort(griddata, numCells,
                    delegate(int c, ushort val) { priorityGrid[c] = val; });
                griddata = null;
                break;
        }
    }
}