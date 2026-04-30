using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Player
{
    public event UnityAction<Void, Vector3, float> Emit = delegate { };

    public static void EmitNoise(NoiseVector3 origin, float strength)
        => emit.Invoke(origin, strength);
}