using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRWAPI
{
    public static class MATH
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        private static Random random = new System.Random();
        public static float Random(float min, float max)
        {

            return (float)random.NextDouble() * (max - min) + min;
        }
    }
}
