using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoosePlaceManager : MonoBehaviour {

    public string FirstSelected = "None";
    private List<CommunicationData> dataRecieveds;

	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;
    public GameObject PanelItem;

    bool goToSelected = false;

    public bool CanPlay = false;

    public AudioSource changeSound;

    public Animator AnimatorBatuguru;
    public Animator AnimatorParapat;
    public Animator AnimatorTomok;

    public AudioSource[] audioSourcesEffect;

    UserScoreData userScore;
	void Start () {

        userScore = UserScoreData.Load();

        UserSoundConfig config = UserSoundConfig.Load();

        for (int i = 0; i < audioSourcesEffect.Length; i++)
        {
            audioSourcesEffect[i].volume = config.SoundVolume;
            audioSourcesEffect[i].mute = !config.SoundStatus;
        }

        SceneCommunication sceneCommunication = new SceneCommunication(SceneName.ChoosePlace);
        List<CommunicationData>  dataRecieveds = sceneCommunication.RetrieveMessages();

        if (dataRecieveds.Count > 0 && (dataRecieveds[0].Sender == SceneName.ChooseLevelParbaba || dataRecieveds[0].Sender == SceneName.ChooseLevelBatuguru || dataRecieveds[0].Sender == SceneName.ChooseLevelParapat || dataRecieveds[0].Sender == SceneName.ChooseLevelTomok))
        {
            FirstSelected = (string)dataRecieveds[0].Data;
        }

        if (FirstSelected == "Parbaba")
        {
            PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(715, PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
        }
        else if (FirstSelected == "Batuguru")
        {
            PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(276, PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
        }
        else if (FirstSelected == "Parapat")
        {
            PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(-227, PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
        }
        else if (FirstSelected == "Tomok")
        {
            PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(-673, PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
        }
        
        string command = "{";
        command += "action:OPEN_CHOOSEPLACE";
        command += "}";
        ServerStatistic.DoRequest(command);
	}
	
	// Update is called once per frame
    float timeScale = 0;
    float speed = 5;
    float startMovePosition;
	void Update () {
        if (goToSelected)
        {
            timeScale += Time.deltaTime * speed;
            if (FirstSelected == "Parbaba")
            {
                PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(startMovePosition, 715, timeScale), PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
                if (PanelItem.GetComponent<RectTransform>().localPosition.x == 715)
                {
                    goToSelected = false;
                }
            }
            else if (FirstSelected == "Batuguru")
            {
                PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(startMovePosition, 276, timeScale), PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
                if (PanelItem.GetComponent<RectTransform>().localPosition.x == 276)
                {
                    goToSelected = false;
                }
            }
            else if (FirstSelected == "Parapat")
            {
                PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(startMovePosition, -227, timeScale), PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
                if (PanelItem.GetComponent<RectTransform>().localPosition.x == -227)
                {
                    goToSelected = false;
                }
            }
            else if (FirstSelected == "Tomok")
            {
                PanelItem.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(startMovePosition, -673, timeScale), PanelItem.GetComponent<RectTransform>().localPosition.y, PanelItem.GetComponent<RectTransform>().localPosition.z);
                if (PanelItem.GetComponent<RectTransform>().localPosition.x == -673)
                {
                    goToSelected = false;
                }
            }
        }

		if (asynOperation != null) {
			if (asynOperation.progress >= 0.9f && transisionAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Loading")) {
				Invoke ("ActivateScene", 1f);
				
			}
		}else if (Input.GetKey(KeyCode.Escape))
		{
			transisionAnimator.SetTrigger("Close");
			PressBack();
		}
	}

	public void PressBack()
	{
		StartCoroutine ("LoadScene");
		
	}
	
	IEnumerator LoadScene()
	{
		asynOperation = Application.LoadLevelAsync (SceneName.ChooseMode.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}

    public void ChangeSelected(string Place)
    {
        if (FirstSelected != Place)
        {
            FirstSelected = Place;
            changeSound.Play();
        }
    }

	public void chooseParbaba()
	{
        if (FirstSelected == "Parbaba")
        {
            StartCoroutine("ParbabaScene");
			transisionAnimator.SetTrigger("Close");
            changeSound.Play();
        }
        else
        {
            FirstSelected = "Parbaba";
            goToSelected = true;
            timeScale = 0;
            startMovePosition = PanelItem.GetComponent<RectTransform>().localPosition.x;
        }
		
	}
	
	IEnumerator ParbabaScene()
	{
		SceneCommunication com = new SceneCommunication (SceneName.ChoosePlace);
		com.SendMessage (SceneName.ChooseLevelParbaba, "Parbaba");
		asynOperation = Application.LoadLevelAsync (SceneName.ChooseLevelParbaba.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}

	public void chooseParapat()
	{
        if (FirstSelected == "Parapat")
        {
            if (userScore.GetScore("Batuguru", 20) > 0 || CanPlay)
            {
                StartCoroutine("ParapatScene");
				transisionAnimator.SetTrigger("Close");
                changeSound.Play();
            }
            else
            {
                AnimatorParapat.SetTrigger("unlock");
            }
        }
        else
        {
            FirstSelected = "Parapat";
            goToSelected = true;
            timeScale = 0;
            startMovePosition = PanelItem.GetComponent<RectTransform>().localPosition.x;
        }
		
	}
	
	IEnumerator ParapatScene()
	{

		SceneCommunication com = new SceneCommunication (SceneName.ChoosePlace);
		com.SendMessage (SceneName.ChooseLevelParapat, "Parapat");
        asynOperation = Application.LoadLevelAsync(SceneName.ChooseLevelParapat.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}

	public void chooseTomok()
	{
        if (FirstSelected == "Tomok")
        {
            if (userScore.GetScore("Parapat", 20) > 0 || CanPlay)
            {
                StartCoroutine("TomokScene");
				transisionAnimator.SetTrigger("Close");
                changeSound.Play();
            }
            else
            {
                AnimatorTomok.SetTrigger("unlock");
            }
        }
        else
        {
            FirstSelected = "Tomok";
            goToSelected = true;
            timeScale = 0;
            startMovePosition = PanelItem.GetComponent<RectTransform>().localPosition.x;
        }
	}
	
	IEnumerator TomokScene()
	{
		SceneCommunication com = new SceneCommunication (SceneName.ChoosePlace);
		com.SendMessage (SceneName.ChooseLevelTomok, "Tomok");
        asynOperation = Application.LoadLevelAsync(SceneName.ChooseLevelTomok.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}

	public void chooseBatuguru()
	{
        if (FirstSelected == "Batuguru")
        {
            if (userScore.GetScore("Parbaba", 20) > 0 || CanPlay)
            {
                StartCoroutine("BatuguruScene");
				transisionAnimator.SetTrigger("Close");
                changeSound.Play();
            }
            else
            {
                AnimatorBatuguru.SetTrigger("unlock");
            }
        }
        else
        {
            FirstSelected = "Batuguru";
            goToSelected = true;
            timeScale = 0;
            startMovePosition = PanelItem.GetComponent<RectTransform>().localPosition.x;
        }
	}
	
	IEnumerator BatuguruScene()
	{
		SceneCommunication com = new SceneCommunication (SceneName.ChoosePlace);
		com.SendMessage (SceneName.ChooseLevelBatuguru, "Batuguru");
        asynOperation = Application.LoadLevelAsync(SceneName.ChooseLevelBatuguru.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}
	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}

    void InputDetector() {
        bool exitStatus = false;
        
        #if UNITY_EDITOR || UNITY_STANDALONE
        exitStatus = Input.GetKeyDown(KeyCode.Escape);
        #elif UNITY_ANDROID
		exitStatus = Input.GetKeyDown (KeyCode.Escape);
        #elif UNITY_WP8
		exitStatus = Input.GetKeyDown (KeyCode.Escape);
        #elif UNITY_IPHONE 
		exitStatus = Input.GetKeyDown (KeyCode.Escape);
        #endif

        if (exitStatus)
        {
            PressBack();
        }
    }

    public void SelectedChange(string Place)
    {
        if (!goToSelected)
        {
            if (FirstSelected != Place)
            {
                FirstSelected = Place;
                changeSound.Play();
            }
        }
    }
}
