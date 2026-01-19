using UnityEngine;

namespace Utils
{
    public static class VectorMath
    {
        public static Vector3 ExtractDotVector(Vector3 vector, Vector3 direction)
        {
            direction.Normalize();
            return direction * Vector3.Dot(vector, direction);
        }
        
        public static Vector3 RemoveDotVector(Vector3 vector, Vector3 direction) 
        {
            direction.Normalize();
            return vector - direction * Vector3.Dot(vector, direction);
        }
    }
}