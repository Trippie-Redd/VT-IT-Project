using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public static class Noise
    {
        public static event UnityAction<Vector3, float> Emit = delegate { };

        public static void EmitNoise(Vector3 origin, float strength)
            => Emit.Invoke(origin, strength);
    }
}
