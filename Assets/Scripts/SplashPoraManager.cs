using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;
using System.Collections.Generic;

public class SplashPoraManager : MonoBehaviour {

    public MainSceneLoader mainSceneLoader;
    public GameObject rectLoadingBar;
    public Text textMessage;

    public bool allowChange = false;
	// Use this for initialization

    public string[] message;
    float Lama;

    float maxWidth;
	void Start () {
        Lama = 0;
        try
        {
            if (Advertisement.isSupported)
            {
                Advertisement.allowPrecache = true;
                Advertisement.Initialize("27637", false);
            }
            else
            {
                Debug.Log("Platform not supported");
            }
            Invoke("Allow",5);
            if (message.Length > 1)
            {
                textMessage.text = message[Random.Range(0, message.Length - 1)];
            }
        }
        catch (System.Exception e)
        {
            textMessage.text = e.ToString();
        }
	}

    public void Allow()
    {
        allowChange = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (mainSceneLoader.asynOperation != null)
        {
            float scale = ((mainSceneLoader.asynOperation.progress /0.9f) + (Lama / 5))/2;
            if(scale > 1)
                scale = 1;
            rectLoadingBar.transform.localScale = new Vector3(scale, 1, 1);
            if (mainSceneLoader.asynOperation.progress >= 0.9f && UpdateChecker.IsUpdateFinish && allowChange)
            {
                mainSceneLoader.ActivateScene();
            }
        }
	}

    void FixedUpdate() {
        if (Lama < 5) {
            Lama += Time.fixedDeltaTime;
        }
    }
}
