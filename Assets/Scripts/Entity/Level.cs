using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class Level{

    private const string DataKey = "user_levels";

	public const int CurrentVersion = 1;

    private string globalId;
    public string GlobalId
    {
        get
        {
            return globalId;
        }
        set
        {
            globalId = value;
        }
    }

	private string name;
	public string Name {
		get {
			return name;
		}
		set {
			name = value;
		}
	}

    public string Place;

	private string creator;
	public string Creator {
		get {
			return creator;
		}
		set {
			creator = value;
		}
	}
	
	private DateTime createdDate;
	public DateTime CreatedDate {
		get {
			return createdDate.ToLocalTime();
		}
		set {
			createdDate = value;
		}
	}

	public DateTime CreatedDateInUtc {
		get {
			return createdDate;
		}
		set {
			createdDate = value;
		}
	}
	
	private int version;
	public int Version {
		get {
			return version;
		}
		set {
			version = value;
		}
	}
	
	private List<Sampah> rubbishes;
	public List<Sampah> Rubbishes {
		get {
			return rubbishes;
		}
		set {
			rubbishes = value;
		}
	}
	
	private List<Penghalang> barriers;
	public List<Penghalang> Barriers {
		get {
			return barriers;
		}
		set {
			barriers = value;
		}
	}
	
	private List<Gelembung> bubbles;
	public List<Gelembung> Bubbles {
		get {
			return bubbles;
		}
		set {
			bubbles = value;
		}
	}

	private int highScore;
	public int HighScore {
		get {
			return highScore;
		}
		set {
			highScore = value;
		}
	}

	private int scoreFor1Star;
	public int ScoreFor1Star {
		get {
			return scoreFor1Star;
		}
		set {
			scoreFor1Star = value;
		}
	}

	private int scoreFor2Star;
	public int ScoreFor2Star {
		get {
			return scoreFor2Star;
		}
		set {
			scoreFor2Star = value;
		}
	}

	private int scoreFor3Star;
	public int ScoreFor3Star {
		get {
			return scoreFor3Star;
		}
		set {
			scoreFor3Star = value;
		}
	}

	public Level(){
		this.Name = "";
		Rubbishes = new List<Sampah> ();
		Barriers = new List<Penghalang> ();
		Bubbles = new List<Gelembung> ();
	}

    public Level(Level level)
    {
        this.Name = level.Name;
        this.Place = level.Place;
        this.Creator = level.Creator;
        this.CreatedDate = level.CreatedDate;
        this.Version = level.Version;
        this.Rubbishes = level.Rubbishes;
        this.Barriers = level.Barriers;
        this.Bubbles = level.Bubbles;
        this.HighScore = level.highScore;
        this.ScoreFor1Star = level.ScoreFor1Star;
        this.ScoreFor2Star = level.ScoreFor2Star;
        this.ScoreFor3Star = level.ScoreFor3Star;
    }


	public Level(string Name, string Place, string Creator,DateTime CreatedDate, int Version){
		this.Name = Name;
        this.Place = Place;
		this.Creator = Creator;
		this.CreatedDate = CreatedDate;
		this.Version = Version;
		Rubbishes = new List<Sampah> ();
		Barriers = new List<Penghalang> ();
		Bubbles = new List<Gelembung> ();
		ScoreFor1Star = 0;
		ScoreFor2Star = 0;
		ScoreFor3Star = 0;
		HighScore = 0;
	}

    /*
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }*/

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Level objAsLevel = obj as Level;
        if (objAsLevel == null) return false;
        else return Equals(objAsLevel);
    }

    public bool Equals(Level other)
    {
        return other.Name == this.Name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

    public bool Validate()
    {
		return IsNewLevel();
    }

    public bool IsNewLevel()
    {
        List<Level> listOfLevel = FindAllAsc();
        return !listOfLevel.Contains(this);
    }

    public bool Delete()
    {
        List<Level> listOfLevel = FindAllAsc();
        bool status = listOfLevel.Remove(this);
        EntityManager.Save<List<Level>>(DataKey, listOfLevel);
        return status;
    }

    public virtual void Save()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Level));
        using (var stream = new FileStream(Path.Combine(Application.persistentDataPath, Name+".xml"), FileMode.Create))
        {
            serializer.Serialize(stream, this);
            Debug.Log(Application.persistentDataPath);
            stream.Close();
        }
        /*
        List<Level> listOfLevel = FindAllAsc();
		if(IsNewLevel())
			this.CreatedDate = System.DateTime.UtcNow;

        if (Validate())
        {
            listOfLevel.Add(this);
            EntityManager.Save<List<Level>>(DataKey, listOfLevel);
			this.Save();
        }
        else {
            this.Delete();
            this.Save();
        }
         * */
    }

    public static List<Level> FindAllAsc()
    {
		List<Level> listOfLevels = EntityManager.Load<List<Level>> (DataKey);
		if(listOfLevels != null)
        	return EntityManager.Load<List<Level>>(DataKey);
		return new List<Level> ();
    }

	public static List<Level> FindAllDesc()
	{
		List<Level> listOfLevels = EntityManager.Load<List<Level>> (DataKey);
		List<Level> listOfListDesc = new List<Level> ();
		if (listOfLevels != null) {
			int count = listOfLevels.Count;
			for(int i=count-1; i >= 0; i--)
			{
				listOfListDesc.Add(listOfLevels[i]);
			}
		}
		return listOfListDesc;
	}

    public static Level FindByName(string name)
    {
        List<Level> listOfLevel = FindAllAsc();
        foreach (Level level in listOfLevel)
        {
            if (level.Name == name)
                return level;
        }
        return null;
    }

    public bool HasUndefineBubble()
    {
        foreach (Gelembung gel in Bubbles)
        {
            if (!gel.Define)
            {
                return true;
            }
        }
        return false;
    }
}
