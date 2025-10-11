using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Prioritize
{
    public static class ConstructibleExtensions
    {
        public static List<ThingDefCountClass> TotalMaterialCost(this IConstructible constructible)
        {
            return constructible.TotalMaterialCost();
        }
    }
}
