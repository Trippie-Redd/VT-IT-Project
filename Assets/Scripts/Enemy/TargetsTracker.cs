using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    // TODO - Implement
    public class TargetsTracker : MonoBehaviour
    {
        [Serializable]
        public struct TargetDamageablePair
        {
            public Target target;
            public Damageable damageable;

            public readonly void Deconstruct(out Target target, out Damageable damageable)
            {
                target = this.target;
                damageable = this.damageable;
            }
        }

        // a tuple would be nicer but they arent Serializable, unity is not it
        [SerializeField] List<TargetDamageablePair> aliveTargets;

        public bool AllTargetsDead => aliveTargets.Count == 0;

        void Update()
        {
            if (aliveTargets.Count == 0)
            {
                // unlock exits
            }

            for (int i = 0; i < aliveTargets.Count; i++)
            {
                if (aliveTargets[i].damageable.IsDead)
                {
                    aliveTargets.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool IsAlive(Target target)
        {
            foreach (var (t, _) in aliveTargets)
            {
                if (t == target) 
                {
                    return true;
                }
            }

            return false;
        }

        // this would be nice to have inside of the Target enum but of course C# doesnt allow that
        public static Target EnumFromString(string str)
        {
            return str switch
            {
                "2slimey" => Target.TooSlimey,
                "nettspend" => Target.Nettspend,
                "epstein" => Target.Epstein,
                _ => Target.InvalidTarget
            };
        }
    }
}