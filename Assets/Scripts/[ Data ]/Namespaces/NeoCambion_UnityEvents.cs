namespace NeoCambion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    namespace Unity
    {
        namespace Events
        {
            /*
            [Serializable]
            public class UnityEvent_classType : UnityEvent<classType> { }
            [Serializable]
            public class UnityEvent_classTypeArray : UnityEvent<classType[]> { }
            [Serializable]
            public class UnityEvent_classTypeList : UnityEvent<List<classType>> { }
            */

            #region [ C# DATA TYPES ]

            [Serializable]
            public class UnityEvent : UnityEngine.Events.UnityEvent
            {
                public UnityEvent() { }
                public UnityEvent(UnityAction call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_bool : UnityEvent<bool>
            {
                public UnityEvent_bool() { }
                public UnityEvent_bool(UnityAction<bool> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_boolArray : UnityEvent<bool[]>
            {
                public UnityEvent_boolArray() { }
                public UnityEvent_boolArray(UnityAction<bool[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_boolList : UnityEvent<List<bool>>
            {
                public UnityEvent_boolList() { }
                public UnityEvent_boolList(UnityAction<List<bool>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_byte : UnityEvent<byte>
            {
                public UnityEvent_byte() { }
                public UnityEvent_byte(UnityAction<byte> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_byteArray : UnityEvent<byte[]>
            {
                public UnityEvent_byteArray() { }
                public UnityEvent_byteArray(UnityAction<byte[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_byteList : UnityEvent<List<byte>>
            {
                public UnityEvent_byteList() { }
                public UnityEvent_byteList(UnityAction<List<byte>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_char : UnityEvent<char>
            {
                public UnityEvent_char() { }
                public UnityEvent_char(UnityAction<char> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_charArray : UnityEvent<char[]>
            {
                public UnityEvent_charArray() { }
                public UnityEvent_charArray(UnityAction<char[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_charList : UnityEvent<List<char>>
            {
                public UnityEvent_charList() { }
                public UnityEvent_charList(UnityAction<List<char>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_decimal : UnityEvent<decimal>
            {
                public UnityEvent_decimal() { }
                public UnityEvent_decimal(UnityAction<decimal> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_decimalArray : UnityEvent<decimal[]>
            {
                public UnityEvent_decimalArray() { }
                public UnityEvent_decimalArray(UnityAction<decimal[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_decimalList : UnityEvent<List<decimal>>
            {
                public UnityEvent_decimalList() { }
                public UnityEvent_decimalList(UnityAction<List<decimal>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_double : UnityEvent<double>
            {
                public UnityEvent_double() { }
                public UnityEvent_double(UnityAction<double> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_doubleArray : UnityEvent<double[]>
            {
                public UnityEvent_doubleArray() { }
                public UnityEvent_doubleArray(UnityAction<double[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_doubleList : UnityEvent<List<double>>
            {
                public UnityEvent_doubleList() { }
                public UnityEvent_doubleList(UnityAction<List<double>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_dynamic : UnityEvent<dynamic>
            {
                public UnityEvent_dynamic() { }
                public UnityEvent_dynamic(UnityAction<dynamic> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_dynamicArray : UnityEvent<dynamic[]>
            {
                public UnityEvent_dynamicArray() { }
                public UnityEvent_dynamicArray(UnityAction<dynamic[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_dynamicList : UnityEvent<List<dynamic>>
            {
                public UnityEvent_dynamicList() { }
                public UnityEvent_dynamicList(UnityAction<List<dynamic>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_float : UnityEvent<float>
            {
                public UnityEvent_float() { }
                public UnityEvent_float(UnityAction<float> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_floatArray : UnityEvent<float[]>
            {
                public UnityEvent_floatArray() { }
                public UnityEvent_floatArray(UnityAction<float[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_floatList : UnityEvent<List<float>>
            {
                public UnityEvent_floatList() { }
                public UnityEvent_floatList(UnityAction<List<float>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_int : UnityEvent<int>
            {
                public UnityEvent_int() { }
                public UnityEvent_int(UnityAction<int> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_intArray : UnityEvent<int[]>
            {
                public UnityEvent_intArray() { }
                public UnityEvent_intArray(UnityAction<int[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_intList : UnityEvent<List<int>>
            {
                public UnityEvent_intList() { }
                public UnityEvent_intList(UnityAction<List<int>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_long : UnityEvent<long>
            {
                public UnityEvent_long() { }
                public UnityEvent_long(UnityAction<long> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_longArray : UnityEvent<long[]>
            {
                public UnityEvent_longArray() { }
                public UnityEvent_longArray(UnityAction<long[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_longList : UnityEvent<List<long>>
            {
                public UnityEvent_longList() { }
                public UnityEvent_longList(UnityAction<List<long>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_object : UnityEvent<object>
            {
                public UnityEvent_object() { }
                public UnityEvent_object(UnityAction<object> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_objectArray : UnityEvent<object[]>
            {
                public UnityEvent_objectArray() { }
                public UnityEvent_objectArray(UnityAction<object[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_objectList : UnityEvent<List<object>>
            {
                public UnityEvent_objectList() { }
                public UnityEvent_objectList(UnityAction<List<object>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_sbyte : UnityEvent<sbyte>
            {
                public UnityEvent_sbyte() { }
                public UnityEvent_sbyte(UnityAction<sbyte> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_sbyteArray : UnityEvent<sbyte[]>
            {
                public UnityEvent_sbyteArray() { }
                public UnityEvent_sbyteArray(UnityAction<sbyte[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_sbyteList : UnityEvent<List<sbyte>>
            {
                public UnityEvent_sbyteList() { }
                public UnityEvent_sbyteList(UnityAction<List<sbyte>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_short : UnityEvent<short>
            {
                public UnityEvent_short() { }
                public UnityEvent_short(UnityAction<short> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_shortArray : UnityEvent<short[]>
            {
                public UnityEvent_shortArray() { }
                public UnityEvent_shortArray(UnityAction<short[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_shortList : UnityEvent<List<short>>
            {
                public UnityEvent_shortList() { }
                public UnityEvent_shortList(UnityAction<List<short>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_string : UnityEvent<string>
            {
                public UnityEvent_string() { }
                public UnityEvent_string(UnityAction<string> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_stringArray : UnityEvent<string[]>
            {
                public UnityEvent_stringArray() { }
                public UnityEvent_stringArray(UnityAction<string[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_stringList : UnityEvent<List<string>>
            {
                public UnityEvent_stringList() { }
                public UnityEvent_stringList(UnityAction<List<string>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_uint : UnityEvent<uint>
            {
                public UnityEvent_uint() { }
                public UnityEvent_uint(UnityAction<uint> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_uintArray : UnityEvent<uint[]>
            {
                public UnityEvent_uintArray() { }
                public UnityEvent_uintArray(UnityAction<uint[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_uintList : UnityEvent<List<uint>>
            {
                public UnityEvent_uintList() { }
                public UnityEvent_uintList(UnityAction<List<uint>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_ulong : UnityEvent<ulong>
            {
                public UnityEvent_ulong() { }
                public UnityEvent_ulong(UnityAction<ulong> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_ulongArray : UnityEvent<ulong[]>
            {
                public UnityEvent_ulongArray() { }
                public UnityEvent_ulongArray(UnityAction<ulong[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_ulongList : UnityEvent<List<ulong>>
            {
                public UnityEvent_ulongList() { }
                public UnityEvent_ulongList(UnityAction<List<ulong>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_ushort : UnityEvent<ushort>
            {
                public UnityEvent_ushort() { }
                public UnityEvent_ushort(UnityAction<ushort> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_ushortArray : UnityEvent<ushort[]>
            {
                public UnityEvent_ushortArray() { }
                public UnityEvent_ushortArray(UnityAction<ushort[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_ushortList : UnityEvent<List<ushort>>
            {
                public UnityEvent_ushortList() { }
                public UnityEvent_ushortList(UnityAction<List<ushort>> call) { AddListener(call); }
            }

            #endregion

            #region [ UNITY CLASSES ]

            [Serializable]
            public class UnityEvent_UnityObject : UnityEvent<UnityEngine.Object>
            {
                public UnityEvent_UnityObject() { }
                public UnityEvent_UnityObject(UnityAction<UnityEngine.Object> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_UnityObjectArray : UnityEvent<UnityEngine.Object[]>
            {
                public UnityEvent_UnityObjectArray() { }
                public UnityEvent_UnityObjectArray(UnityAction<UnityEngine.Object[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_UnityObjectList : UnityEvent<List<UnityEngine.Object>>
            {
                public UnityEvent_UnityObjectList() { }
                public UnityEvent_UnityObjectList(UnityAction<List<UnityEngine.Object>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_GameObject : UnityEvent<GameObject>
            {
                public UnityEvent_GameObject() { }
                public UnityEvent_GameObject(UnityAction<GameObject> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_GameObjectArray : UnityEvent<GameObject[]>
            {
                public UnityEvent_GameObjectArray() { }
                public UnityEvent_GameObjectArray(UnityAction<GameObject[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_GameObjectList : UnityEvent<List<GameObject>>
            {
                public UnityEvent_GameObjectList() { }
                public UnityEvent_GameObjectList(UnityAction<List<GameObject>> call) { AddListener(call); }
            }

            [Serializable]
            public class UnityEvent_Graphic : UnityEvent<Graphic>
            {
                public UnityEvent_Graphic() { }
                public UnityEvent_Graphic(UnityAction<Graphic> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_GraphicArray : UnityEvent<Graphic[]>
            {
                public UnityEvent_GraphicArray() { }
                public UnityEvent_GraphicArray(UnityAction<Graphic[]> call) { AddListener(call); }
            }
            [Serializable]
            public class UnityEvent_GraphicList : UnityEvent<List<Graphic>>
            {
                public UnityEvent_GraphicList() { }
                public UnityEvent_GraphicList(UnityAction<List<Graphic>> call) { AddListener(call); }
            }

            #endregion
        }
    }
}
