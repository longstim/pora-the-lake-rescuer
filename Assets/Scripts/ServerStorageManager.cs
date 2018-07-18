using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerStorageManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SubmitPostPlay(StaticLevel dataLevel, int WhiteBubbleBonus,int RedBubbleBonus,int OrangBubbleBonus,int GoldBonus, string StatusPostPlay)
    {
        if (dataLevel.HighScore < EntityManager.Load<int>("cache_" + dataLevel.Place + "_" + dataLevel.Level.ToString()))
        {
            dataLevel.HighScore = EntityManager.Load<int>("cache_" + dataLevel.Place + "_" + dataLevel.Level.ToString());
        }

        if (FB.IsLoggedIn)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
            wwwForm.AddField("facebook_id", FB.UserId);
            wwwForm.AddField("white_bubble", WhiteBubbleBonus);
            wwwForm.AddField("red_bubble", RedBubbleBonus);
            wwwForm.AddField("orange_bubble", OrangBubbleBonus);
            wwwForm.AddField("gold", GoldBonus);
            wwwForm.AddField("level", ((StaticLevel)dataLevel).Place+"_"+((StaticLevel)dataLevel).Level.ToString()); /// parbaba_1
            wwwForm.AddField("score", dataLevel.HighScore);
            wwwForm.AddField("status", StatusPostPlay);

            WWW www = new WWW(UserDataManager.URL_SUBMIT_ONCE_PLAY, wwwForm);

            StartCoroutine(DoRequestPostPlay(www,dataLevel));
        }
        else {
            EntityManager.Save<int>("cache_" + dataLevel.Place + "_" + dataLevel.Level.ToString(), dataLevel.HighScore);
            UserDataManager.LevelScores[dataLevel.Place + "_" + dataLevel.Level.ToString()] = dataLevel.HighScore;
        }
    }

    private IEnumerator DoRequestPostPlay(WWW www,StaticLevel dataLevel)
    {
        yield return www;
        if (www.error != null)
        { 
            UserDataManager.LevelScores[(((StaticLevel)dataLevel).Place+"_"+((StaticLevel)dataLevel).Level.ToString())]=dataLevel.HighScore;
            EntityManager.Save<int>("cache_" + dataLevel.Place + "_" + dataLevel.Level.ToString(), dataLevel.HighScore);
        }
    }

}
