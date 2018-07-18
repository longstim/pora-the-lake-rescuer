using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
class UserItemStock
{
    public static List<string> Items = new List<string>();

    public string DeviceId;
    public List<string> items = new List<string>();

    public UserItemStock()
    {
        DeviceId = SystemInfo.deviceUniqueIdentifier;
    }

    public void Save()
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/userItemStock.dat");

        formater.Serialize(fileStream, this);
        fileStream.Close();
    }

    public void Load()
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream;
        UserItemStock data;
        if (!File.Exists(Application.persistentDataPath + "/userItemStock.dat"))
        {
            fileStream = File.Create(Application.persistentDataPath + "/userItemStock.dat");
            data = new UserItemStock();
            formater.Serialize(fileStream, data);
            fileStream.Close();
        }
        else {
            fileStream = File.Open(Application.persistentDataPath + "/userItemStock.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            data = (UserItemStock)formater.Deserialize(fileStream);
            fileStream.Close();
            if (data.DeviceId != SystemInfo.deviceUniqueIdentifier)
            {
                fileStream = File.Create(Application.persistentDataPath + "/userItemStock.dat");
                formater.Serialize(fileStream, new UserItemStock());
                fileStream.Close();
            }
        }
        this.items = data.items;
        Items.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            Items.Add(items[i]);
        }
    }

    public bool HaveBuy(string alias)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log(items[i]);
        }
        return items.Contains(alias);
    }

    public void Add(string alias)
    {
        items.Add(alias);
        Save();
        Items = items;
    }

    public void Add(string[] aliasess)
    {
        for (int i = 0; i < aliasess.Length; i++)
        {
            items.Add(aliasess[i]);
        }
        Save();
        Items = items;
    }
}