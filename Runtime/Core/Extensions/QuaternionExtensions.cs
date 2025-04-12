/* 
      |\      _,,,---,,_
ZZZzz /,`.-'`'    -.  ;-;;,_
     |,4-  ) )-,_. ,\ (  `'-'
    '---''(_/--'  `-'\_)
*/

using System;
using UnityEngine;

namespace TravelBox.Extensions
{
    public static class QuaternionExtensions
    {
        /*
         * A dot product of 1 means the quaternions are identical.
         * A dot product of -1 means they are identical but flipped (represent the same rotation).
         */
        public static bool IsApproximatelyEqualTo(this Quaternion source, Quaternion other, float tolerance = 0.0001f)
        {
            return Mathf.Abs(Quaternion.Dot(source, other)) > 1f - (tolerance * tolerance);
        }
    }
}

/* created at 2025-01-19, Sun, 16:50 🌊 */
/* dreamy guardian ASCII kitty by Felix Lee, found at asciiart.eu 🐱‍👤 */