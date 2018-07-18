using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[System.Serializable]
class PoraCustomizationData
{
    public string DeviceId;

    public string CurrentHeadwear;
    public string CurrentBodywear;
    public string CurrentGunwear;

    public PoraCustomizationData()
    {
        DeviceId = SystemInfo.deviceUniqueIdentifier;
    }
}

public class PoraCustomization : MonoBehaviour {

    [System.Serializable]
    public class PoraItem
    {
        public string Name = "";
        public GameObject[] item;
    }

    public GameObject[] HeadwearsDefault;
    public GameObject[] BodywearsDefault;
    public GameObject[] GunwearsDefault;

    public PoraItem[] Headwears;
    public PoraItem[] Bodywears;
    public PoraItem[] Gunswears;

    [HideInInspector]
    public string CurrentHeadwear;
    [HideInInspector]
    public string CurrentBodywear;
    [HideInInspector]
    public string CurrentGunwear;

	// Use this for initialization
	void Start () {
        Load();
        Refresh();
	}

    public void Save()
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath +"/profileInfo.dat");
        PoraCustomizationData data = new PoraCustomizationData();
        data.CurrentHeadwear = CurrentHeadwear;
        data.CurrentBodywear = CurrentBodywear;
        data.CurrentGunwear = CurrentGunwear;
        formater.Serialize(fileStream,data);
        fileStream.Close();
    }
    public void Load()
    {
        BinaryFormatter formater = new BinaryFormatter();
        PoraCustomizationData data;
        FileStream fileStream;
        if (!File.Exists(Application.persistentDataPath + "/profileInfo.dat"))
        {
            data = new PoraCustomizationData();
            data.CurrentHeadwear = "";
            data.CurrentBodywear = "";
            data.CurrentGunwear = "";
            fileStream = File.Create(Application.persistentDataPath + "/profileInfo.dat");
            formater.Serialize(fileStream, data);
            fileStream.Close();
        }
        else {
            fileStream = File.Open(Application.persistentDataPath + "/profileInfo.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            data = (PoraCustomizationData)formater.Deserialize(fileStream);
            fileStream.Close();
            if (data.DeviceId != SystemInfo.deviceUniqueIdentifier)
            {
                data = new PoraCustomizationData();
                data.CurrentHeadwear = "";
                data.CurrentBodywear = "";
                data.CurrentGunwear = "";
                fileStream = File.Create(Application.persistentDataPath + "/profileInfo.dat");
                formater.Serialize(fileStream, data);
                fileStream.Close();
            }
        }

        CurrentHeadwear = data.CurrentHeadwear;
        CurrentBodywear = data.CurrentBodywear;
        CurrentGunwear = data.CurrentGunwear;
    }

    public void UseItemHeadwear(string ItemId)
    {
        CurrentHeadwear = ItemId;
        Refresh();
        Save();
    }

    public void UseItemBodywear(string ItemId)
    {
        CurrentBodywear = ItemId;
        Refresh();
        Save();
    }

    public void UseItemGunwear(string ItemId)
    {
        CurrentGunwear = ItemId;
        Refresh();
        Save();
    }

    public void Refresh()
    {
        DisableAll();
        bool useDefaultHeadewar = true;
        bool useDefaultBodywear = true;
        bool useDefaultGunwear = true;

        for (int i = 0; i < Headwears.Length; i++)
        {
            if (Headwears[i].Name == CurrentHeadwear)
            {
                for (int j = 0; j < Headwears[i].item.Length; j++)
                {
                    Headwears[i].item[j].SetActive(true);
                }
                useDefaultHeadewar = false;
                break;
            }
        }

        for (int i = 0; i < Bodywears.Length; i++)
        {
            if (Bodywears[i].Name == CurrentBodywear)
            {
                for (int j = 0; j < Bodywears[i].item.Length; j++)
                {
                    Bodywears[i].item[j].SetActive(true);
                }
                useDefaultBodywear = false;
                break;
            }
        }

        for (int i = 0; i < Gunswears.Length; i++)
        {
            if (Gunswears[i].Name == CurrentGunwear)
            {
                for (int j = 0; j < Gunswears[i].item.Length; j++)
                {
                    Gunswears[i].item[j].SetActive(true);
                }
                useDefaultGunwear = false;
                break;
            }
        }

        if (useDefaultHeadewar)
        {
            for (int j = 0; j < HeadwearsDefault.Length; j++)
            {
                HeadwearsDefault[j].SetActive(true);
            }
        }
        if (useDefaultBodywear)
        {
            for (int j = 0; j < BodywearsDefault.Length; j++)
            {
                BodywearsDefault[j].SetActive(true);
            }
        }
        if (useDefaultGunwear)
        {
            for (int j = 0; j < GunwearsDefault.Length; j++)
            {
                GunwearsDefault[j].SetActive(true);
            }
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < Headwears.Length; i++)
        {
            for (int j = 0; j < Headwears[i].item.Length; j++)
            {
                Headwears[i].item[j].SetActive(false);
            }
        }

        for (int i = 0; i < Bodywears.Length; i++)
        {
            for (int j = 0; j < Bodywears[i].item.Length; j++)
            {
                Bodywears[i].item[j].SetActive(false);
            }
        }

        for (int i = 0; i < Gunswears.Length; i++)
        {
            for (int j = 0; j < Gunswears[i].item.Length; j++)
            {
                Gunswears[i].item[j].SetActive(false);
            }
        }

        for (int j = 0; j < GunwearsDefault.Length; j++)
        {
            GunwearsDefault[j].SetActive(false);
        }

        for (int j = 0; j < BodywearsDefault.Length; j++)
        {
            BodywearsDefault[j].SetActive(false);
        }

        for (int j = 0; j < HeadwearsDefault.Length; j++)
        {
            HeadwearsDefault[j].SetActive(false);
        }
    }
}
