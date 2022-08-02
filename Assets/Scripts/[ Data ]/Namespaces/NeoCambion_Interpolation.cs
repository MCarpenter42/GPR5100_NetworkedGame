using System;
using System.Collections;
namespace NeoCambion
{
    using UnityEngine;

    namespace Interpolation
    {
        public static class InterpDelta
        {
            public enum InterpTypes { Linear, CosCurve, CosSpeedUp, CosSlowDown, CosSpeedUpSlowDown, CosSlowDownSpeedUp, SmoothedLinear };

            public static float CosCurve(float rawDelta)
            {
                rawDelta = Mathf.Clamp(rawDelta, 0.0f, 1.0f);
                float rad = rawDelta * Mathf.PI;
                float cos = -Mathf.Cos(rad);
                float output = (cos + 1.0f) * 0.5f;
                return output;
            }

            public static float CosSpeedUp(float rawDelta)
            {
                rawDelta = Mathf.Clamp(rawDelta, 0.0f, 1.0f);
                float rad = rawDelta - 2.0f;
                rad *= Mathf.PI;
                rad /= 2.0f;
                float cos = Mathf.Cos(rad);
                float output = 1.0f + cos;
                return output;
            }

            public static float CosSlowDown(float rawDelta)
            {
                rawDelta = Mathf.Clamp(rawDelta, 0.0f, 1.0f);
                float rad = rawDelta - 1.0f;
                rad *= Mathf.PI;
                rad /= 2.0f;
                float output = Mathf.Cos(rad);
                return output;
            }

            public static float CosSpeedUpSlowDown(float rawDelta, bool forward)
            {
                float output;
                if (forward)
                {
                    output = CosSpeedUp(rawDelta);
                }
                else
                {
                    output = CosSlowDown(rawDelta);
                }
                return output;
            }

            public static float CosSlowDownSpeedUp(float rawDelta, bool forward)
            {
                float output;
                if (forward)
                {
                    output = CosSlowDown(rawDelta);
                }
                else
                {
                    output = CosSpeedUp(rawDelta);
                }
                return output;
            }

            public static float CosHill(float rawDelta)
            {
                rawDelta = Mathf.Clamp(rawDelta, 0.0f, 1.0f);
                float rad = rawDelta * Mathf.PI * 2.0f;
                float cos = -Mathf.Cos(rad);
                float output = (cos + 1.0f) * 0.5f;
                return output;
            }

            public static float CosValley(float rawDelta)
            {
                rawDelta = Mathf.Clamp(rawDelta, 0.0f, 1.0f);
                float rad = rawDelta * Mathf.PI * 2.0f;
                float cos = Mathf.Cos(rad);
                float output = (cos + 1.0f) * 0.5f;
                return output;
            }

            public static float SmoothedLinear(float rawDelta, float smoothing0to1)
            {
                float output = 0.0f;

                float n = 0.25f + 0.75f * Mathf.Clamp(smoothing0to1, 0.0f, 1.0f);
                float p1 = Mathf.Sqrt(n) / 2 + 1;
                float p2 = Mathf.Sqrt(n) / 2;

                float piDivN = Mathf.PI / n;

                if (rawDelta < n / 2.0f)
                {
                    output = (Mathf.Pow(n, p1) * (1 - Mathf.Cos(piDivN * rawDelta))) / 2;
                }
                else if (rawDelta > 1.0f - n / 2.0f)
                {
                    output = 1 - (Mathf.Pow(n, p1) * (1 + Mathf.Cos(piDivN * rawDelta + Mathf.PI - piDivN))) / 2;
                }
                else if (rawDelta >= n / 2.0f && rawDelta <= 1.0f - n / 2.0f)
                {
                    output = (Mathf.Pow(n, p2) * Mathf.PI) / 2 * Mathf.Sin(Mathf.PI / 2) * (rawDelta - 0.5f) + 0.5f;
                }
                else if (rawDelta == 0.5f)
                {
                    output = 0.5f;
                }

                return output;
            }
        }

        public static class ITime
        {
            public static float Time(bool realtime)
            {
                if (realtime)
                {
                    return UnityEngine.Time.unscaledTime;
                }
                else
                {
                    return UnityEngine.Time.time;
                }
            }
            
            public static float DeltaTime(bool realtime)
            {
                if (realtime)
                {
                    return UnityEngine.Time.unscaledDeltaTime;
                }
                else
                {
                    return UnityEngine.Time.deltaTime;
                }
            }
            
            public static float FixedDeltaTime(bool realtime)
            {
                if (realtime)
                {
                    return UnityEngine.Time.fixedUnscaledDeltaTime;
                }
                else
                {
                    return UnityEngine.Time.fixedDeltaTime;
                }
            }

            public static IEnumerator Wait(float time, bool realtime)
            {
                if (realtime)
                {
                    yield return new WaitForSecondsRealtime(time);
                }
                else
                {
                    yield return new WaitForSeconds(time);
                }
            }
        }
    }
}
