using System.IO;
using System.IO.IsolatedStorage;
//using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;
public static class IsolatedStorageHelper
{
    public static T GetObject<T>(string key)
    {
        var localSettings = IsolatedStorageSettings.ApplicationSettings;
        
        if (localSettings.Contains(key))
        {
            string serializedObject = localSettings[key].ToString();
            return Deserialize<T>(serializedObject);
        }

        return default(T);
    }

    public static void SaveObject<T>(string key, T objectToSave)
    {
        var localSettings = IsolatedStorageSettings.ApplicationSettings;
        string serializedObject = Serialize(objectToSave);
        localSettings[key] = serializedObject;
    }

    public static void DeleteObject(string key)
    {
        var localSettings = IsolatedStorageSettings.ApplicationSettings;
        localSettings.Remove(key);
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
        var localSettings = IsolatedStorageSettings.ApplicationSettings;
        localSettings.Clear();
        
    }
}