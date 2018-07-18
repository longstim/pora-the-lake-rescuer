using UnityEngine;
using System.Collections;

public class BacksoundIntroController : MonoBehaviour {

    public static bool DestroyNow = false;

    AudioSource audioSource;
    public BacksoundController backsoundController;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        Debug.Log(BacksoundController.IsActive);
        Debug.Log(DestroyNow);
        if (BacksoundController.IsActive)
        {
            Destroy(gameObject);
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!audioSource.isPlaying)
        {
            backsoundController.GetComponent<AudioSource>().Play();
            backsoundController.isTriggerByIntro = false;
            BacksoundController.IsActive = true;
            Destroy(gameObject);
        }

        if (DestroyNow)
        {
            Destroy(gameObject);
            DestroyNow = false;
        }
	}    
}
