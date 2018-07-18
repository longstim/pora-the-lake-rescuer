using UnityEngine;
using System.Collections;

public class StoreManager : MonoBehaviour {


	public GameObject paket1;
	public GameObject paket2;
	public GameObject paket3;

    public AudioSource[] audioSourcesEffect;

	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;
	
	// Use this for initialization
	void Start () {
        new UserItemStock().Load();
        UserSoundConfig config = UserSoundConfig.Load();
        for (int i = 0; i < audioSourcesEffect.Length; i++)
        {
            audioSourcesEffect[i].volume = config.SoundVolume;
            audioSourcesEffect[i].mute = !config.SoundStatus;
        }

        string command = "{";
        command += "action:OPEN_STORE";
        command += "}";
        ServerStatistic.DoRequest(command);
	}
	
	// Update is called once per frame
	void Update () {
		if (asynOperation != null) {
			if (asynOperation.progress >= 0.9f && transisionAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Loading")) {
				Invoke ("ActivateScene", 1f);		
			}
		}else if (Input.GetKey (KeyCode.Escape)) {
			transisionAnimator.SetTrigger ("Close");
			LoadingScene();
		}
	}
	
	public void LoadingScene()
	{
		StartCoroutine ("LoadScene");
		
	}
	
	IEnumerator LoadScene()
	{
		asynOperation = Application.LoadLevelAsync (SceneName.Main.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}
	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}

    public void showPaket1()
    {

        paket1.SetActive(true);
        paket2.SetActive(false);
        paket3.SetActive(false);
    }

    public void showPaket2()
    {

        paket1.SetActive(false);
        paket2.SetActive(true);
        paket3.SetActive(false);
    }

    public void showPaket3()
    {

        paket1.SetActive(false);
        paket2.SetActive(false);
        paket3.SetActive(true);
    }
}
