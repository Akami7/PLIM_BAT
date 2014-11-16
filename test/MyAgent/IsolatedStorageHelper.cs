using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;
public static class IsolatedStorageHelper
{
    public static T GetObject<T>(string fileName)
    {
        var localFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
        StreamReader reader = null;

        try
        {
            reader = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, localFileStorage));
            string serializedObject = reader.ReadToEnd();
            reader.Close();
            return Deserialize<T>(serializedObject);

        }
        catch
        {
            return default(T);
        }

        
    }

    public static void SaveObject<T>(string fileName, T objectToSave)
    {
        var localFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
        StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, localFileStorage));

        string serializedObject = Serialize(objectToSave);

        writer.Write(serializedObject);
        writer.Close();
    }

    public static void DeleteObject(string fileName)
    {
        var localFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
        localFileStorage.DeleteFile(fileName);
    }

    private static string Serialize(object objectToSerialize)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
            serializer.WriteObject(ms, objectToSerialize);
            ms.Position = 0;

            using (StreamReader reader = new StreamReader(ms))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private static T Deserialize<T>(string jsonString)
    {
        using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms);
        }
    }

    internal static void Clear()
    {
        var localFileStorage = IsolatedStorageFile.GetUserStoreForApplication();
        localFileStorage.Remove();
        
    }
}