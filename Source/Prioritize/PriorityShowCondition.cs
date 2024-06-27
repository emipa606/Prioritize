using System;
using Verse;

namespace Prioritize;

public struct PriorityShowCondition(Func<Thing, bool> cond, string lab)
{
    public readonly Func<Thing, bool> Cond = cond;
    public readonly string label = lab;
}