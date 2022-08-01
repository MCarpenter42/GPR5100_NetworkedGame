namespace NeoCambion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    namespace Collections
    {
        public static class Ext_Collections
        {
            #region [ BOUNDS CHECKING ]

            public static bool InBounds<T>(this int index, T[] array)
            {
                if (index > -1 && index < array.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool InBounds<T>(this T[] array, int index)
            {
                if (index > -1 && index < array.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool InBounds<T>(this int index, List<T> list)
            {
                if (index > -1 && index < list.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool InBounds<T>(this List<T> list, int index)
            {
                if (index > -1 && index < list.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            #endregion

            #region [ TYPE CONVERSION ]

            public static List<T> ToList<T>(this T[] array)
            {
                List<T> listOut = new List<T>();
                for (int i = 0; i < array.Length; i++)
                {
                    listOut.Add(array[i]);
                }
                return listOut;
            }

            public static T[] ToArray<T>(this List<T> list)
            {
                T[] arrayOut = new T[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    arrayOut[i] = list[i];
                }
                return arrayOut;
            }

            #endregion

            #region [ DATA COPYING ]

            public static void CopyListData<T>(List<T> source, List<T> destination)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    destination.Add(source[i]);
                }
            }

            public static void CopyTo<T>(this List<T> source, List<T> destination)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    destination.Add(source[i]);
                }
            }

            public static void CopyFrom<T>(this List<T> destination, List<T> source)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    destination.Add(source[i]);
                }
            }

            #endregion

            #region [ SEARCHING ]

            public static bool SubListContains<T>(this List<List<T>> listOfLists, T item)
            {
                foreach (List<T> subList in listOfLists)
                {
                    if (subList.Contains(item))
                    {
                        return true;
                    }
                }
                return false;
            }

            public static List<int> IndicesOf<T>(this List<T> list, T item)
            {
                List<int> output = new List<int>();
                int lastIndex = -1;
                bool contSearch = true;
                while (contSearch)
                {
                    int n = list.IndexOf(item, lastIndex + 1);
                    if (n == -1)
                    {
                        contSearch = false;

                        break;
                    }
                    else
                    {
                        output.Add(n);
                        lastIndex = n;
                    }
                }
                return output;
            }

            public static List<int[]> IndicesOf<T>(this List<List<T>> listOfLists, T item)
            {
                List<int[]> output = new List<int[]>();

                for (int i = 0; i < listOfLists.Count; i++)
                {
                    List<int> indices = listOfLists[i].IndicesOf(item);
                    if (indices.Count > 0)
                    {
                        for (int j = 0; j < indices.Count; j++)
                        {
                            output.Add(new int[] { i, indices[j] });
                        }
                    }
                }

                return output;
            }

            #endregion

            #region [ RANDOMISATION ]

            public static T PickFromList<T>(this List<T> itemList)
            {
                if (itemList.Count > 0)
                {
                    int n = UnityEngine.Random.Range(0, itemList.Count - 1);
                    return itemList[n];
                }
                else
                {
                    return default;
                }
            }

            public static T[] Shuffle<T>(this T[] array)
            {
                int n = array.Length;
                T[] output = new T[n];
                List<T> vals = new List<T>();
                for (int i = 0; i < n; i++)
                {
                    vals.Add(array[i]);
                }
                for (int i = 0; i < n; i++)
                {
                    int r = 0;
                    if (vals.Count > 1)
                    { r = UnityEngine.Random.Range(0, vals.Count - 1); }
                    output[i] = vals[r];
                    vals.RemoveAt(r);
                }
                return output;
            }

            public static List<T> Shuffle<T>(this List<T> list)
            {
                int n = list.Count;
                List<T> output = new List<T>();
                List<T> vals = new List<T>();
                vals.CopyFrom(list);
                for (int i = 0; i < n; i++)
                {
                    int r = 0;
                    if (vals.Count > 1)
                    { r = UnityEngine.Random.Range(0, vals.Count - 1); }
                    output[i] = vals[r];
                    vals.RemoveAt(r);
                }
                return output;
            }

            #endregion

            public static void Clear<T>(this T[] array)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = default(T);
                }
            }
        }

        public static class Ext_StringList
        {
            public static List<bool> Contains(this List<string> strings, string value)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Contains(value));
                }
                return output;
            }
            
            public static List<bool> EndsWith(this List<string> strings, string value, StringComparison comparisonType)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].EndsWith(value, comparisonType));
                }
                return output;
            }
            
            public static List<bool> EndsWith(this List<string> strings, string value, bool ignoreCase, System.Globalization.CultureInfo culture)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].EndsWith(value, ignoreCase, culture));
                }
                return output;
            }
            
            public static List<bool> EndsWith(this List<string> strings, string value)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].EndsWith(value));
                }
                return output;
            }

            public static List<int> IndexOf(this List<string> strings, string value, int startIndex, StringComparison comparisonType)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, startIndex, comparisonType));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, string value, StringComparison comparisonType)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, comparisonType));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, string value, int startIndex, int count)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, startIndex, count));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, string value)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, char value, int startIndex, int count)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, startIndex, count));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, char value, int startIndex)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, startIndex));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, char value)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, string value, int startIndex, int count, StringComparison comparisonType)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, startIndex, count, comparisonType));
                }
                return output;
            }
            
            public static List<int> IndexOf(this List<string> strings, string value, int startIndex)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOf(value, startIndex));
                }
                return output;
            }

            public static List<int> IndexOfAny(this List<string> strings, char[] anyOf)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOfAny(anyOf));
                }
                return output;
            }
            
            public static List<int> IndexOfAny(this List<string> strings, char[] anyOf, int startIndex, int count)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOfAny(anyOf, startIndex, count));
                }
                return output;
            }
            
            public static List<int> IndexOfAny(this List<string> strings, char[] anyOf, int startIndex)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IndexOfAny(anyOf, startIndex));
                }
                return output;
            }

            public static List<string> Insert(this List<string> strings, int startIndex, string value)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Insert(startIndex, value));
                }
                return output;
            }

            public static List<bool> IsNormalized(this List<string> strings)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IsNormalized());
                }
                return output;
            }
            
            public static List<bool> IsNormalized(this List<string> strings, System.Text.NormalizationForm normalizationForm)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].IsNormalized(normalizationForm));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, string value, int startIndex, StringComparison comparisonType)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, startIndex, comparisonType));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, string value, StringComparison comparisonType)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, comparisonType));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, string value, int startIndex, int count)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, startIndex, count));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, string value)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, char value, int startIndex, int count)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, startIndex, count));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, char value, int startIndex)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, startIndex));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, char value)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, string value, int startIndex, int count, StringComparison comparisonType)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, startIndex, count, comparisonType));
                }
                return output;
            }

            public static List<int> LastIndexOf(this List<string> strings, string value, int startIndex)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOf(value, startIndex));
                }
                return output;
            }

            public static List<int> LastIndexOfAny(this List<string> strings, char[] anyOf)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOfAny(anyOf));
                }
                return output;
            }

            public static List<int> LastIndexOfAny(this List<string> strings, char[] anyOf, int startIndex, int count)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOfAny(anyOf, startIndex, count));
                }
                return output;
            }

            public static List<int> LastIndexOfAny(this List<string> strings, char[] anyOf, int startIndex)
            {
                List<int> output = new List<int>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].LastIndexOfAny(anyOf, startIndex));
                }
                return output;
            }

            public static List<string> Normalize(this List<string> strings)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Normalize());
                }
                return output;
            }

            public static List<string> Normalize(this List<string> strings, System.Text.NormalizationForm normalizationForm)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Normalize(normalizationForm));
                }
                return output;
            }

            public static List<string> PadLeft(this List<string> strings, int totalWidth)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].PadLeft(totalWidth));
                }
                return output;
            }
            
            public static List<string> PadLeft(this List<string> strings, int totalWidth, char paddingChar)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].PadLeft(totalWidth, paddingChar));
                }
                return output;
            }
            
            public static List<string> PadRight(this List<string> strings, int totalWidth)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].PadRight(totalWidth));
                }
                return output;
            }
            
            public static List<string> PadRight(this List<string> strings, int totalWidth, char paddingChar)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].PadRight(totalWidth, paddingChar));
                }
                return output;
            }
            
            public static List<string> Remove(this List<string> strings, int startIndex)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Remove(startIndex));
                }
                return output;
            }
            
            public static List<string> Remove(this List<string> strings, int startIndex, int count)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Remove(startIndex, count));
                }
                return output;
            }
            
            public static List<string> Replace(this List<string> strings, string oldValue, string newValue)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Replace(oldValue, newValue));
                }
                return output;
            }
            
            public static List<string> Replace(this List<string> strings, char oldChar, char newChar)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Replace(oldChar, newChar));
                }
                return output;
            }

            public static List<string[]> Split(this List<string> strings, string[] separator, int count, StringSplitOptions option)
            {
                List<string[]> output = new List<string[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Split(separator, count, option));
                }
                return output;
            }
            
            public static List<string[]> Split(this List<string> strings, params char[] separator)
            {
                List<string[]> output = new List<string[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Split(separator));
                }
                return output;
            }
            
            public static List<string[]> Split(this List<string> strings, char[] separator, int count)
            {
                List<string[]> output = new List<string[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Split(separator, count));
                }
                return output;
            }
            
            public static List<string[]> Split(this List<string> strings, char[] separator, int count, StringSplitOptions options)
            {
                List<string[]> output = new List<string[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Split(separator, count, options));
                }
                return output;
            }
            
            public static List<string[]> Split(this List<string> strings, char[] separator, StringSplitOptions options)
            {
                List<string[]> output = new List<string[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Split(separator, options));
                }
                return output;
            }
            
            public static List<string[]> Split(this List<string> strings, string[] separator, StringSplitOptions options)
            {
                List<string[]> output = new List<string[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Split(separator, options));
                }
                return output;
            }

            public static List<bool> StartsWith(this List<string> strings, string value, StringComparison comparisonType)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].StartsWith(value, comparisonType));
                }
                return output;
            }

            public static List<bool> StartsWith(this List<string> strings, string value, bool ignoreCase, System.Globalization.CultureInfo culture)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].StartsWith(value, ignoreCase, culture));
                }
                return output;
            }

            public static List<bool> StartsWith(this List<string> strings, string value)
            {
                List<bool> output = new List<bool>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].StartsWith(value));
                }
                return output;
            }

            public static List<string> Substring(this List<string> strings, int startIndex)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Substring(startIndex));
                }
                return output;
            }

            public static List<string> Substring(this List<string> strings, int startIndex, int length)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Substring(startIndex, length));
                }
                return output;
            }

            public static List<char[]> ToCharArray(this List<string> strings, int startIndex, int length)
            {
                List<char[]> output = new List<char[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToCharArray(startIndex, length));
                }
                return output;
            }
            
            public static List<char[]> ToCharArray(this List<string> strings)
            {
                List<char[]> output = new List<char[]>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToCharArray());
                }
                return output;
            }

            public static List<string> ToLower(this List<string> strings)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToLower());
                }
                return output;
            }

            public static List<string> ToLower(this List<string> strings, System.Globalization.CultureInfo culture)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToLower(culture));
                }
                return output;
            }

            public static List<string> ToLowerInvariant(this List<string> strings)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToLowerInvariant());
                }
                return output;
            }
            
            public static List<string> ToUpper(this List<string> strings)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToUpper());
                }
                return output;
            }

            public static List<string> ToUpper(this List<string> strings, System.Globalization.CultureInfo culture)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToUpper(culture));
                }
                return output;
            }

            public static List<string> ToUpperInvariant(this List<string> strings)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].ToUpperInvariant());
                }
                return output;
            }

            public static List<string> Trim(this List<string> strings)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Trim());
                }
                return output;
            }

            public static List<string> Trim(this List<string> strings, params char[] trimChars)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].Trim(trimChars));
                }
                return output;
            }
            
            public static List<string> TrimEnd(this List<string> strings, params char[] trimChars)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].TrimEnd(trimChars));
                }
                return output;
            }
            
            public static List<string> TrimStart(this List<string> strings, params char[] trimChars)
            {
                List<string> output = new List<string>();
                for (int i = 0; i < strings.Count; i++)
                {
                    output.Add(strings[i].TrimStart(trimChars));
                }
                return output;
            }
        }
    }
}