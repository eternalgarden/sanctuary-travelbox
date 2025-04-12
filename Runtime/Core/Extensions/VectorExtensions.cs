using UnityEngine;

namespace TravelBox.Extensions
{
    public static class VectorExtensions
    {
        public static bool IsApproximatelyEqualTo(this Vector3 source, Vector3 compare, float tolerance = 0.0001f)
        {
            return Vector3.SqrMagnitude(source - compare) < tolerance * tolerance;
        }
    }
}
