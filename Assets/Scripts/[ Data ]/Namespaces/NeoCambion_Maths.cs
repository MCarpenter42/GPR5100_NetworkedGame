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

        public static class VectorMaths
        {
            public static float AngleFromAxis(this Vector3 vect, DualAxis plane, bool measureFromSecond)
            {
                Vector2 vectConv;
                switch (plane)
                {
                    case DualAxis.XY:
                        if (measureFromSecond)
                        {
                            vectConv.x = vect.y;
                            vectConv.y = vect.x;
                        }
                        else
                        {
                            vectConv.x = vect.x;
                            vectConv.y = vect.y;
                        }
                        break;

                    default:
                    case DualAxis.XZ:
                        if (measureFromSecond)
                        {
                            vectConv.x = vect.z;
                            vectConv.y = vect.x;
                        }
                        else
                        {
                            vectConv.x = vect.x;
                            vectConv.y = vect.z;
                        }
                        break;

                    case DualAxis.YZ:
                        if (measureFromSecond)
                        {
                            vectConv.x = vect.z;
                            vectConv.y = vect.y;
                        }
                        else
                        {
                            vectConv.x = vect.y;
                            vectConv.y = vect.z;
                        }
                        break;
                }
                vectConv.Normalize();
                float angle = Vector2.Angle(Vector2.up, vectConv);
                if (vectConv.x >= 0.0f)
                {
                    return angle;
                }
                else
                {
                    return -angle;
                }
            }
        }
    }
}