using System;
using UnityEngine;

namespace Core
{
    public static class MathExtensions
    {
        /// <summary>
        /// Round to <see cref="decimals"/> decimals.
        ///
        /// Modifies in-place and return itself.
        /// </summary>
        public static Vector3 Tidy(this Vector3 value, int decimals = 3)
        {
            value.x = value.x.Tidy();
            value.y = value.y.Tidy();
            value.z = value.z.Tidy();
            return value;
        }

        /// <summary>
        /// Round to <see cref="decimals"/> decimals.
        /// </summary>
        public static float Tidy(this float value, int decimals = 3)
        {
            return (float)Decimal.Round((decimal)value, decimals);
        }
    }
}