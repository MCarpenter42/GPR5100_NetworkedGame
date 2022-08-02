namespace NeoCambion
{
    using System;
    using System.Reflection;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    #region [ ENUMERATION TYPES ]

    public enum Axis { X, Y, Z };
    public enum DualAxis { XY, XZ, YZ };
    public enum CompassBearing { North, East, South, West };
    public enum CompassBearingPrecise
    {
        North, NorthNorthEast, NorthEast, EastNorthEast,
        East, EastSouthEast, SouthEast, SouthSouthEast,
        South, SouthSouthWest, SouthWest, WestSouthWest,
        West, WestNorthWest, NorthWest, NorthNorthWest
    };
    public enum RotDirection { Clockwise, CounterClockwise };

    public enum Condition_Number { Never, LessThan, LessThanOrEqualTo, EqualTo, GreaterThanOrEqualTo, GreaterThan, Always };
    public enum Condition_String { Never, Matches, DoesNotMatch, Contains, DoesNotContain, IsSubstring, IsNotSubstring, Always };

    #endregion

    public static class Ext_Object
    {
        public static PropertyInfo GetProperty<T>(this object obj, string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }

        public static object GetPropertyValue<T>(this T obj, string propertyName)
        {
            return typeof(T).GetProperty(propertyName).GetValue(obj);
        }
    }

    public static class Ext_String
    {
        public static List<char> alphaNumUnderscore = new List<char>
        {
            '_', '-',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static bool IsEmptyOrNullOrWhiteSpace(this string text)
        {
            return text.IsNullOrEmpty() || text.IsNullOrWhiteSpace();
        }

        public static bool ValidateString(this string text)
        {
            return text.ValidateString(alphaNumUnderscore, false);
        }
        
        public static bool ValidateString(this string text, bool emptyInvalid)
        {
            return text.ValidateString(alphaNumUnderscore, emptyInvalid);
        }

        public static bool ValidateString(this string text, List<char> validChars)
        {
            return text.ValidateString(validChars, false);
        }
        
        public static bool ValidateString(this string text, List<char> validChars, bool emptyInvalid)
        {
            bool textValid = true;
            if (emptyInvalid && IsEmptyOrNullOrWhiteSpace(text))
            {
                textValid = false;
            }

            int n = text.Length;

            for (int i = 0; i < n; i++)
            {
                char toCheck = char.Parse(text.Substring(i, 1));
                if (!validChars.Contains(toCheck))
                {
                    textValid = false;
                    break;
                }
            }

            return textValid;
        }

        public static string RandomString()
        {
            int a = 5;
            int b = 20;
            int l = UnityEngine.Random.Range(a, b);
            return RandomString(alphaNumUnderscore, l);
        }

        public static string RandomString(int length)
        {
            return RandomString(alphaNumUnderscore, length);
        }

        public static string RandomString(List<char> charSet)
        {
            int a = 5;
            int b = 20;
            int l = UnityEngine.Random.Range(a, b);
            return RandomString(charSet, l);
        }

        public static string RandomString(List<char> charSet, int length)
        {
            string output = "";
            for (int i = 0; i < length; i++)
            {
                int n = UnityEngine.Random.Range(0, charSet.Count - 1);
                output += charSet[n].ToString();
            }
            return output;
        }
    }

    namespace Unity
    {
        public static class UnityExt_Float
        {
            public static string[] StopwatchTime(this float time)
            {
                int seconds = (int)Mathf.FloorToInt(time);
                int subSeconds = (int)Mathf.Floor((time - seconds) * 100.0f);

                int tMinutes = seconds - seconds % 60;
                int tSeconds = seconds % 60;

                string strMinutes = tMinutes.ToString();
                string strSeconds = tSeconds.ToString();
                string strSubSecs = subSeconds.ToString();

                if (strSeconds.Length < 2)
                {
                    strSeconds = "0" + strSeconds;
                }
                if (strSubSecs.Length < 2)
                {
                    strSubSecs = "0" + strSubSecs;
                }

                return new string[] { strMinutes, strSeconds, strSubSecs };
            }
        }

        public static class UnityExt_GameObject
        {
            public static bool Exists(this GameObject obj)
            {
                return obj != null;
            }

            public static bool HasComponent<T>(this GameObject obj) where T : Component
            {
                return obj.GetComponent<T>() != null;
            }

            public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
            {
                if (obj.GetComponent<T>() != null)
                {
                    return obj.GetComponent<T>();
                }
                else
                {
                    return obj.AddComponent<T>();
                }
            }

            public static List<T> GetComponents<T>(this GameObject[] objects)
            {
                List<T> componentsInObjects = new List<T>();
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].TryGetComponent(out T objComponent))
                    {
                        componentsInObjects.Add(objComponent);
                    }
                }
                return componentsInObjects;
            }
            
            public static List<T> GetComponents<T>(this List<GameObject> objects)
            {
                List<T> componentsInObjects = new List<T>();
                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i].TryGetComponent(out T objComponent))
                    {
                        componentsInObjects.Add(objComponent);
                    }
                }
                return componentsInObjects;
            }

            public static List<GameObject> GetObjectsWithComponent<T>(this GameObject[] objects)
            {
                List<GameObject> itemsWithComponent = new List<GameObject>();
                if (objects.Length > 0)
                {
                    for (int i = 0; i < objects.Length; i++)
                    {
                        GameObject item = objects[i];
                        T itemComponent = item.GetComponent<T>();
                        if (!itemComponent.Equals(null))
                        {
                            itemsWithComponent.Add(item);
                        }
                    }
                }

                return itemsWithComponent;
            }
            
            public static List<GameObject> GetObjectsWithComponent<T>(this List<GameObject> objects)
            {
                List<GameObject> itemsWithComponent = new List<GameObject>();
                if (objects.Count > 0)
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        GameObject item = objects[i];
                        T itemComponent = item.GetComponent<T>();
                        if (!itemComponent.Equals(null))
                        {
                            itemsWithComponent.Add(item);
                        }
                    }
                }

                return itemsWithComponent;
            }

            public static List<GameObject> GetObjectsWithTag(this GameObject[] objects, string tag)
            {
                List<GameObject> itemsWithTag = new List<GameObject>();
                if (objects.Length > 0)
                {
                    for (int i = 0; i < objects.Length; i++)
                    {
                        GameObject item = objects[0];
                        if (item.CompareTag(tag))
                        {
                            itemsWithTag.Add(item);
                        }
                    }
                }

                return itemsWithTag;
            }

            public static List<GameObject> GetObjectsWithTag(this List<GameObject> objects, string tag)
            {
                List<GameObject> itemsWithTag = new List<GameObject>();
                if (objects.Count > 0)
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        GameObject item = objects[0];
                        if (item.CompareTag(tag))
                        {
                            itemsWithTag.Add(item);
                        }
                    }
                }

                return itemsWithTag;
            }

            public static List<T> GetComponentsInChildren<T>(GameObject parentObj)
            {
                List<T> componentsInChildren = new List<T>();
                if (parentObj.transform.childCount > 0)
                {
                    for (int i = 0; i < parentObj.transform.childCount; i++)
                    {
                        GameObject child = parentObj.transform.GetChild(i).gameObject;
                        if (child.TryGetComponent<T>(out T childComponent))
                        {
                            componentsInChildren.Add(childComponent);
                        }
                    }
                }
                return componentsInChildren;
            }

            public static List<GameObject> GetChildrenWithComponent<T>(this GameObject parentObj)
            {
                List<GameObject> childrenWithComponent = new List<GameObject>();
                if (parentObj.transform.childCount > 0)
                {
                    for (int i = 0; i < parentObj.transform.childCount; i++)
                    {
                        GameObject child = parentObj.transform.GetChild(i).gameObject;
                        T childComponent;
                        if (child.TryGetComponent<T>(out childComponent))
                        {
                            childrenWithComponent.Add(child);
                        }
                    }
                }
                return childrenWithComponent;
            }

            public static List<GameObject> GetChildrenWithTag(this GameObject parentObj, string tag)
            {
                List<GameObject> childrenWithTag = new List<GameObject>();
                if (parentObj.transform.childCount > 0)
                {
                    for (int i = 0; i < parentObj.transform.childCount; i++)
                    {
                        GameObject child = parentObj.transform.GetChild(i).gameObject;
                        if (child.CompareTag(tag))
                        {
                            childrenWithTag.Add(child);
                        }
                    }
                }
                return childrenWithTag;
            }
        }

        public static class UnityExt_Transform
        {
            public static void MoveTowards(this Transform trn, Vector3 target)
            {
                trn.MoveTowards(target, 1.0f, false);
            }

            public static void MoveTowards(this Transform trn, Vector3 target, float speed)
            {
                trn.MoveTowards(target, speed, false);
            }

            public static void MoveTowards(this Transform trn, Vector3 target, bool fixedDeltaTime)
            {
                trn.MoveTowards(target, 1.0f, fixedDeltaTime);
            }

            public static void MoveTowards(this Transform trn, Vector3 target, float speed, bool fixedDeltaTime)
            {
                Vector3 direction = (target - trn.position).normalized;
                if (fixedDeltaTime)
                {
                    trn.position += direction * speed * Time.fixedDeltaTime;
                }
                else
                {
                    trn.position += direction * speed * Time.deltaTime;
                }
            }
            
            public static void MoveTowards(this Transform trn, Vector3 target, Vector3 tether, float range)
            {
                trn.MoveTowards(target, tether, range, 1.0f, false);
            }

            public static void MoveTowards(this Transform trn, Vector3 target, Vector3 tether, float range, float speed)
            {
                trn.MoveTowards(target, tether, range, speed, false);
            }

            public static void MoveTowards(this Transform trn, Vector3 target, Vector3 tether, float range, bool fixedDeltaTime)
            {
                trn.MoveTowards(target, tether, range, 1.0f, fixedDeltaTime);
            }

            public static void MoveTowards(this Transform trn, Vector3 target, Vector3 tether, float range, float speed, bool fixedDeltaTime)
            {
                Vector3 direction = (target - trn.position).normalized;
                Vector3 newPos;
                if (fixedDeltaTime)
                {
                    newPos = trn.position + direction * speed * Time.fixedDeltaTime;
                }
                else
                {
                    newPos = trn.position + direction * speed * Time.deltaTime;
                }
                Vector3 disp = newPos - tether;
                if (disp.magnitude > range)
                {
                    newPos = (disp).normalized * range;
                }
                trn.position = newPos;
            }
            
            public static void MoveTowards(this Transform trn, Transform target)
            {
                trn.MoveTowards(target, 1.0f, false);
            }

            public static void MoveTowards(this Transform trn, Transform target, float speed)
            {
                trn.MoveTowards(target, speed, false);
            }

            public static void MoveTowards(this Transform trn, Transform target, bool fixedDeltaTime)
            {
                trn.MoveTowards(target, 1.0f, fixedDeltaTime);
            }

            public static void MoveTowards(this Transform trn, Transform target, float speed, bool fixedDeltaTime)
            {
                trn.MoveTowards(target.position, speed, fixedDeltaTime);
            }

            public static void MoveTo(this Transform trn, Vector3 target)
            {
                trn.MoveTo(target, false);
            }

            public static void MoveTo(this Transform trn, Vector3 target, Axis ignoreAxis)
            {
                trn.MoveTo(target, false, ignoreAxis);
            }

            public static void MoveTo(this Transform trn, Vector3 target, DualAxis ignoreAxes)
            {
                trn.MoveTo(target, false, ignoreAxes);
            }

            public static void MoveTo(this Transform trn, Vector3 target, Vector3 posMin, Vector3 posMax)
            {
                trn.MoveTo(target, false, posMin, posMax);
            }

            public static void MoveTo(this Transform trn, Vector3 target, Vector3 posMin, Vector3 posMax, Axis ignoreAxis)
            {
                trn.MoveTo(target, false, posMin, posMax, ignoreAxis);
            }

            public static void MoveTo(this Transform trn, Vector3 target, Vector3 posMin, Vector3 posMax, DualAxis ignoreAxes)
            {
                trn.MoveTo(target, false, posMin, posMax, ignoreAxes);
            }

            public static void MoveTo(this Transform trn, Vector3 target, bool localPos)
            {
                if (localPos)
                {
                    Vector3 localTarget = trn.InverseTransformPoint(target);
                    trn.localPosition = localTarget;
                }
                else
                {
                    trn.position = target;
                }
            }

            public static void MoveTo(this Transform trn, Vector3 target, bool localPos, Axis ignoreAxis)
            {
                if (localPos)
                {
                    Vector3 localTarget = trn.InverseTransformPoint(target);
                    Vector3 lockedTarget = localTarget;
                    switch (ignoreAxis)
                    {
                        default:
                        case Axis.X:
                            lockedTarget.x = trn.localPosition.x;
                            break;

                        case Axis.Y:
                            lockedTarget.y = trn.localPosition.y;
                            break;

                        case Axis.Z:
                            lockedTarget.z = trn.localPosition.z;
                            break;
                    }
                    trn.localPosition = lockedTarget;
                }
                else
                {
                    Vector3 lockedTarget = target;
                    switch (ignoreAxis)
                    {
                        default:
                        case Axis.X:
                            lockedTarget.x = trn.position.x;
                            break;

                        case Axis.Y:
                            lockedTarget.y = trn.position.y;
                            break;

                        case Axis.Z:
                            lockedTarget.z = trn.position.z;
                            break;
                    }
                    trn.position = lockedTarget;
                }
            }

            public static void MoveTo(this Transform trn, Vector3 target, bool localPos, DualAxis ignoreAxes)
            {
                if (localPos)
                {
                    Vector3 localTarget = trn.InverseTransformPoint(target);
                    Vector3 lockedTarget = localTarget;
                    switch (ignoreAxes)
                    {
                        default:
                        case DualAxis.XY:
                            lockedTarget.x = trn.localPosition.x;
                            lockedTarget.y = trn.localPosition.y;
                            break;

                        case DualAxis.XZ:
                            lockedTarget.x = trn.localPosition.x;
                            lockedTarget.z = trn.localPosition.z;
                            break;

                        case DualAxis.YZ:
                            lockedTarget.y = trn.localPosition.y;
                            lockedTarget.z = trn.localPosition.z;
                            break;
                    }
                    trn.localPosition = lockedTarget;
                }
                else
                {
                    Vector3 lockedTarget = target;
                    switch (ignoreAxes)
                    {
                        default:
                        case DualAxis.XY:
                            lockedTarget.x = trn.position.x;
                            lockedTarget.y = trn.position.y;
                            break;

                        case DualAxis.XZ:
                            lockedTarget.x = trn.position.x;
                            lockedTarget.z = trn.position.z;
                            break;

                        case DualAxis.YZ:
                            lockedTarget.y = trn.position.y;
                            lockedTarget.z = trn.position.z;
                            break;
                    }
                    trn.position = lockedTarget;
                }
            }

            public static void MoveTo(this Transform trn, Vector3 target, bool localPos, Vector3 posMin, Vector3 posMax)
            {
                if (localPos)
                {
                    Vector3 localTarget = trn.InverseTransformPoint(target);
                    Vector3 lockedTarget = localTarget;
                    lockedTarget.x = Mathf.Clamp(localTarget.x, posMin.x, posMax.x);
                    lockedTarget.y = Mathf.Clamp(localTarget.y, posMin.y, posMax.y);
                    lockedTarget.z = Mathf.Clamp(localTarget.z, posMin.z, posMax.z);
                    trn.localPosition = lockedTarget;
                }
                else
                {
                    Vector3 lockedTarget = trn.InverseTransformPoint(target);
                    lockedTarget.x = Mathf.Clamp(lockedTarget.x, posMin.x, posMax.x);
                    lockedTarget.y = Mathf.Clamp(lockedTarget.y, posMin.y, posMax.y);
                    lockedTarget.z = Mathf.Clamp(lockedTarget.z, posMin.z, posMax.z);
                    trn.position = lockedTarget;
                }
            }

            public static void MoveTo(this Transform trn, Vector3 target, bool localPos, Vector3 posMin, Vector3 posMax, Axis ignoreAxis)
            {
                if (localPos)
                {
                    Vector3 localTarget = trn.InverseTransformPoint(target);
                    Vector3 lockedTarget = localTarget;
                    switch (ignoreAxis)
                    {
                        default:
                        case Axis.X:
                            lockedTarget.x = trn.localPosition.x;
                            break;

                        case Axis.Y:
                            lockedTarget.y = trn.localPosition.y;
                            break;

                        case Axis.Z:
                            lockedTarget.z = trn.localPosition.z;
                            break;
                    }
                    lockedTarget.x = Mathf.Clamp(lockedTarget.x, posMin.x, posMax.x);
                    lockedTarget.y = Mathf.Clamp(lockedTarget.y, posMin.y, posMax.y);
                    lockedTarget.z = Mathf.Clamp(lockedTarget.z, posMin.z, posMax.z);
                    trn.localPosition = lockedTarget;
                }
                else
                {
                    Vector3 lockedTarget = target;
                    switch (ignoreAxis)
                    {
                        default:
                        case Axis.X:
                            lockedTarget.x = trn.position.x;
                            break;

                        case Axis.Y:
                            lockedTarget.y = trn.position.y;
                            break;

                        case Axis.Z:
                            lockedTarget.z = trn.position.z;
                            break;
                    }
                    lockedTarget.x = Mathf.Clamp(lockedTarget.x, posMin.x, posMax.x);
                    lockedTarget.y = Mathf.Clamp(lockedTarget.y, posMin.y, posMax.y);
                    lockedTarget.z = Mathf.Clamp(lockedTarget.z, posMin.z, posMax.z);
                    trn.position = lockedTarget;
                }
            }

            public static void MoveTo(this Transform trn, Vector3 target, bool localPos, Vector3 posMin, Vector3 posMax, DualAxis ignoreAxes)
            {
                if (localPos)
                {
                    Vector3 localTarget = trn.InverseTransformPoint(target);
                    Vector3 lockedTarget = localTarget;
                    switch (ignoreAxes)
                    {
                        default:
                        case DualAxis.XY:
                            lockedTarget.x = trn.localPosition.x;
                            lockedTarget.y = trn.localPosition.y;
                            break;

                        case DualAxis.XZ:
                            lockedTarget.x = trn.localPosition.x;
                            lockedTarget.z = trn.localPosition.z;
                            break;

                        case DualAxis.YZ:
                            lockedTarget.y = trn.localPosition.y;
                            lockedTarget.z = trn.localPosition.z;
                            break;
                    }
                    lockedTarget.x = Mathf.Clamp(lockedTarget.x, posMin.x, posMax.x);
                    lockedTarget.y = Mathf.Clamp(lockedTarget.y, posMin.y, posMax.y);
                    lockedTarget.z = Mathf.Clamp(lockedTarget.z, posMin.z, posMax.z);
                    trn.localPosition = lockedTarget;
                }
                else
                {
                    Vector3 lockedTarget = target;
                    switch (ignoreAxes)
                    {
                        default:
                        case DualAxis.XY:
                            lockedTarget.x = trn.position.x;
                            lockedTarget.y = trn.position.y;
                            break;

                        case DualAxis.XZ:
                            lockedTarget.x = trn.position.x;
                            lockedTarget.z = trn.position.z;
                            break;

                        case DualAxis.YZ:
                            lockedTarget.y = trn.position.y;
                            lockedTarget.z = trn.position.z;
                            break;
                    }
                    lockedTarget.x = Mathf.Clamp(lockedTarget.x, posMin.x, posMax.x);
                    lockedTarget.y = Mathf.Clamp(lockedTarget.y, posMin.y, posMax.y);
                    lockedTarget.z = Mathf.Clamp(lockedTarget.z, posMin.z, posMax.z);
                    trn.position = lockedTarget;
                }
            }

            public static void MoveTo(this Transform trn, Vector3 target, Vector3 tether, float range)
            {
                float dist = (target - tether).magnitude;
                if (dist <= range)
                {
                    trn.MoveTo(target, false);
                }
                else
                {
                    Vector3 dir = (target - tether).normalized;
                    Vector3 tetheredTarget = tether + dir * range;
                    trn.MoveTo(tetheredTarget, false);
                }
            }

            public static void MoveTo(this Transform trn, Transform target)
            {
                trn.MoveTo(target.position, false);
            }

            public static void MoveTo(this Transform trn, Transform target, Axis ignoreAxis)
            {
                trn.MoveTo(target.position, false, ignoreAxis);
            }

            public static void MoveTo(this Transform trn, Transform target, DualAxis ignoreAxes)
            {
                trn.MoveTo(target.position, false, ignoreAxes);
            }

            public static void MoveTo(this Transform trn, Transform target, Vector3 posMin, Vector3 posMax)
            {
                trn.MoveTo(target.position, false, posMin, posMax);
            }

            public static void MoveTo(this Transform trn, Transform target, Vector3 posMin, Vector3 posMax, Axis ignoreAxis)
            {
                trn.MoveTo(target.position, false, posMin, posMax, ignoreAxis);
            }

            public static void MoveTo(this Transform trn, Transform target, Vector3 posMin, Vector3 posMax, DualAxis ignoreAxes)
            {
                trn.MoveTo(target.position, false, posMin, posMax, ignoreAxes);
            }

            public static void MoveTo(this Transform trn, Transform target, bool localPos)
            {
                trn.MoveTo(target.position, localPos);
            }

            public static void MoveTo(this Transform trn, Transform target, bool localPos, Axis ignoreAxis)
            {
                trn.MoveTo(target.position, localPos, ignoreAxis);
            }

            public static void MoveTo(this Transform trn, Transform target, bool localPos, DualAxis ignoreAxes)
            {
                trn.MoveTo(target.position, localPos, ignoreAxes);
            }

            public static void MoveTo(this Transform trn, Transform target, bool localPos, Vector3 posMin, Vector3 posMax)
            {
                trn.MoveTo(target.position, localPos, posMin, posMax);
            }

            public static void MoveTo(this Transform trn, Transform target, bool localPos, Vector3 posMin, Vector3 posMax, Axis ignoreAxis)
            {
                trn.MoveTo(target.position, localPos, posMin, posMax, ignoreAxis);
            }

            public static void MoveTo(this Transform trn, Transform target, bool localPos, Vector3 posMin, Vector3 posMax, DualAxis ignoreAxes)
            {
                trn.MoveTo(target.position, localPos, posMin, posMax, ignoreAxes);
            }

            public static void MoveTo(this Transform trn, Transform target, Vector3 tether, float range)
            {
                trn.MoveTo(target.position, tether, range);
            }

        }

        public static class UnityExt_Vector2
        {
            public static Vector2 Closest(this Vector2 origin, Vector2[] points)
            {
                return points[ClosestIndex(origin, points)];
            }

            public static Vector2 Closest(this Vector2 origin, List<Vector2> points)
            {
                return points[ClosestIndex(origin, points)];
            }

            public static int ClosestIndex(this Vector2 origin, Vector2[] points)
            {
                int closest = -1;
                float closestDist = float.MaxValue;

                for (int i = 0; i < points.Length; i++)
                {
                    if ((origin - points[i]).magnitude < closestDist)
                    {
                        closest = i;
                        closestDist = (origin - points[i]).magnitude;
                    }
                }

                return closest;
            }

            public static int ClosestIndex(this Vector2 origin, List<Vector2> points)
            {
                int closest = -1;
                float closestDist = float.MaxValue;

                for (int i = 0; i < points.Count; i++)
                {
                    if ((origin - points[i]).magnitude < closestDist)
                    {
                        closest = i;
                        closestDist = (origin - points[i]).magnitude;
                    }
                }

                return closest;
            }

            public static Vector2 Furthest(this Vector2 origin, Vector2[] points)
            {
                return points[FurthestIndex(origin, points)];
            }

            public static Vector2 Furthest(this Vector2 origin, List<Vector2> points)
            {
                return points[FurthestIndex(origin, points)];
            }

            public static int FurthestIndex(this Vector2 origin, Vector2[] points)
            {
                int furthest = -1;
                float furthestDist = -1.0f;

                for (int i = 0; i < points.Length; i++)
                {
                    if ((origin - points[i]).magnitude > furthestDist)
                    {
                        furthest = i;
                        furthestDist = (origin - points[i]).magnitude;
                    }
                }

                return furthest;
            }

            public static int FurthestIndex(this Vector2 origin, List<Vector2> points)
            {
                int furthest = -1;
                float furthestDist = -1.0f;

                for (int i = 0; i < points.Count; i++)
                {
                    if ((origin - points[i]).magnitude > furthestDist)
                    {
                        furthest = i;
                        furthestDist = (origin - points[i]).magnitude;
                    }
                }

                return furthest;
            }

            public static Vector2 SetAxis(this Vector2 vect, Axis axis, float value)
            {
                switch (axis)
                {
                    default:
                    case Axis.X:
                        vect.x = value;
                        break;

                    case Axis.Y:
                        vect.y = value;
                        break;

                    case Axis.Z:
                        throw new Exception("Vector2 objects have no Z value to set!");
                }
                return vect;
            }

            public static Vector2 AddToAxis(this Vector2 vect, Axis axis, float value)
            {
                switch (axis)
                {
                    default:
                    case Axis.X:
                        vect.x += value;
                        break;

                    case Axis.Y:
                        vect.y += value;
                        break;

                    case Axis.Z:
                        throw new Exception("Vector2 objects have no Z value to modify!");
                }
                return vect;
            }
        }

        public static class UnityExt_Vector3
        {
            public static Vector3 RestrictRotVector(this Vector3 rotVect)
            {
                if (rotVect.x > 180.0f)
                {
                    rotVect.x -= 360.0f;
                }
                else if (rotVect.x < -180.0f)
                {
                    rotVect.x += 360.0f;
                }

                if (rotVect.y > 180.0f)
                {
                    rotVect.y -= 360.0f;
                }
                else if (rotVect.y < -180.0f)
                {
                    rotVect.y += 360.0f;
                }

                if (rotVect.z > 180.0f)
                {
                    rotVect.z -= 360.0f;
                }
                else if (rotVect.z < -180.0f)
                {
                    rotVect.z += 360.0f;
                }

                return rotVect;
            }

            public static Vector3 Closest(this Vector3 origin, Vector3[] points)
            {
                return points[ClosestIndex(origin, points)];
            }
            
            public static Vector3 Closest(this Vector3 origin, List<Vector3> points)
            {
                return points[ClosestIndex(origin, points)];
            }
            
            public static int ClosestIndex(this Vector3 origin, Vector3[] points)
            {
                int closest = -1;
                float closestDist = float.MaxValue;

                for (int i = 0; i < points.Length; i++)
                {
                    if ((origin - points[i]).magnitude < closestDist)
                    {
                        closest = i;
                        closestDist = (origin - points[i]).magnitude;
                    }
                }

                return closest;
            }
            
            public static int ClosestIndex(this Vector3 origin, List<Vector3> points)
            {
                int closest = -1;
                float closestDist = float.MaxValue;

                for (int i = 0; i < points.Count; i++)
                {
                    if ((origin - points[i]).magnitude < closestDist)
                    {
                        closest = i;
                        closestDist = (origin - points[i]).magnitude;
                    }
                }

                return closest;
            }
            
            public static Vector3 Furthest(this Vector3 origin, Vector3[] points)
            {
                return points[FurthestIndex(origin, points)];
            }
            
            public static Vector3 Furthest(this Vector3 origin, List<Vector3> points)
            {
                return points[FurthestIndex(origin, points)];
            }
            
            public static int FurthestIndex(this Vector3 origin, Vector3[] points)
            {
                int furthest = -1;
                float furthestDist = -1.0f;

                for (int i = 0; i < points.Length; i++)
                {
                    if ((origin - points[i]).magnitude > furthestDist)
                    {
                        furthest = i;
                        furthestDist = (origin - points[i]).magnitude;
                    }
                }

                return furthest;
            }
            
            public static int FurthestIndex(this Vector3 origin, List<Vector3> points)
            {
                int furthest = -1;
                float furthestDist = -1.0f;

                for (int i = 0; i < points.Count; i++)
                {
                    if ((origin - points[i]).magnitude > furthestDist)
                    {
                        furthest = i;
                        furthestDist = (origin - points[i]).magnitude;
                    }
                }

                return furthest;
            }

            public static Vector3 SetAxis(this Vector3 vect, Axis axis, float value)
            {
                switch (axis)
                {
                    default:
                    case Axis.X:
                        vect.x = value;
                        break;

                    case Axis.Y:
                        vect.y = value;
                        break;

                    case Axis.Z:
                        vect.z = value;
                        break;
                }
                return vect;
            }

            public static Vector3 AddToAxis(this Vector3 vect, Axis axis, float value)
            {
                switch (axis)
                {
                    default:
                    case Axis.X:
                        vect.x += value;
                        break;

                    case Axis.Y:
                        vect.y += value;
                        break;

                    case Axis.Z:
                        vect.z += value;
                        break;
                }
                return vect;
            }
        }
    }
}