using UnityEngine;
using System.Collections;

public class LoadingChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("ChangeScene", 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeScene()
	{
		Application.LoadLevel ("SplashPora");
	}
}
