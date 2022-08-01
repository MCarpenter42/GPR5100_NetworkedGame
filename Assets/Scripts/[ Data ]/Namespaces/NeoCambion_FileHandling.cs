namespace NeoCambion
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;

    namespace Unity
    {
        namespace IO
        {
            public class FileHandler
            {
                public static void SaveData(object data, string fileSubPath)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    string path = Application.persistentDataPath;
                    if (fileSubPath.Substring(0, 1) != "/")
                    {
                        path += "/";
                    }
                    path += fileSubPath;

                    Debug.Log("Saving data to \"" + path + "\"");
                    FileStream stream = new FileStream(path, FileMode.Create);

                    formatter.Serialize(stream, data);
                    stream.Close();
                }

                public static object LoadData(string fileSubPath)
                {
                    string path = Application.persistentDataPath;
                    if (fileSubPath.Substring(0, 1) != "/")
                    {
                        path += "/";
                    }
                    path += fileSubPath;
                    Debug.Log("Loading data from \"" + path + "\"");
                    if (File.Exists(path))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream(path, FileMode.Open);

                        object obj = formatter.Deserialize(stream);

                        stream.Close();
                        return obj;
                    }
                    else
                    {
                        Debug.LogError("File not found, returning null");
                        return null;
                    }
                }
            }
        }
    }
}