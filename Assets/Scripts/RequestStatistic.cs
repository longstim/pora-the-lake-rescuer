using UnityEngine;
using System.Collections;

public class RequestStatistic : MonoBehaviour {

    public string Command;
    public string Session;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(doRequest());
	}

    private IEnumerator doRequest()
    { 
        WWWForm form = new WWWForm();
        form.AddField("app_token", "kshfishmkmwsdui");
        form.AddField("device_id", SystemInfo.deviceUniqueIdentifier);
        form.AddField("session", Session);
        form.AddField("command", Command);
        WWW wwwRequest = new WWW("https://nedstudio.net/ned/api/web/v1/statistics/submit-command", form);

        yield return wwwRequest;

        Destroy(gameObject);
    }
}
