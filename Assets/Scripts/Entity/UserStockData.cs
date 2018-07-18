using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
class UserStockData
{
    const int InitialGems = 50;
    const int InitialGold = 1000;
    const int InitialWhiteBubble = 0;
    const int InitialRedBubble = 0;
    const int InitialOrangeBubble = 0;

    public static int GemsStock;
    public static int GoldStock;
    public static int WhiteBubbleStock;
    public static int RedBubbleStock;
    public static int OrangeBubbleStock;

    public string DeviceId;

    public int Gems;
    public int Gold;
    public int WhiteBubble;
    public int RedBubble;
    public int OrangeBubble;

    public UserStockData()
    {
        DeviceId = SystemInfo.deviceUniqueIdentifier;
    }

    public void Save()
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/userStock.dat");
        formater.Serialize(fileStream, this);
        fileStream.Close();

        GemsStock = this.Gems;
        GoldStock = this.Gold;
        WhiteBubbleStock = this.WhiteBubble;
        RedBubbleStock = this.RedBubble;
        OrangeBubbleStock = this.OrangeBubble;
    }

    public static UserStockData Load()
    {
        BinaryFormatter formater = new BinaryFormatter();
        UserStockData data;
        FileStream fileStream;
        if (!File.Exists(Application.persistentDataPath + "/userStock.dat"))
        {
            fileStream = File.Create(Application.persistentDataPath + "/userStock.dat");
            data = new UserStockData();
            data.Gems = InitialGems;
            data.Gold = InitialGold;
            data.WhiteBubble = InitialWhiteBubble;
            data.RedBubble = InitialOrangeBubble;
            data.OrangeBubble = InitialOrangeBubble;
            formater.Serialize(fileStream, data);
            fileStream.Close();
        }
        else
        {
            fileStream = File.Open(Application.persistentDataPath + "/userStock.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            data = (UserStockData)formater.Deserialize(fileStream);
            fileStream.Close();
            if (data.DeviceId != SystemInfo.deviceUniqueIdentifier)
            {
                formater = new BinaryFormatter();
                fileStream = File.Create(Application.persistentDataPath + "/userStock.dat");
                data = new UserStockData();
                formater.Serialize(fileStream, data);
                fileStream.Close();
            }
        }
        GemsStock = data.Gems;
        GoldStock = data.Gold;
        WhiteBubbleStock = data.WhiteBubble;
        RedBubbleStock = data.RedBubble;
        OrangeBubbleStock = data.OrangeBubble;

        return data;
    }

    public bool PlusMinGem(int gem)
    {
        Load();
        if (gem + Gems < 0)
        {
            return false;
        }

        Gems = Gems + gem;
        Save();
        return true;
    }

    public bool PlusMinGold(int gold)
    {
        Load();
        if (Gold + gold < 0)
        {
            return false;
        }

        Gold = Gold + gold;
        Save();
        return true;
    }

    public bool PlusMinBubble(int white,int red,int orange)
    {
        Load();
        if (WhiteBubble + white < 0 || RedBubble + red < 0 || OrangeBubble + orange < 0)
        {
            return false;
        }

        WhiteBubble = WhiteBubble + white;
        RedBubble = RedBubble + red;
        OrangeBubble = OrangeBubble + orange;
        Save();
        return true;
    }
}