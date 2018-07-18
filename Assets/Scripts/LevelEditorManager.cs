using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEditorManager : MonoBehaviour {

	[System.Serializable]
	public class buttonShow
	{
		public GameObject show;
		public GameObject hide;
	}

	[System.Serializable]
	public class tabLeaderBoard
	{
		public GameObject showTabPub;
		public GameObject showTabPvt;
		public GameObject btnActivePub;
		public GameObject btnPassivePub;
		public GameObject btnActivePvt;
		public GameObject btnPassivePvt;
		public GameObject btnUserPub;
		public GameObject btnUserPvt;
	}


	public GameObject challange;
	public GameObject ShowChallange;
	public GameObject showLevel;
	public GameObject customPora;
	public buttonShow btnShowHide;
	public tabLeaderBoard tabPubPvt;


	public void showLeader()
	{
		btnShowHide.show.SetActive (false);
		btnShowHide.hide.SetActive (true);
	}
	public void hideLeader()
	{
		btnShowHide.show.SetActive (true);
		btnShowHide.hide.SetActive (false);

	}

	public void tabShowpublic()
	{
		tabPubPvt.showTabPub.SetActive (true);
		tabPubPvt.showTabPvt.SetActive (false);
		tabPubPvt.btnActivePub.SetActive (true);
		tabPubPvt.btnActivePvt.SetActive (false);
		tabPubPvt.btnPassivePub.SetActive (false);
		tabPubPvt.btnPassivePvt.SetActive (true);
		tabPubPvt.btnUserPub.SetActive (true);
		tabPubPvt.btnUserPvt.SetActive (false);
	}

	public void tabShowprivate()
	{
		tabPubPvt.showTabPub.SetActive (false);
		tabPubPvt.showTabPvt.SetActive (true);
		tabPubPvt.btnActivePub.SetActive (false);
		tabPubPvt.btnActivePvt.SetActive (true);
		tabPubPvt.btnPassivePub.SetActive (true);
		tabPubPvt.btnPassivePvt.SetActive (false);
		tabPubPvt.btnUserPub.SetActive (false);
		tabPubPvt.btnUserPvt.SetActive (true);
	}
	public void showPrePlayLevel()
	{
		showLevel.SetActive (true);
	}

	public void showCustom()
	{
		customPora.SetActive (true);
	}

	public void showPopUpChallange()
	{
		challange.SetActive (true);
	}

	public void challangeConfirm()
	{
		ShowChallange.SetActive (true);
		challange.SetActive (false);
	}
	void Start () {
	
	}
	
	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;
	// Update is called once per frame
	void Update () {

		if (asynOperation != null) {
			if (asynOperation.progress >= 0.9f && transisionAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Loading")) {
				Invoke ("ActivateScene", 1f);		
			}
		}else if (Input.GetKey (KeyCode.Escape)) {
			LoadingScene();
		}
	
	}

	public void LoadingScene()
	{
		transisionAnimator.SetTrigger("Close");
		StartCoroutine ("LoadScene");
		
	}
	
	IEnumerator LoadScene()
	{
		asynOperation = Application.LoadLevelAsync (SceneName.ChooseMode.ToString());
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}
	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}
}
