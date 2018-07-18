using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class UpdateChecker : MonoBehaviour {

    public const string CurrentVersion = "1.3.0";
    public static string ServerVersion = "1.3.0";

    [HideInInspector]
    public static bool IsUpdateFinish = false;

	// Use this for initialization
	void Start () {
        if (Application.loadedLevelName == SceneName.SplashPora.ToString())
        {
            StartCoroutine("CheckUpdate");
            DontDestroyOnLoad(gameObject);
            Invoke("BeFinish", 10);
        }
        else {
            gameObject.SetActive(ServerVersion != CurrentVersion);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void BeFinish()
    {
        IsUpdateFinish = true;
    }

    IEnumerator CheckUpdate()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("token", "kshfishmkmwsdui");
        WWW www = new WWW("https://www.nedstudio.net/ned/api/web/v1/app-installs/current-version");

        yield return www;

        if (www.error == null)
        {
            Debug.Log(www.text);
            var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
            Dictionary<string, object> data = dict["data"] as Dictionary<string, object>;

            ServerVersion = ((string)data["version"]);
            if (ServerVersion != CurrentVersion)
            {
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.ned.PoraTheLakeRescuer");
            }
        }
        else {
            Debug.Log(www.error);
        }
        IsUpdateFinish = true;
        Destroy(gameObject);
    }

    public void OnClick()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ned.PoraTheLakeRescuer");
    }

}
