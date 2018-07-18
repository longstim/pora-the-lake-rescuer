using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class EntityManager
{

    public static void SaveToBinary<T>(string name, T instance)
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/"+name+".dat");
        formater.Serialize(fileStream, instance);
        fileStream.Close();
    }

    public static T LoadFromBinary<T>(string name)
    {
        if (!File.Exists(Application.persistentDataPath + "/" + name + ".dat"))
        {
            return default(T);
        }
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/" + name + ".dat", FileMode.Open);
        T data = (T)formater.Deserialize(fileStream);
        fileStream.Close();
        return data;
    }

    public static void Save<T>(string name, T instance)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
#if UNITY_STANDALONE || UNITY_EDITOR
        using (var stream = new FileStream(Path.Combine(Application.persistentDataPath, name+".xml"), FileMode.Create))
        {
            serializer.Serialize(stream, instance);
        }
#else
        using (var ms = new MemoryStream ()) {
            serializer.Serialize (ms, instance);
            Debug.Log(System.Text.ASCIIEncoding.ASCII.GetString (ms.ToArray ()));
            PlayerPrefs.SetString (name, System.Text.ASCIIEncoding.ASCII.GetString (ms.ToArray ()));
        }
#endif
    }



    public static T Load<T>(string name)
    {
        T instance;
#if UNITY_STANDALONE || UNITY_EDITOR
        StreamReader streamReader;
        if (!name.Contains("/"))
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, name))) return default(T);
                
            streamReader = File.OpenText(Path.Combine(Application.persistentDataPath, name));
        }else{
            if (!File.Exists(name)) return default(T);

            streamReader = File.OpenText(name);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (var ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(streamReader.ReadToEnd())))
        {
            instance = (T)serializer.Deserialize(ms);
        }
        streamReader.Close();
#else
        if (!PlayerPrefs.HasKey(name)) return default(T);

        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (var ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(PlayerPrefs.GetString(name))))
        {
            instance = (T)serializer.Deserialize(ms);
        }
#endif
        return instance;
    }

    public static T XMLToInstance<T>(string data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(new StringReader(data));
    }

}