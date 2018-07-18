using UnityEngine;
using System.Collections;

public class SubmitScoreController : MonoBehaviour {


    void OnEnable()
    {
        UserScoreData.OnScoreChange += Submit;
    }

    void OnDisable()
    {
        UserScoreData.OnScoreChange -= Submit;
    }

    string GlobalId;
    int Score;
    public void Submit(string globalId, int Score)
    {
        if(FB.IsLoggedIn)
        {
            GlobalId = globalId;
            this.Score = Score;
            StartCoroutine(DoRequest());
        }
    }

    IEnumerator DoRequest()
    { 
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
        wwwForm.AddField("facebook_id", FB.UserId);
        wwwForm.AddField("level_system_id", GlobalId);
        wwwForm.AddField("score", Score);

        WWW www = new WWW(UserDataManager.URL_SUBMIT_ONCE_PLAY,wwwForm);

        yield return www;

        if (www.error == null)
        {
            Debug.Log(www.text);
        }
        else {
            Debug.Log(www.error);
        }
    }
}
