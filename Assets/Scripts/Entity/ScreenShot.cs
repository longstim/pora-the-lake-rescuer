using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScreenShot{

    public class CluePenghalang {
        public Vector2 Posisi;
        public string Packagename;
        public float Rotasi;
        public float KecepatanRotasi;
        public string Nama;
    }

    public class ClueGelembung {
        public Vector2 Posisi;
        public int Size;
        public string PackageName;
        public BubbleController.BubbleType Type;
    }

    public class ClueSampah {
        public Vector2 Posisi;
        public string Packagename;
        public float Rotasi;
        public float KecepatanRotasi;
        public int Size;
    }

    public class CluePora {
        public Vector2 Posisi;

        public CluePora(Vector2 position)
        {
            Posisi = position;
        }

        public CluePora() { 
        
        }
    }

    public class PlaceClue {
        public string Place;

        public PlaceClue(string place)
        {
            Place = place;
        }

        public PlaceClue() { 
        
        }
    }

    private List<CluePenghalang> barries;

	public List<CluePenghalang> Barries {
		get {
			return barries;
		}
		set {
			barries = value;
		}
	}

    private List<ClueGelembung> bubbles;

	public List<ClueGelembung> Bubbles {
		get {
			return bubbles;
		}
		set {
			bubbles = value;
		}
	}

    private List<ClueSampah> rubbish;

	public List<ClueSampah> Rubbish {
		get {
			return rubbish;
		}
		set {
			rubbish = value;
		}
	}

    private CluePora pora;

	public CluePora Pora {
		get {
			return pora;
		}
		set {
			pora = value;
		}
	}

    private PlaceClue place;

    public PlaceClue Place
    {
        get{
            return place;
        }
        set {
            place = value;
        }

    }

    public ScreenShot() {
        Rubbish = new List<ClueSampah>();
        Bubbles = new List<ClueGelembung>();
        Barries = new List<CluePenghalang>();
    }

    public static string Serialize(ScreenShot screenshot)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(screenshot.GetType());

        using (StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, screenshot);
            return textWriter.ToString();
        }
    }

    public static ScreenShot Deserialize(string screenInText)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ScreenShot));
        return (ScreenShot)serializer.Deserialize(new StringReader(screenInText));
    }
}
