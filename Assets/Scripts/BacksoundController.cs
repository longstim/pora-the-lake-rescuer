using UnityEngine;
using System.Collections;

public class BacksoundController : MonoBehaviour {

    public static bool DestroyNow = false;
    public static bool IsActive = false;

    static float volume = 1;
    public static float Volume {
        get { return volume; }
        set {
            volume = value;
            newChange = true;
        }
    }

    static bool newChange = false;
    public bool isTriggerByIntro = false;

	// Use this for initialization
	void Start () {
        if (IsActive)
        {
           Destroy(gameObject);
        }
        else {
            if (!isTriggerByIntro)
            {
                IsActive = true;
                GetComponent<AudioSource>().Play();
            }
            DontDestroyOnLoad(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (newChange)
        {
            GetComponent<AudioSource>().volume = volume;
            newChange = false;
        }
        if (!IsActive && !isTriggerByIntro)
        {
            Destroy(gameObject);
        }

        if (DestroyNow)
        {
            Destroy(gameObject);
            DestroyNow = false;
        }
	}
}
