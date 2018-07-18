using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class FacebookDataUser : MonoBehaviour {

    public static Texture2D FBPhoto = null;
    public static string FBName ="";

    public GameObject FacebookDataUserPanel;
    public RawImage ImageProfile;
    public Text TextName;

    public void OnEnable()
    {
        LoadingPopupController.OnLoginSuccess += Success;
    }

    public void OnDisable()
    {
        LoadingPopupController.OnLoginSuccess -= Success;
    }

    void Success()
    {
        if (FB.IsLoggedIn && (FBName == "" || FBPhoto == null))
        {
            transform.localScale = new Vector3(1, 1, 1);
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, Callback);
            StartCoroutine(LoadProfilePhoto());
        }
        else if (FB.IsLoggedIn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            TextName.text = FBName;
            ImageProfile.texture = FBPhoto;
        }
        else
        {
            Hide();
        }
    }


	// Use this for initialization
	void Start () {
        Hide();
        if (FB.IsLoggedIn && (FBName == "" || FBPhoto == null))
        {
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, Callback);
            StartCoroutine(LoadProfilePhoto());
        }
        else if (FB.IsLoggedIn)
        {
            TextName.text = FBName;
            ImageProfile.texture = FBPhoto;
        }
	}

    public IEnumerator LoadProfilePhoto()
    {
        WWW www = new WWW("https://graph.facebook.com/"+FB.UserId+"/picture?width=200&height=200&redirect=true");
        yield return www;
        if (www.error == null)
        {
            FBPhoto = www.texture;
            ImageProfile.texture = FBPhoto;
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void Callback(FBResult result)
    {
        if (result.Error == null)
        {
            var dict = Json.Deserialize(result.Text) as Dictionary<string, object>;
            Debug.Log(result.Text);

            FBName = (string)dict["first_name"];
            TextName.text = FBName;
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public void Logout()
    {
        FB.Logout();
        FBName = "";
        FBPhoto = null;
        Hide();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        Success();
        Debug.Log("asdas");
    }

    public void Hide()
    {
        transform.localScale = Vector3.zero;
    }
}
