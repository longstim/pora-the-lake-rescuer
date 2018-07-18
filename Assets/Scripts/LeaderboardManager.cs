using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public class LeaderboardManager : MonoBehaviour {
    public GameObject OwnFriendItem;
    public GameObject OwnGlobalItem;


    public GameObject PanelItemFriend;
    public GameObject PanelItemGlobal;

    public GameObject itemTemplate;
    public GameObject LoadingPopupFriend;
    public GameObject LoadingPopupGlobal;

    StaticLevel dataStaticLevel= null;
    bool LastStatus = false;

    public GameObject ConnectButton;

    public WatchClueManager watchClueManager;

    int CountRequsetGlobal = 1;
    int CountRequestFriend = 1;

    void OnEnable()
    {
        LoadingPopupController.OnLoginSuccess += OnLoginSuccess;
        LoadingPopupController.OnLoginFailed += OnLoginFailed;
    }

    void OnDisable()
    {
        LoadingPopupController.OnLoginSuccess -= OnLoginSuccess;
        LoadingPopupController.OnLoginFailed -= OnLoginFailed;
    }

    public void OnLoginSuccess()
    {
        Refresh();
        Debug.Log("OnLoginSuccess");
    }

    public void OnLoginFailed()
    {
        //Refresh();
        Debug.Log("OnLoginFailed");
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Refresh()
    {
        ConnectButton.SetActive(false);
        if (dataStaticLevel != null)
        {;
            if (FB.IsLoggedIn)
            {
                for (int i = 0; i < PanelItemFriend.transform.childCount; i++)
                {
                    Destroy(PanelItemFriend.transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < PanelItemGlobal.transform.childCount; i++)
                {
                    Destroy(PanelItemGlobal.transform.GetChild(i).gameObject);
                }

                OwnGlobalItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataStaticLevel.HighScore, "", "", "");
                OwnFriendItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataStaticLevel.HighScore, "", "", "");

                LoadingPopupFriend.SetActive(true);
                LoadingPopupGlobal.SetActive(true);
                StartCoroutine("RefreshFriend");
                StartCoroutine("RefreshGlobal");
            }
            else
            {
                OwnGlobalItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataStaticLevel.HighScore, "", "", "");
                OwnFriendItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataStaticLevel.HighScore, "", "", "");
                LoadingPopupFriend.SetActive(false);
                LoadingPopupGlobal.SetActive(false);
                ConnectButton.SetActive(true);
            }
        }
    }

    public void Show(StaticLevel dataLevel)
    {
        CountRequsetGlobal = 1;
        CountRequestFriend = 1;

        ConnectButton.SetActive(false);
        if (dataStaticLevel==null || dataStaticLevel.GlobalId != dataLevel.GlobalId || !LastStatus)
        {
            this.dataStaticLevel = dataLevel;
            if (FB.IsLoggedIn)
            {
                for (int i = 0; i < PanelItemFriend.transform.childCount; i++ )
                {
                    Destroy(PanelItemFriend.transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < PanelItemGlobal.transform.childCount; i++)
                {
                    Destroy(PanelItemGlobal.transform.GetChild(i).gameObject);
                }

                OwnGlobalItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataLevel.HighScore, "", "", "");
                OwnFriendItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataLevel.HighScore, "", "", "");

                LoadingPopupFriend.SetActive(true);
                LoadingPopupGlobal.SetActive(true);
                StartCoroutine("RefreshFriend");
                StartCoroutine("RefreshGlobal");
            }
            else
            {
                OwnGlobalItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataLevel.HighScore, "", "", "");
                OwnFriendItem.GetComponent<ItemLeaderboard>().Init(0, "You", dataLevel.HighScore, "", "", "");
                LoadingPopupFriend.SetActive(false);
                LoadingPopupGlobal.SetActive(false);
                ConnectButton.SetActive(true);
            }
        }
    }

    public IEnumerator RefreshFriend()
    {
        WWWForm wwwForm = new WWWForm();
        string friends = FB.UserId;
        foreach (KeyValuePair<string, string> pair in UserDataManager.Friends)
        {
            friends += "," + pair.Key;
        }
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
        wwwForm.AddField("facebook_id", FB.UserId);
        wwwForm.AddField("list_friend", friends);
        wwwForm.AddField("level", dataStaticLevel.GlobalId);
        Debug.Log("app_token :"+UserDataManager.APP_TOKEN);
        Debug.Log("facebook_id : " + FB.UserId);
        Debug.Log("list_friend : " + friends);
        Debug.Log("level : " + dataStaticLevel.GlobalId);
        
        WWW www = new WWW(UserDataManager.URL_LEADERBOARD_LEVEL_FRIEND, wwwForm);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("RefreshFriend :" + www.text);

            var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
            var data = (Dictionary<string, object>)dict["data"];
            Dictionary<string, object> own = (Dictionary<string, object>)data["own"];
            if(own != null)
                OwnFriendItem.GetComponent<ItemLeaderboard>().Init(int.Parse(own["position"].ToString()), "You", int.Parse(own["score"].ToString()), "", "","");

            List<object> tempListUser = (List<object>)data["global"];

            for (int i = 0; i < PanelItemFriend.transform.childCount; i++)
            {
                Destroy(PanelItemFriend.transform.GetChild(i).gameObject);
            }

            if (tempListUser != null)
            {
                for (int i = 0; i < tempListUser.Count; i++)
                {
                    GameObject itemLeaderboard = (GameObject)Instantiate(itemTemplate);
                    var user = ((Dictionary<string, object>)(tempListUser[i]));
                    if (FB.UserId == user["facebook_id"].ToString())
                    {
                        itemLeaderboard.GetComponent<ItemLeaderboard>().Init(int.Parse(user["position"].ToString()), "You", int.Parse(user["score"].ToString()), "", "", "");
                        itemLeaderboard.GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        itemLeaderboard.GetComponent<ItemLeaderboard>().Init(int.Parse(user["position"].ToString()), user["facebook_name"].ToString(), int.Parse(user["score"].ToString()), "", "", "");
                    }
                    itemLeaderboard.transform.SetParent(PanelItemFriend.transform);
                    itemLeaderboard.transform.localScale = new Vector3(1, 1, 1);

                    int flagClue = int.Parse(user["clue_data"].ToString());
                    if (flagClue == 1)
                    {
                        itemLeaderboard.GetComponent<ItemLeaderboard>().ShowWatchClue(watchClueManager, user["id"].ToString());
                    }
                }
            }
            LastStatus = true;
            LoadingPopupFriend.SetActive(false);
        }
        else {
            Debug.Log(www.error);
            if (CountRequestFriend < 3)
            {
                StartCoroutine("RefreshFriend");
                CountRequestFriend++;
            }
            else {
                LastStatus = false;
                LoadingPopupFriend.SetActive(false);
            }
        }
    }

    public IEnumerator RefreshGlobal()
    {
        WWWForm wwwForm = new WWWForm();
        
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
        wwwForm.AddField("facebook_id", FB.UserId);
        wwwForm.AddField("level", dataStaticLevel.GlobalId);

        WWW www = new WWW(UserDataManager.URL_LEADERBOARD_LEVEL_GLOBAL, wwwForm);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("RefreshGlobal :" + www.text);

            var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
    
            var data = (Dictionary<string, object>)dict["data"];

            Dictionary<string, object> own = (Dictionary<string, object>)data["own"];
            if (own != null)
            {
                OwnGlobalItem.GetComponent<ItemLeaderboard>().Init(int.Parse(own["position"].ToString()), "You", int.Parse(own["score"].ToString()), "", "", "");
            }

            List<object> tempListUser = (List<object>)data["global"];

            for (int i = 0; i < PanelItemGlobal.transform.childCount; i++)
            {
                Destroy(PanelItemGlobal.transform.GetChild(i).gameObject);
            }

            if (tempListUser != null)
            {
                bool haveScore = false;
                for (int i = 0; i < tempListUser.Count; i++)
                {
                    GameObject itemLeaderboard = (GameObject)Instantiate(itemTemplate);
                    var user = ((Dictionary<string, object>)(tempListUser[i]));
                    if (FB.UserId == user["facebook_id"].ToString())
                    {
                        itemLeaderboard.GetComponent<ItemLeaderboard>().Init(int.Parse(user["position"].ToString()), "You", int.Parse(user["score"].ToString()), "", "", "");
                        itemLeaderboard.GetComponent<Image>().color = Color.green;
                        haveScore = true;
                    }
                    else
                    {
                        itemLeaderboard.GetComponent<ItemLeaderboard>().Init(int.Parse(user["position"].ToString()), user["facebook_name"].ToString(), int.Parse(user["score"].ToString()), "", "", "");
                    }
                    itemLeaderboard.transform.SetParent(PanelItemGlobal.transform);
                    itemLeaderboard.transform.localScale = new Vector3(1, 1, 1);
                    int flagClue = int.Parse(user["clue_data"].ToString());
                    if (flagClue == 1)
                    {
                        itemLeaderboard.GetComponent<ItemLeaderboard>().ShowWatchClue(watchClueManager, user["id"].ToString());
                    }
                }
            }
            LastStatus = true;
            LoadingPopupGlobal.SetActive(false);
        }
        else
        {
            Debug.Log(www.error);
            if (CountRequsetGlobal < 3)
            {
                StartCoroutine("RefreshGlobal");
                CountRequsetGlobal++;
            }
            else
            {
                LastStatus = false;
                LoadingPopupGlobal.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
    }
    
}
