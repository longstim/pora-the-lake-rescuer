using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StaticLevel : Level
{
    public string Season;
    public int Level;

    public int WhiteQuantity;
    public int RedQuantity;
    public int OrangeQuantity;

    public string GlobalId
    {
        get
        {
            if (Place == "Parbaba")
            {
                return Level.ToString();
            }
            else if (Place == "Batuguru") {
                return (Level + 20).ToString();
            }
            else if (Place == "Parapat")
            {
                return (Level + 40).ToString();
            }
            else if (Place == "Tomok")
            {
                return (Level + 60).ToString();
            }

            return (0).ToString();
        }
    }

    public StaticLevel()
        : base(new Level())
    {

    }

    public StaticLevel(Level level) : base(level)
    {
        
    }

    public StaticLevel(StaticLevel level)
        : base(level)
    {
        
    }

    public StaticLevel(string Season, string Place, int level)
        : base(EntityManager.XMLToInstance<StaticLevel>(Resources.Load<TextAsset>(Season + "/" + Place + "/" + level.ToString()).text))
    {
        this.Season = Season;
        this.Place = Place;
        this.Level = level;
        StaticLevel temp = EntityManager.XMLToInstance<StaticLevel>(Resources.Load<TextAsset>(Season + "/" + Place + "/" + level.ToString()).text);
        this.WhiteQuantity = temp.WhiteQuantity;
        this.RedQuantity = temp.RedQuantity;
        this.OrangeQuantity = temp.OrangeQuantity;

        UserScoreData userScore = UserScoreData.Load();
        this.HighScore = userScore.GetScore(Place, Level);
    }

    /*public StaticLevel(string GlobalId,string Season, string Place, int level)
        : base(EntityManager.XMLToInstance<StaticLevel>(Resources.Load<TextAsset>(Season + "/" + Place + "/" + level.ToString()).text))
    {
        this.GlobalId = GlobalId;
        this.Season = Season;
        this.Place = Place;
        this.Level = level;
        StaticLevel temp = EntityManager.XMLToInstance<StaticLevel>(Resources.Load<TextAsset>(Season + "/" + Place + "/" + level.ToString()).text);
        this.WhiteQuantity = temp.WhiteQuantity;
        this.RedQuantity = temp.RedQuantity;
        this.OrangeQuantity = temp.OrangeQuantity;
        UserScoreData userScore = UserScoreData.Load();
        this.HighScore = userScore.GetScore(Place, Level);
    }*/

    public override void Save()
    {
        UserScoreData userScore = UserScoreData.Load();
        userScore.SetScore(Place, Level, HighScore);
        if (HighScore >= ScoreFor3Star)
        {
            userScore.SetStar(Place, Level, 3);
        }
        else if (HighScore >= ScoreFor2Star)
        {
            userScore.SetStar(Place, Level, 2);
        }
        else if (HighScore > 0)
        {
            userScore.SetStar(Place, Level, 1);
        }
    }

    public void SaveLevel()
    {
        EntityManager.Save<StaticLevel>(this.Name, this);
    }

    public bool IsExist(string Season, string Place, int level)
    {
        return Resources.Load<TextAsset>(Season + "/" + Place + "/" + level.ToString()) != null;
    }
}