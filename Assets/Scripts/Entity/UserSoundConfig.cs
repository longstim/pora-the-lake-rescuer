using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class UserSoundConfig{

    public string DeviceID;
    public float MusicVolume = 1;
    public float SoundVolume = 1;
    public bool MusicStatus = true;
    public bool SoundStatus = true;

    public UserSoundConfig()
    {
        DeviceID = SystemInfo.deviceUniqueIdentifier;
    }

    public void Save()
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/userSound.dat");
        formater.Serialize(fileStream, this);
        fileStream.Close();
    }

    public static UserSoundConfig Load()
    {
        BinaryFormatter formater = new BinaryFormatter();
        UserSoundConfig data;
        FileStream fileStream;
        if (!File.Exists(Application.persistentDataPath + "/userSound.dat"))
        {
            data = new UserSoundConfig();
            fileStream = File.Create(Application.persistentDataPath + "/userSound.dat");
            formater.Serialize(fileStream, data);
            fileStream.Close();
        }
        else
        {
            fileStream = File.Open(Application.persistentDataPath + "/userSound.dat", FileMode.Open);
            data = (UserSoundConfig)formater.Deserialize(fileStream);
            fileStream.Close();
            if (data.DeviceID != SystemInfo.deviceUniqueIdentifier)
            {
                fileStream = File.Create(Application.persistentDataPath + "/userSound.dat");
                data = new UserSoundConfig();
                formater.Serialize(fileStream, data);
                fileStream.Close();
            }
        }
        return data;
    }
}
