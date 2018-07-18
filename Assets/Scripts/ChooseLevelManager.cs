using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public class ChooseLevelManager : MonoBehaviour {


    public SceneName CurrentScene;
	public GameObject PanelPreviewBubble;
	public GameObject white;
	public GameObject Red;
	public GameObject yellow;
    public Animator LoadingPopUp;


    public AudioSource[] audioSourcesEffect;
    public AudioSource[] audioSourcesMusic;

	public PrePlayPopupManager preplayPopUp;
    public GameObject SlideLevel;

	public void showWhite()
	{
		PanelPreviewBubble.SetActive (true);
		white.SetActive (true);
		Red.SetActive (false);
		yellow.SetActive (false);
	}
	
	public void showRed()
	{
		PanelPreviewBubble.SetActive (true);
		white.SetActive (false);
		Red.SetActive (true);
		yellow.SetActive (false);
	}
	
	public void showYellow()
	{
		PanelPreviewBubble.SetActive (true);
		white.SetActive (false);
		Red.SetActive (false);
		yellow.SetActive (true);
	}
	public void exitPreview()
	{
		PanelPreviewBubble.SetActive (false);
	}

	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;

    string data;
    int levelLastPlayed = 1;

	// Use this for initialization
	void Start () {

        UserSoundConfig config = UserSoundConfig.Load();

        for (int i = 0; i < audioSourcesEffect.Length; i++)
        {
            audioSourcesEffect[i].volume = config.SoundVolume;
            audioSourcesEffect[i].mute = !config.SoundStatus;
        }

        for (int i = 0; i < audioSourcesMusic.Length; i++)
        {
            audioSourcesMusic[i].volume = config.MusicVolume;
            audioSourcesMusic[i].mute = !config.MusicStatus;
        }

        SceneCommunication com = new SceneCommunication(CurrentScene);
		List<CommunicationData> listOfdata = com.RetrieveMessages ();

        if (listOfdata.Count == 0)
        {
            data = "Parbaba";
        }
        else if (listOfdata.Count == 1 && (listOfdata[0].Sender == SceneName.PlayParbaba || listOfdata[0].Sender == SceneName.PlayBatuguru || listOfdata[0].Sender == SceneName.PlayParapat || listOfdata[0].Sender == SceneName.PlayTomok))
        {
            preplayPopUp.Show((StaticLevel)listOfdata[0].Data);
        }
        else
        {
            data = (string)listOfdata[0].Data;
            if (listOfdata[0].Sender == SceneName.PlayParbaba || listOfdata[0].Sender == SceneName.PlayBatuguru || listOfdata[0].Sender == SceneName.PlayParapat || listOfdata[0].Sender == SceneName.PlayTomok)
            {
                levelLastPlayed = (int)listOfdata[1].Data;
            }
        }

        SlideLevel.GetComponent<ScrollRect>().horizontalNormalizedPosition = levelLastPlayed <= 5 ? 0 : levelLastPlayed / 20f;

        string command = "{";
        command += "action:OPEN_CHOOSELEVEL";
        command += ",place:"+data;
        command += "}";
        ServerStatistic.DoRequest(command);

	
	}

	// Update is called once per frame
	void Update () {
		if (asynOperation != null) {
			if (asynOperation.progress >= 0.9f && transisionAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Loading")) {
				Invoke ("ActivateScene", 1f);
			}
		}
		else if (Input.GetKey(KeyCode.Escape))
		{
			if(preplayPopUp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Show"))
			{
				preplayPopUp.Hide();
			}
            else if(LoadingPopUp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(9).IsName("Show")){
               
            }
			else
			{
				transisionAnimator.SetTrigger("Close");
				BackScene();
			}
			
		}
	}

	public void BackScene()
	{
        SceneCommunication sceneCommunication = new SceneCommunication(CurrentScene);
        sceneCommunication.SendMessage(SceneName.ChoosePlace, data);
		StartCoroutine ("LoadScene");
	}
	
	IEnumerator LoadScene()
	{
		
		asynOperation = Application.LoadLevelAsync (SceneName.ChoosePlace.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
		Debug.Log ("Masuk");
	}
	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}

}
