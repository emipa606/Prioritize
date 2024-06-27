using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Prioritize;

public class Workgiver_UniversalConstruct : WorkGiver_ConstructDeliverResources
{
    private static readonly List<WorkGiver_Scanner> CheckList = [];

    public Workgiver_UniversalConstruct()
    {
        //ConstructFinishFrames
        //ConstructDeliverResourcesToFrames
        //ConstructDeliverResourcesToBlueprints
        CheckList.Add(DefDatabase<WorkGiverDef>.GetNamed("ConstructFinishFrames").Worker as WorkGiver_Scanner);
        CheckList.Add(
            DefDatabase<WorkGiverDef>.GetNamed("ConstructDeliverResourcesToFrames").Worker as WorkGiver_Scanner);
        CheckList.Add(
            DefDatabase<WorkGiverDef>.GetNamed("ConstructDeliverResourcesToBlueprints").Worker as WorkGiver_Scanner);
    }

    public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Construction);

    private Job NoCostFrameMakeJobFor(IConstructible c)
    {
        if (c is Blueprint_Install)
        {
            return null;
        }

        if (c is Blueprint && c.TotalMaterialCost().Count == 0)
        {
            return new Job(JobDefOf.PlaceNoCostFrame)
            {
                targetA = (Thing)c
            };
        }

        return null;
    }


    public override Danger MaxPathDanger(Pawn pawn)
    {
        return Danger.Deadly;
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        foreach (var wgiver in CheckList)
        {
            var res = wgiver.JobOnThing(pawn, t, forced);
            if (res != null)
            {
                return res;
            }
        }

        return null;
    }
}