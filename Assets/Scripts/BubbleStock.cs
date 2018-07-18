using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BubbleStock : MonoBehaviour {

    public Text text;

    public static bool newChange = false;
	// Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("bubble"))
        {
            PlayerPrefs.SetInt("bubble", 0);
            PlayerPrefs.Save();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (newChange)
        {
            text.text = PlayerPrefs.GetInt("bubble").ToString(); ;
            newChange = false;
        }
	}
}
