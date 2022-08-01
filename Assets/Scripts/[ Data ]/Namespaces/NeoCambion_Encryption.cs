namespace NeoCambion
{
    using System;
    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.Security.Cryptography;
    using UnityEngine;

    namespace Encryption
    {
        public enum EncryptionType { AES, DES, XOR };

        public static class EncryptionHandler
        {
            public static EncryptionType DefaultType = EncryptionType.AES;

            public static Encryption_AES AES = new Encryption_AES();
            public static Encryption_DES DES = new Encryption_DES();
            public static Encryption_XOR XOR = new Encryption_XOR();

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public static void SetAllDefaults(string aesKey, string aesIV, string desKey, int xorKey)
            {
                AES.SetDefaults(aesKey, aesIV);
                DES.SetDefault(desKey);
                XOR.SetDefault(xorKey);
            }

            public static void RandomiseAllDefaults()
            {
                AES.SetRandomDefaults();
                DES.SetRandomDefault();
                XOR.SetRandomDefault();
            }

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public static string Encrypt(string data)
            {
                return Encrypt(data, DefaultType);
            }

            public static string Encrypt(string data, EncryptionType encType)
            {
                switch (encType)
                {
                    default:
                    case EncryptionType.AES:
                        return AES.Encrypt(data);

                    case EncryptionType.DES:
                        return DES.Encrypt(data);

                    case EncryptionType.XOR:
                        return XOR.EncryptDecrypt(data);
                }
            }
            
            public static EncryptedObject Encrypt(object obj)
            {
                return Encrypt(obj, DefaultType);
            }

            public static EncryptedObject Encrypt(object obj, EncryptionType encType)
            {
                try
                {
                    switch (encType)
                    {
                        default:
                        case EncryptionType.AES:
                            return AES.Encrypt(obj);

                        case EncryptionType.DES:
                            return DES.Encrypt(obj);

                        case EncryptionType.XOR:
                            return XOR.Encrypt(obj);
                    }
                }
                catch
                {
                    throw new Exception("ERROR: Cannot encrypt object \"" + obj + "\", as it is a non-serializable type! <" + obj.GetType().ToString() + ">");
                }
            }

            public static string Decrypt(string data)
            {
                return Decrypt(data, DefaultType);
            }

            public static string Decrypt(string data, EncryptionType encType)
            {
                switch (encType)
                {
                    default:
                    case EncryptionType.AES:
                        return AES.Decrypt(data);

                    case EncryptionType.DES:
                        return DES.Decrypt(data);

                    case EncryptionType.XOR:
                        return XOR.EncryptDecrypt(data);
                }
            }
            
            public static T Decrypt<T>(EncryptedObject encObj)
            {
                return Decrypt<T>(encObj, DefaultType);
            }

            public static T Decrypt<T>(EncryptedObject encObj, EncryptionType encType)
            {
                switch (encType)
                {
                    default:
                    case EncryptionType.AES:
                        return AES.Decrypt<T>(encObj);

                    case EncryptionType.DES:
                        return DES.Decrypt<T>(encObj);

                    case EncryptionType.XOR:
                        return XOR.Decrypt<T>(encObj);
                }
            }

            public static T TypedObjectFromJson<T>(string dataString)
            {
                return JsonUtility.FromJson<T>(dataString);
            }
        }

        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

        public class Encryption_AES
        {
            private string keyDefault = "A60A5770FE5E7AB200BA9CFC94E4E8B0"; //set any string of 32 chars
            private byte[] keyDefaultBytes = new byte[256]; //set any string of 32 chars
            private string ivDefault = "1234567887654321"; //set any string of 16 chars
            private byte[] ivDefaultBytes = new byte[128]; //set any string of 16 chars

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public Encryption_AES()
            {
                keyDefaultBytes = Encoding.ASCII.GetBytes(keyDefault);
                ivDefaultBytes = Encoding.ASCII.GetBytes(ivDefault);
            }

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public void SetDefaults(string keyDefaultNew, string ivDefaultNew)
            {
                keyDefault = keyDefaultNew;
                keyDefaultBytes = Encoding.ASCII.GetBytes(keyDefaultNew);
                ivDefault = ivDefaultNew;
                ivDefaultBytes = Encoding.ASCII.GetBytes(ivDefaultNew);
            }

            public void SetRandomDefaults()
            {
                SetDefaults(GenerateRandomKey(), GenerateRandomIV());
            }

            public string[] GetDefaults()
            {
                return new string[] { keyDefault, ivDefault };
            }

            public string GenerateRandomKey()
            {
                string randKey = "";

                char[] alphaNumeric = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
                for (int i = 0; i < 32; i++)
                {
                    randKey += alphaNumeric[UnityEngine.Random.Range(0, alphaNumeric.Length)];
                }

                return randKey;
            }

            public string GenerateRandomIV()
            {
                string randIV = "";

                char[] alphaNumeric = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
                for (int i = 0; i < 16; i++)
                {
                    randIV += alphaNumeric[UnityEngine.Random.Range(0, alphaNumeric.Length)];
                }

                return randIV;
            }

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public string Encrypt(string input)
            {
                return Encrypt(input, keyDefaultBytes, ivDefaultBytes);
            }

            public string Encrypt(string input, string key, string iv)
            {
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                return Encrypt(input, keyBytes, ivBytes);
            }

            public string Encrypt(string input, byte[] key, byte[] iv)
            {
                AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
                aesProvider.BlockSize = 128;
                aesProvider.KeySize = 256;
                aesProvider.Key = key;
                aesProvider.IV = iv;
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.Padding = PaddingMode.PKCS7;

                byte[] textBytes = Encoding.ASCII.GetBytes(input);
                ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor(aesProvider.Key, aesProvider.IV);

                byte[] result = cryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                return Convert.ToBase64String(result);
            }

            public EncryptedObject Encrypt(object obj)
            {
                return Encrypt(obj, keyDefaultBytes, ivDefaultBytes);
            }

            public EncryptedObject Encrypt(object obj, string key, string iv)
            {
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                return Encrypt(obj, keyBytes, ivBytes);
            }

            public EncryptedObject Encrypt(object obj, byte[] key, byte[] iv)
            {
                string objType = obj.GetType().ToString();
                string data = JsonUtility.ToJson(obj);

                AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
                aesProvider.BlockSize = 128;
                aesProvider.KeySize = 256;
                aesProvider.Key = key;
                aesProvider.IV = iv;
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.Padding = PaddingMode.PKCS7;

                byte[] textBytes = Encoding.ASCII.GetBytes(data);
                ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor(aesProvider.Key, aesProvider.IV);

                byte[] byteData = cryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                return new EncryptedObject(objType, Convert.ToBase64String(byteData));
            }

            public string Decrypt(string input)
            {
                return Decrypt(input, keyDefaultBytes, ivDefaultBytes);
            }

            public string Decrypt(string input, string key, string iv)
            {
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                return Decrypt(input, keyBytes, ivBytes);
            }

            public string Decrypt(string input, byte[] key, byte[] iv)
            {
                AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
                aesProvider.BlockSize = 128;
                aesProvider.KeySize = 256;
                aesProvider.Key = key;
                aesProvider.IV = iv;
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.Padding = PaddingMode.PKCS7;

                byte[] textBytes = Convert.FromBase64String(input);
                ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor();

                byte[] result = cryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                return Encoding.ASCII.GetString(result);
            }

            public T Decrypt<T>(EncryptedObject encObj)
            {
                return Decrypt<T>(encObj, keyDefaultBytes, ivDefaultBytes);
            }

            public T Decrypt<T>(EncryptedObject encObj, string key, string iv)
            {
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                return Decrypt<T>(encObj, keyBytes, ivBytes);
            }

            public T Decrypt<T>(EncryptedObject encObj, byte[] key, byte[] iv)
            {
                AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
                aesProvider.BlockSize = 128;
                aesProvider.KeySize = 256;
                aesProvider.Key = key;
                aesProvider.IV = iv;
                aesProvider.Mode = CipherMode.CBC;
                aesProvider.Padding = PaddingMode.PKCS7;

                byte[] textBytes = Convert.FromBase64String(encObj.dataString);
                ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor();

                byte[] objData = cryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                string dataString = Encoding.ASCII.GetString(objData);

                MethodInfo method = typeof(EncryptionHandler).GetMethod(nameof(EncryptionHandler.TypedObjectFromJson));
                MethodInfo generic = method.MakeGenericMethod(Type.GetType(encObj.objectType));
                object[] args = { dataString };

                return (T)generic.Invoke(null, args);
            }
        }

        public class Encryption_DES
        {
            private string keyDefault = "fY8k0Wn2";
            private byte[] keyDefaultBytes = new byte[64];

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public Encryption_DES()
            {
                keyDefaultBytes = Encoding.ASCII.GetBytes(keyDefault);
            }

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public void SetDefault(string keyDefaultNew)
            {
                keyDefault = keyDefaultNew;
                keyDefaultBytes = Encoding.ASCII.GetBytes(keyDefaultNew);
            }

            public void SetRandomDefault()
            {
                SetDefault(GenerateRandomKey());
            }

            public string GetDefault()
            {
                return keyDefault;
            }

            public string GenerateRandomKey()
            {
                string randKey = "";

                char[] alphaNumeric = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
                for (int i = 0; i < 8; i++)
                {
                    randKey += alphaNumeric[UnityEngine.Random.Range(0, alphaNumeric.Length)];
                }

                return randKey;
            }

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public string Encrypt(string input)
            {
                return Encrypt(input, keyDefault);
            }

            public string Encrypt(string input, string key)
            {
                byte[] textBytes = Encoding.ASCII.GetBytes(input);
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                ICryptoTransform cryptoTransform = desProvider.CreateEncryptor(keyBytes, keyBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                //Set up Stream & Write Encript data
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, cryptoTransform, mode);
                cStream.Write(textBytes, 0, textBytes.Length);
                cStream.FlushFinalBlock();

                //Read Ecncrypted Data From Memory Stream
                byte[] result = new byte[mStream.Length];
                mStream.Position = 0;
                mStream.Read(result, 0, result.Length);

                return Convert.ToBase64String(result);
            }

            public EncryptedObject Encrypt(object obj)
            {
                return Encrypt(obj, keyDefault);
            }

            public EncryptedObject Encrypt(object obj, string key)
            {
                string objType = obj.GetType().ToString();
                string data = JsonUtility.ToJson(obj);

                byte[] textBytes = Encoding.ASCII.GetBytes(data);
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                ICryptoTransform cryptoTransform = desProvider.CreateEncryptor(keyBytes, keyBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                //Set up Stream & Write Encript data
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, cryptoTransform, mode);
                cStream.Write(textBytes, 0, textBytes.Length);
                cStream.FlushFinalBlock();

                //Read Ecncrypted Data From Memory Stream
                byte[] byteData = new byte[mStream.Length];
                mStream.Position = 0;
                mStream.Read(byteData, 0, byteData.Length);

                return new EncryptedObject(objType, Convert.ToBase64String(byteData));
            }

            public string Decrypt(string input)
            {
                return Decrypt(input, keyDefault);
            }

            public string Decrypt(string input, string key)
            {
                byte[] textBytes = Convert.FromBase64String(input);
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                ICryptoTransform cryptoTransform = desProvider.CreateDecryptor(keyBytes, keyBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                //Set up Stream & Write Encript data
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, cryptoTransform, mode);
                cStream.Write(textBytes, 0, textBytes.Length);
                cStream.FlushFinalBlock();

                //Read Ecncrypted Data From Memory Stream
                byte[] result = new byte[mStream.Length];
                mStream.Position = 0;
                mStream.Read(result, 0, result.Length);

                return Encoding.ASCII.GetString(result);
            }

            public T Decrypt<T>(EncryptedObject encObj)
            {
                return Decrypt<T>(encObj, keyDefault);
            }

            public T Decrypt<T>(EncryptedObject encObj, string key)
            {
                byte[] textBytes = Convert.FromBase64String(encObj.dataString);
                byte[] keyBytes = Encoding.ASCII.GetBytes(key);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                ICryptoTransform cryptoTransform = desProvider.CreateDecryptor(keyBytes, keyBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                //Set up Stream & Write Encript data
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, cryptoTransform, mode);
                cStream.Write(textBytes, 0, textBytes.Length);
                cStream.FlushFinalBlock();

                //Read Ecncrypted Data From Memory Stream
                byte[] objData = new byte[mStream.Length];
                mStream.Position = 0;
                mStream.Read(objData, 0, objData.Length);
                string dataString = Encoding.ASCII.GetString(objData);

                MethodInfo method = typeof(EncryptionHandler).GetMethod(nameof(EncryptionHandler.TypedObjectFromJson));
                MethodInfo generic = method.MakeGenericMethod(Type.GetType(encObj.objectType));
                object[] args = { dataString };

                return (T)generic.Invoke(this, args);
            }
        }

        public class Encryption_XOR
        {
            private int keyDefault = 80714292;

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public void SetDefault(int keyDefaultNew)
            {
                keyDefault = keyDefaultNew;
            }

            public void SetRandomDefault()
            {
                keyDefault = GenerateRandomKey();
            }

            public int GetDefault()
            {
                return keyDefault;
            }

            public int GenerateRandomKey()
            {
                return UnityEngine.Random.Range(0, (int)short.MaxValue);
            }

            public string EncryptDecrypt(string input)
            {
                return EncryptDecrypt(input, keyDefault);
            }

            public string EncryptDecrypt(string input, int key)
            {
                StringBuilder output = new StringBuilder(input.Length);
                for (int i = 0; i < input.Length; i++)
                {
                    char ch = (char)(input[i] ^ key);
                    output.Append(ch);
                }
                return output.ToString();
            }

            public EncryptedObject Encrypt(object obj)
            {
                return Encrypt(obj, keyDefault);
            }

            public EncryptedObject Encrypt(object obj, int key)
            {
                string objType = obj.GetType().ToString();
                string data = JsonUtility.ToJson(obj);

                StringBuilder output = new StringBuilder(data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    char ch = (char)(data[i] ^ key);
                    output.Append(ch);
                }

                return new EncryptedObject(objType, output.ToString());
            }

            public T Decrypt<T>(EncryptedObject encObj)
            {
                return Decrypt<T>(encObj, keyDefault);
            }

            public T Decrypt<T>(EncryptedObject encObj, int key)
            {
                int l = encObj.dataString.Length;

                StringBuilder dataString = new StringBuilder(l);
                for (int i = 0; i < l; i++)
                {
                    char ch = (char)(encObj.dataString[i] ^ key);
                    dataString.Append(ch);
                }

                MethodInfo method = typeof(EncryptionHandler).GetMethod(nameof(EncryptionHandler.TypedObjectFromJson));
                MethodInfo generic = method.MakeGenericMethod(Type.GetType(encObj.objectType));
                object[] args = { dataString };

                return (T)generic.Invoke(this, args);
            }
        }

        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

        [Serializable]
        public class EncryptedObject
        {
            public string objectType;
            public string dataString;

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public EncryptedObject()
            { }

            public EncryptedObject(string objectType, string dataString)
            {
                this.objectType = objectType;
                this.dataString = dataString;
            }

            /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

            public string json
            {
                get
                {
                    return JsonUtility.ToJson(this);
                }
            }
        }
    }
}