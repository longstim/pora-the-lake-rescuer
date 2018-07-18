using UnityEngine;
using System.Collections;

public class ChooseModeManager : MonoBehaviour {


	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;

    public AudioSource[] audioSourcesEffect;

	// Use this for initialization
	void Start () {
        UserSoundConfig config = UserSoundConfig.Load();

        for (int i = 0; i < audioSourcesEffect.Length; i++)
        {
            audioSourcesEffect[i].volume = config.SoundVolume;
            audioSourcesEffect[i].mute = !config.SoundStatus;
        }

        string command = "{";
        command += "action:OPEN_CHOOSEMODE";
        command += "}";
        ServerStatistic.DoRequest(command);
	}
	
	// Update is called once per frame
	void Update () {
	

		if (asynOperation != null) {
			if (asynOperation.progress >= 0.9f && transisionAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Loading")) {
				Invoke ("ActivateScene", 1f);
				
			}
		}else if (Input.GetKey(KeyCode.Escape))
		{
			transisionAnimator.SetTrigger("Close");
			back();
		}
	}

	public void showLevelSystem()
	{
		StartCoroutine ("LoadScene");
		
	}
	
	IEnumerator LoadScene()
	{

		asynOperation = Application.LoadLevelAsync (SceneName.ChoosePlace.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
		Debug.Log ("Masuk");
	}
	public void back()
	{
		StartCoroutine ("BackScene");
		
	}
	
	IEnumerator BackScene()
	{
		
		asynOperation = Application.LoadLevelAsync (SceneName.Main.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
		Debug.Log ("Masuk");
	}

	public void showLevelEditor()
	{
		StartCoroutine ("LevelEditorScene");
		
	}
	
	IEnumerator LevelEditorScene()
	{
		
		asynOperation = Application.LoadLevelAsync (SceneName.LevelEditorMenu.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
		Debug.Log ("Masuk");
	}

	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}


}
