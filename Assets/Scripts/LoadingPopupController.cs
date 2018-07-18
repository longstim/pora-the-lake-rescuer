using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class LoadingPopupController : MonoBehaviour {

    public Text TextName;

    private bool loadingFriendsStatus = false;
    private bool LoadingFriendsStatus
    {
        get { return loadingFriendsStatus; }
        set {
            loadingFriendsStatus = value;
            AfterRequest();
        }
    }
    private bool readScoreStatus = false;
    private bool ReadScoreStatus
    {
        get { return readScoreStatus; }
        set
        {
            readScoreStatus = value;
            AfterRequest();
        }
    }

    private bool submitScoreStatus = false;
    private bool SubmitScoreStatus
    {
        get { return submitScoreStatus; }
        set
        {
            submitScoreStatus = value;
            AfterRequest();
        }
    }

    private bool connectFacebookStatus = false;
    private bool ConnectFacebookStatus
    {
        get { return connectFacebookStatus; }
        set
        {
            connectFacebookStatus = value;
            AfterRequest();
        }
    }

	public delegate void Callback();

	public Text textMessage;

    Animator animator;

    Callback successCallback;
    Callback failedCallback;

    public static event Callback OnLoginSuccess;
    public static event Callback OnLoginFailed;

    public static event Callback OnReadAllScoresSuccess;

    public bool usingCallback = false;

    public MessagePopupController messagePopupController;
    public PoraCustomization poraCustomization;

    int CountRequestConnect = 1;
    int CountRequestReadAllScore = 1;
    int CountSubmitAllScores = 1;
    int CountRequestFriends = 1;
    int CountRequestName = 1;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        successCallback = null;
        failedCallback = null;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Show()
    {
        CountRequestConnect = 1;
        CountRequestReadAllScore = 1;
        CountSubmitAllScores = 1;
        CountRequestFriends = 1;
        CountRequestName = 1;

        ConnectFacebookStatus = false;
        ReadScoreStatus = false;
        SubmitScoreStatus = false;
        LoadingFriendsStatus = false;

        if (FB.IsLoggedIn)
            return;

        usingCallback = false;
        loadingFriendsStatus = false;
        readScoreStatus = false;
        if (!FB.IsInitialized)
        {
            animator.SetTrigger("Show");
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {
            if (!FB.IsLoggedIn)
            {
                animator.SetTrigger("Show");
                FB.Login("publish_actions,user_location,user_likes,user_groups,user_friends", LoginCallback);
            }
            else {
                messagePopupController.Show("You have been login!");
            }
        }
    }

    public void Show(Callback successCallback, Callback failedCallback)
    {
        CountRequestConnect = 1;
        CountRequestReadAllScore = 1;
        CountSubmitAllScores = 1;
        CountRequestFriends = 1;
        CountRequestName = 1;

        ConnectFacebookStatus = false;
        ReadScoreStatus = false;
        SubmitScoreStatus = false;
        LoadingFriendsStatus = false;

        if (FB.IsLoggedIn)
            return;

        usingCallback = true;
        loadingFriendsStatus = false;
        readScoreStatus = false;
        this.successCallback = successCallback;
        this.failedCallback = failedCallback;
        if (!FB.IsInitialized)
        {
            animator.SetTrigger("Show");
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {
            if (!FB.IsLoggedIn)
            {
                animator.SetTrigger("Show");
                FB.Login("publish_actions,user_location,user_likes,user_groups,user_friends", LoginCallback);
            }
            else
            {
                messagePopupController.Show("You have been login!");
            }
        }
    }

    private void OnInitComplete()
    {
        if (FB.IsLoggedIn)
        {
            SuccessLogin();
        }
        else
        {
            FB.Login("publish_actions,user_location,user_likes,user_groups,user_friends", LoginCallback);
        }
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void LoginCallback(FBResult result)
    {
        if (result.Error == null && FB.IsLoggedIn)
        {
            SuccessLogin();
        }
        else {
            FailLogin();
        }
    }

    private void FailLogin()
    {
        Hide();
        messagePopupController.Show("Failed to login!");
    }

    private void SuccessLogin()
    {
        FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, RequestNameCallback);
        LoadUserFriends();

        string command = "{";
        command += "action:NEW_FACEBOOK_LOGIN";
        command += "}";
        ServerStatistic.DoRequest(command);
    }

    public void RequestNameCallback(FBResult result)
    {
        if (result.Error == null)
        {
            var dict = Json.Deserialize(result.Text) as Dictionary<string, object>;
            Debug.Log(result.Text);
            StartCoroutine(ConnectFacebook((string)dict["first_name"]));
        }
        else
        {
            Debug.Log(result.Error);
            if (CountRequestName < 3)
            {
                LoadUserFriends();
                CountRequestName++;
            }
            else {
                Hide();
                messagePopupController.Show("Failed to connect to server!");
            }
        }
    }

    public IEnumerator ConnectFacebook(string first_name)
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("facebook_id", FB.UserId);
        wwwForm.AddField("facebook_name", first_name);
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);

        WWW www = new WWW(UserDataManager.URL_CONNECT_FACEBOOK, wwwForm);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("ConnectFacebook : " + www.text);
            StartCoroutine(ReadAllScores());
            StartCoroutine(SubmitAllScores());
            ConnectFacebookStatus = true;
        }
        else
        {
            Debug.Log("ConnectFacebook : " + www.error);
            if (CountRequestConnect < 3)
            {
                StartCoroutine(ConnectFacebook(first_name));
                CountRequestConnect++;
            }
            else {
                ConnectFacebookStatus = true;
                ReadScoreStatus = true;
                SubmitScoreStatus = true;
            }
        }
    }

    public IEnumerator ReadAllScores()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("facebook_id", FB.UserId);
        wwwForm.AddField("app_token",UserDataManager.APP_TOKEN);

        WWW www = new WWW(UserDataManager.URL_READ_ALL_SCORE, wwwForm);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("ReadAllScores : " + www.text);
            UserScoreData userScore = UserScoreData.Load();
            var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
            List<object> data = dict["data"] as List<object>;
            Debug.Log(data);
            if (data != null)
            {
                Debug.Log("Berhasil");
                for (int i = 0; i < data.Count; i++)
                {
                    Dictionary<string, object> scoreData = (Dictionary<string, object>)data[i];
                    userScore.RenewScore(int.Parse((string)scoreData["level_system_id"]), int.Parse((string)scoreData["score"]));
                }
            }
            else {
                Debug.Log("Gagal");
            }
            ReadScoreStatus = true;
            if (OnReadAllScoresSuccess != null)
            {
                OnReadAllScoresSuccess();
            }
        }
        else {
            Debug.Log("ReadAllScores : " + www.error);
            if (CountRequestReadAllScore < 3)
            {
                StartCoroutine(SubmitAllScores());
                CountRequestReadAllScore++;
            }
            else
            {
                ReadScoreStatus = true;
            }
        }    
    }

    public IEnumerator SubmitAllScores()
    {
        UserScoreData userScores = UserScoreData.Load();

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("facebook_id", FB.UserId);
        wwwForm.AddField("app_token", UserDataManager.APP_TOKEN);
        wwwForm.AddField("scores", userScores.SerializeToJsonUsingGlobalId());
        Debug.Log("facebook_id : " + FB.UserId);
        Debug.Log("app_token : " + UserDataManager.APP_TOKEN);
        Debug.Log("scores : " + userScores.SerializeToJsonUsingGlobalId());

        WWW www = new WWW(UserDataManager.URL_SUBMIT_ALL_SCORE, wwwForm);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("SubmitAllScores : " + www.text);
            SubmitScoreStatus = true;
        }
        else
        {
            if (CountSubmitAllScores < 3)
            {
                StartCoroutine(SubmitAllScores());
                CountSubmitAllScores++;
            }
            else
            {
                SubmitScoreStatus = true;
            }
        }
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

            UserDataManager.Friends.Clear();
            for (int i = 0; i < tempFriends.Count; i++)
            {
                var friendDict = ((Dictionary<string, object>)(tempFriends[i]));
                UserDataManager.Friends.Add(((string)friendDict["id"]), (string)friendDict["first_name"]);
            }

            if (dict.ContainsKey("next"))
            {
                var query = (string)dict["next"];
                FB.API(query, Facebook.HttpMethod.GET, CallbackLoadFriends);
            }
            else
            {
                UserDataManager.IsSuccessLoadFriend = true;
            }
            LoadingFriendsStatus = true;
        }
        else {
            Debug.Log("CallbackLoadFriends : " + result.Error);
            if (CountRequestFriends < 3)
            {
                LoadUserFriends();
                CountRequestFriends++;
            }
            else {
                LoadingFriendsStatus = true;       
            }
        }
    }

    private void Hide()
    {
        animator.SetTrigger("Hide");
    }

    private void AfterRequest()
    {
        if (LoadingFriendsStatus && ReadScoreStatus && SubmitScoreStatus && ConnectFacebookStatus)
        {
            Debug.Log("1");
            if (usingCallback)
            {
                Debug.Log("2");
                if (ReadScoreStatus)
                {
                    Debug.Log("3");
                    if (successCallback!=null)
                        successCallback();

                    usingCallback = false;
                }
                else
                {
                    if (failedCallback != null)
                        failedCallback();

                    usingCallback = false;
                }
            }

            if (FB.IsLoggedIn)
            {
                Debug.Log("Berhasil Login Callback");
                if (OnLoginSuccess != null)
                    OnLoginSuccess();
            }
            else {
                Debug.Log("Gagal Login Callback");
                if (OnLoginFailed != null)
                    OnLoginFailed();
            }

            if (poraCustomization)
                poraCustomization.Refresh();

            messagePopupController.Show("Welcome !");
            Hide();
        }
    }

    public void CheckToLogin() {
        if (!FB.IsLoggedIn)
        {
            Show();
        }
    }
}
