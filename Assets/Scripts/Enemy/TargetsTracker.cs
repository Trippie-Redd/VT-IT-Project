using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// TODO - Implement
// TODO - Make not static
public class TargetsTracker
{
    public static HashSet<Targets> _aliveTargets;

    public static bool IsAlive(Targets target)
        => _aliveTargets.Contains(target);

    public static Targets EnumFromString(string str)
    {
        return str switch
        {
            "2slimey" => Targets.TooSlimey,
            "nettspend" => Targets.Nettspend,
            "epstein" => Targets.Epstein,
            _ => Targets.InvalidTarget
        };
    }
}
