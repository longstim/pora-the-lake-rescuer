using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Facebook.MiniJSON;

[System.Serializable]
public class UserScoreData
{
    public delegate void CallbackChangeScore(string globalId, int score);

    public static event CallbackChangeScore OnScoreChange;

    public string IdDevice;

    public int[] ParbabaStar = new int[20];
    public int[] BatuguruStar = new int[20];
    public int[] ParapatStar = new int[20];
    public int[] TomokStar = new int[20];

    public int[] ParbabaScore = new int[20];
    public int[] BatuguruScore = new int[20];
    public int[] ParapatScore = new int[20];
    public int[] TomokScore = new int[20];


    public int GetScore(string Place, int Level)
    {
        if (Place == "Parbaba")
        {
            return ParbabaScore[Level - 1];
        }
        else if (Place == "Batuguru")
        {
            return BatuguruScore[Level - 1];
        }
        else if (Place == "Parapat")
        {
            return ParapatScore[Level - 1];
        }
        else if (Place == "Tomok")
        {
            return TomokScore[Level - 1];
        }
        return 0;
    }

    public int GetPlaceStar(string Place)
    {
        int temp = 0;
        if (Place == "Parbaba")
        {
            for (int i = 0; i < ParbabaStar.Length; i++)
            {
                temp += ParbabaStar[i];
            }
        }
        else if (Place == "Batuguru")
        {
            for (int i = 0; i < BatuguruStar.Length; i++)
            {
                temp += BatuguruStar[i];
            }
        }
        else if (Place == "Parapat")
        {
            for (int i = 0; i < ParapatStar.Length; i++)
            {
                temp += ParapatStar[i];
            }
        }
        else if (Place == "Tomok")
        {
            for (int i = 0; i < TomokStar.Length; i++)
            {
                temp += TomokStar[i];
            }
        }
        return temp;
    }

    public void SetStar(string Place, int Level, int Star)
    {
        if (Place == "Parbaba" && ParbabaStar[Level - 1] < Star)
        {
            ParbabaStar[Level - 1] = Star;
        }
        else if (Place == "Batuguru" && BatuguruStar[Level - 1] < Star)
        {
            BatuguruStar[Level - 1] = Star;
        }
        else if (Place == "Parapat" && ParapatStar[Level - 1] < Star)
        {
            ParapatStar[Level - 1] = Star;
        }
        else if (Place == "Tomok" && TomokStar[Level - 1] < Star)
        {
            TomokStar[Level - 1] = Star;
        }
        Save();
    }

    public int GetStar(string Place, int Level)
    {
        if (Place == "Parbaba")
        {
            return ParbabaStar[Level - 1] ;
        }
        else if (Place == "Batuguru")
        {
            return BatuguruStar[Level - 1];
        }
        else if (Place == "Parapat")
        {
            return ParapatStar[Level - 1];
        }
        else if (Place == "Tomok")
        {
            return TomokStar[Level - 1];
        }
        return 0;
    }

    public void SetScore(string Place, int Level, int Score)
    {
        if (Place == "Parbaba" && ParbabaScore[Level - 1] < Score)
        {
            ParbabaScore[Level - 1] = Score;
            if (OnScoreChange != null)
                OnScoreChange((Level).ToString(), Score);
        }
        else if (Place == "Batuguru" && BatuguruScore[Level - 1] < Score)
        {
            BatuguruScore[Level - 1] = Score;
            if (OnScoreChange != null)
                OnScoreChange((Level+20).ToString(),Score);
        }
        else if (Place == "Parapat" && ParapatScore[Level - 1] < Score)
        {
            ParapatScore[Level - 1] = Score;
            if (OnScoreChange != null)
                OnScoreChange((Level + 40).ToString(), Score);
        }
        else if (Place == "Tomok" && TomokScore[Level - 1] < Score)
        {
            TomokScore[Level - 1] = Score;
            if (OnScoreChange != null)
                OnScoreChange((Level + 60).ToString(), Score);
        }
        Save();
    }

    public void RenewScore(int globalLevelId, int Score)
    {
        if (globalLevelId <= 20)
        {
            SetScore("Parbaba", globalLevelId, Score);
        }
        else if (globalLevelId <= 40)
        {
            SetScore("Batuguru", globalLevelId - 20, Score);
        }
        else if (globalLevelId <= 60)
        {
            SetScore("Batuguru", globalLevelId - 20 - 20, Score);
        }
        else if (globalLevelId <= 80)
        {
            SetScore("Batuguru", globalLevelId - 20 - 20 - 20, Score);
        }
    }

    public UserScoreData()
    {
        IdDevice = SystemInfo.deviceUniqueIdentifier;
    }

    public void Save()
    {
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/userScore.dat");
        formater.Serialize(fileStream, this);
        fileStream.Close();
    }

    public static UserScoreData Load()
    {
        BinaryFormatter formater = new BinaryFormatter();
        UserScoreData data;
        FileStream fileStream;
        if (!File.Exists(Application.persistentDataPath + "/userScore.dat"))
        {
            fileStream = File.Create(Application.persistentDataPath + "/userScore.dat");
            data = new UserScoreData();
            formater.Serialize(fileStream, data);
            fileStream.Close();
        }
        else
        {
            fileStream = File.Open(Application.persistentDataPath + "/userScore.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            data = (UserScoreData)formater.Deserialize(fileStream);
            fileStream.Close();
            if (data.IdDevice != SystemInfo.deviceUniqueIdentifier)
            {
                FileStream fileStream2 = File.Create(Application.persistentDataPath + "/userScore.dat");
                data = new UserScoreData();
                formater.Serialize(fileStream2, data);
                fileStream2.Close();
            }
        }

        return data;
    }

    public string SerializeToJsonUsingGlobalId()
    {
        UserScoreData userScore = UserScoreData.Load();
        List<Dictionary<string, string>> scores = new List<Dictionary<string, string>>();
        int i = 1;
        bool finish = false;
        while (i <= 20 && !finish)
        {
            if (userScore.ParbabaScore[i - 1] != 0)
            {
                Dictionary<string, string> score = new Dictionary<string, string>();
                score.Add("level", i + "");
                score.Add("score", userScore.ParbabaScore[i - 1] + "");
                scores.Add(score);
            }
            else
            {
                finish = true;
            }
            i++;
        }

        while (i <= 40 && !finish)
        {
            if (userScore.BatuguruScore[i - 21] != 0)
            {
                Dictionary<string, string> score = new Dictionary<string, string>();
                score.Add("level", i + "");
                score.Add("score", userScore.BatuguruScore[i - 21] + "");
                scores.Add(score);
            }
            else
            {
                finish = true;
            }
            i++;
        }

        while (i <= 60 && !finish)
        {
            if (userScore.ParapatScore[i - 41] != 0)
            {
                Dictionary<string, string> score = new Dictionary<string, string>();
                score.Add("level", i + "");
                score.Add("score", userScore.ParapatScore[i - 41] + "");
                scores.Add(score);
            }
            else
            {
                finish = true;
            }
            i++;
        }

        while (i <= 80 && !finish)
        {
            if (userScore.TomokScore[i - 61] != 0)
            {
                Dictionary<string, string> score = new Dictionary<string, string>();
                score.Add("level", i + "");
                score.Add("score", userScore.TomokScore[i - 61] + "");
                scores.Add(score);
            }
            else
            {
                finish = true;
            }
            i++;
        };
        return Json.Serialize(scores);
    }
}