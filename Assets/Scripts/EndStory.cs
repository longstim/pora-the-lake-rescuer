using UnityEngine;
using System.Collections;

public class EndStory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void GoToMain()
	{
		Application.LoadLevel (SceneName.Main.ToString());
	}
}
