using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class PoraSceneLoader : MonoBehaviour {

	public bool IsLoadingFinished = false;
	public float LoadingProgress = 0;
    public Animator anim;
	// Use this for initialization

	void Start () {
        anim.SetTrigger("splash");
        Invoke("ChangeScene", 4f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ChangeScene()
	{
		Application.LoadLevel (SceneName.SplashPora.ToString());
	}
}
