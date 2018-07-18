using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainManager : MonoBehaviour {

	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;
    public ConfirmationPopupController confirmationPopup;
	
	// Use this for initialization
	void Start () {
        string command = "{";
        command += "action:OPEN_MAIN";
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
            if (!confirmationPopup.IsShowing())
            {
                confirmationPopup.Show("Do you want to quit the game?", YesExit, NoExit);
            }
		}
	}

    public void YesExit()
    {
        Application.Quit();
    }

    public void NoExit()
    {

    }

	public void LoadingScene()
	{
		StartCoroutine ("LoadScene");

	}

	IEnumerator LoadScene()
	{
		Debug.Log ("Masuk");
		asynOperation = Application.LoadLevelAsync (SceneName.ChooseMode.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}

	public void StoreScene()
	{
		StartCoroutine ("LoadStoreScene");
		
	}
	
	IEnumerator LoadStoreScene()
	{
		asynOperation = Application.LoadLevelAsync (SceneName.Store.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}
	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}
}
