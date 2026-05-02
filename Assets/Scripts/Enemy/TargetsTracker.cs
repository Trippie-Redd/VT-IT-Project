using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// TODO - Implement
// TODO - Make not static
public class TargetsTracker : MonoBehaviour
{
    public HashSet<Targets> _aliveTargets = new()
    {
        Targets.Nettspend,
        Targets.TooSlimey
    };

    public bool IsAlive(Targets target)
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
