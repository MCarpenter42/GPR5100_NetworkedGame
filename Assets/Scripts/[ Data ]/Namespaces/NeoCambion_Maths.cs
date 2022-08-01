namespace NeoCambion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    namespace Maths
    {
        public static class Ext_Mathf
        {
            public static float ToRad(this float degrees)
            {
                return degrees * Mathf.PI / 180.0f;
            }

            public static float ToDeg(this float radians)
            {
                return radians * 180.0f / Mathf.PI;
            }

            public static int Clamp(this int value, int min, int max)
            {
                if (value < min)
                {
                    return min;
                }
                else if (value > max)
                {
                    return max;
                }
                else
                {
                    return value;
                }
            }

            public static float WrapClamp(this float value, float min, float max)
            {
                float range = max - min;
                if (value < min)
                {
                    float diff = min - value;
                    int mult = (int)((diff - (diff % range)) / range) + 1;
                    return value + (float)mult * range;
                }
                else if (value > max)
                {
                    float diff = value - max;
                    int mult = (int)((diff - (diff % range)) / range) + 1;
                    return value - (float)mult * range;
                }
                else
                {
                    return value;
                }
            }

            public static int WrapClamp(this int value, int min, int max)
            {
                int range = max - min;
                if (value < min)
                {
                    int diff = min - value;
                    int mult = ((diff - (diff % range)) / range) + 1;
                    return value + mult * range;
                }
                else if (value > max)
                {
                    int diff = value - max;
                    int mult = ((diff - (diff % range)) / range) + 1;
                    return value - mult * range;
                }
                else
                {
                    return value;
                }
            }
        }

        public static class BoolConversion
        {
            public static int ToInt(this bool intBool)
            {
                if (intBool)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            public static int ToInt(this bool intBool, int trueVal, int falseVal)
            {
                if (intBool)
                {
                    return trueVal;
                }
                else
                {
                    return falseVal;
                }
            }

            public static bool ToBool(this int boolInt)
            {
                if (boolInt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}