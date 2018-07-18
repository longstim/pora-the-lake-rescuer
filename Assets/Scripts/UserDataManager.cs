using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;

public class UserDataManager : MonoBehaviour {

    public static bool IsSuccessLoadData = false;
    public static bool IsSuccessLoadFriend = false;
    public static Dictionary<string, string> Friends = new Dictionary<string, string>();
    public static Dictionary<string, object> UsedItems = new Dictionary<string, object>();
    public static List<string> UserItems = new List<string>();
    public static Dictionary<string, int> LevelScores = new Dictionary<string, int>();
    public static int WhiteBubbles = 0;
    public static int RedBubbles = 0;
    public static int OrangeBubbles = 0;
    public static int Golds = 0;
    public static int Gems = 0;

    public const string APP_TOKEN = "kshfishmkmwsdui";
    public const string URL_INSTALL = "https://www.nedstudio.net/ned/api/web/v1/app-installs";
    public const string URL_READ_USER_DATA = "https://www.nedstudio.net/ned/api/web/v1/one-requests/loading-screen";
    public const string URL_BUY_GOLD = "https://www.nedstudio.net/ned/api/web/v1/golds/buy-gold";
    public const string URL_BUY_GEMS = "https://www.nedstudio.net/ned/api/web/v1/gems/buy-gems";
    public const string URL_BUY_BUBBLE = "https://www.nedstudio.net/ned/api/web/v1/bubbles/buy-bubble";
    public const string URL_USE_BUBBLE = "https://www.nedstudio.net/ned/api/web/v1/bubbles/use-bubble";
    public const string URL_BUY_BARRIER = "https://www.nedstudio.net/ned/api/web/v1/barriers/buy-barrier";
    public const string URL_BUY_RUBBISH = "https://www.nedstudio.net/ned/api/web/v1/trashers/buy-trasher";
    public const string URL_BUY_PLACE = "#";
    public const string URL_SUBMIT_ONCE_PLAY = "http://nedstudio.net/ned/api/web/v1/leader-boards/submit-score";
    public const string URL_LEADERBOARD_LEVEL_FRIEND = "https://www.nedstudio.net/ned/api/web/v1/leader-boards/read-friend-leader-board-in-level";
    public const string URL_LEADERBOARD_LEVEL_GLOBAL = "https://nedstudio.net/ned/api/web/v1/leader-boards/read-global-leader-board-in-level";
    public const string URL_BUY_ITEM = "https://www.nedstudio.net/ned/api/web/v1/items/buy-item";
    public const string URL_SUBMIT_ALL_SCORE = "http://nedstudio.net/ned/api/web/v1/app-installs/migrate-score";
    public const string URL_READ_ALL_SCORE = "http://nedstudio.net/ned/api/web/v1/app-installs/read-score-player";
    public const string URL_CONNECT_FACEBOOK = "http://nedstudio.net/ned/api/web/v1/app-installs/connect-facebook";

    public bool IsFinishLoad {
        get { return LoadingFriendsStatus && LoadingUserDataStatus && LoadingRegisterDevice; }
    }

    private static bool LoadingFriendsStatus = false;
    private static bool LoadingUserDataStatus = false;
    private static bool LoadingRegisterDevice = false;
    void Awake()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    void Start()
    {
        
    }

    private void OnInitComplete() 
    {
        StartCoroutine("RegisterDevice");
        if (FB.IsLoggedIn)
        {
            StartCoroutine("LoadUserData");
        }
        else { 
            LoadingFriendsStatus = true;
            LoadingUserDataStatus = true;
        }
    }

    public IEnumerator RegisterDevice()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("device_id", SystemInfo.deviceUniqueIdentifier);
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
        wwwForm.AddField("platform", Application.platform.ToString());
        wwwForm.AddField("language", Application.systemLanguage.ToString());
        wwwForm.AddField("operating_system", SystemInfo.operatingSystem);
        //wwwForm.AddField("device_type", SystemInfo.deviceType.ToString());
        wwwForm.AddField("device_model", SystemInfo.deviceModel);
        wwwForm.AddField("country", "ID");
        wwwForm.AddField("screen_resolution", Screen.width + "x" + Screen.height);

        WWW www = new WWW(UserDataManager.URL_INSTALL, wwwForm);

        yield return www;
        Debug.Log(www.text);
        if (www.error == null)
        {
            
        }
        else
        {
            
        }
        LoadingRegisterDevice = true;
    }

    private void OnHideUnity(bool isGameShown) {}

    public IEnumerator LoadUserData()
    { 
        WWWForm wwwForm =  new WWWForm();
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
        wwwForm.AddField("facebook_id", FB.UserId);
        WWW www = new WWW(UserDataManager.URL_READ_USER_DATA, wwwForm);

        yield return www;
        Debug.Log(www.error);
        if (www.error == null)
        {
            SuccessLoadData(www);   
        }
        else
        {            
            FailedLoadData(www);
        }
    }

    public void SuccessLoadData(WWW www)
    {
        var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
        Debug.Log(www.text);
        var status = (string)(((Dictionary<string, object>)dict)["status"]);
        if (status == "200")
        {
            LoadUserFriends();
            var data = (Dictionary<string, object>)dict["data"];

            var bubbles = (Dictionary<string, object>)data["bubble"];
            UserDataManager.WhiteBubbles = int.Parse(bubbles["white_bubble"].ToString());
            UserDataManager.RedBubbles = int.Parse(bubbles["red_bubble"].ToString());
            UserDataManager.OrangeBubbles = int.Parse(bubbles["orange_bubble"].ToString());

            UserDataManager.Golds = int.Parse(data["gold"].ToString());
            UserDataManager.Gems = int.Parse(data["gems"].ToString());

            Dictionary<string, object> tempUsedItems = (Dictionary<string, object>)(data["item"]);
            UserDataManager.UsedItems.Add("head_wear", (string)tempUsedItems["head_wear"]);
            UserDataManager.UsedItems.Add("body_wear", (string)tempUsedItems["body_wear"]);
            UserDataManager.UsedItems.Add("gun_wear", (string)tempUsedItems["gun_wear"]);

            List<object> tempLevelScore = (List<object>)data["scores"];
            for (int i = 0; i < tempLevelScore.Count; i++)
            {
                var tempUserScore = ((Dictionary<string, object>)(tempLevelScore[i]));
                if (int.Parse(tempUserScore["score"].ToString()) > EntityManager.Load<int>("cache_" + ((string)tempUserScore["level_system_id"])))
                {
                    UserDataManager.LevelScores[((string)tempUserScore["level_system_id"])] = EntityManager.Load<int>("cache_" + ((string)tempUserScore["level_system_id"]));
                }
                else {
                    UserDataManager.LevelScores[((string)tempUserScore["level_system_id"])] = int.Parse(tempUserScore["score"].ToString());
                }
                // level : mode_place_level ex: parbaba_1
            }

            List<object> ownItems = (List<object>)data["own_item"];
            UserDataManager.UserItems.Clear();
            for (int i = 0; i < ownItems.Count; i++)
            {
                var tempOwnItems = ((Dictionary<string, object>)(ownItems[i]));
                UserDataManager.UserItems.Add((string)tempOwnItems["name"]);
                // level : mode_place_level ex: parbaba_1
            }
            IsSuccessLoadData = true;
        }
        LoadingUserDataStatus = true;
    }

    public void FailedLoadData(WWW www)
    {
        LoadingFriendsStatus = true;
        LoadingUserDataStatus = true;
    }

    public void LoadUserFriends()
    {
        FB.API("/me/friends?fields=id,first_name", Facebook.HttpMethod.GET, CallbackLoadFriends);
    }

    public void CallbackLoadFriends(FBResult result)
    {
        if (result.Error == null)
        {
            var dict = Json.Deserialize(result.Text) as Dictionary<string, object>;
            var tempFriends = (List<object>)(((Dictionary<string, object>)dict)["data"]);

            for (int i = 0; i < tempFriends.Count; i++)
            {
                var friendDict = ((Dictionary<string, object>)(tempFriends[i]));
                Friends[((string)friendDict["id"])] = (string)friendDict["first_name"];
            }

            if (dict.ContainsKey("next"))
            {
                var query = (string)dict["next"];
                FB.API(query, Facebook.HttpMethod.GET, CallbackLoadFriends);
            }
            else
            {
                IsSuccessLoadFriend = true;
            }
        }
        LoadingFriendsStatus = true;
    }

}
