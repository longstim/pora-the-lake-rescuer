using UnityEngine;
using System.Collections;

public class FailPopupController : MonoBehaviour {

	public enum ButtonName{
		Menu,
		Replay,
		BackToEditor,
	}

	public delegate void Callback(ButtonName button);

	Callback callback;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowForPlayingStaticLevel(Callback callback)
	{
		this.callback = callback;
        GetComponent<Animator>().SetTrigger("Show");
	}
	
	public void ShowForPlayingEditedLevel(Callback callback)
	{
		this.callback = callback;
        GetComponent<Animator>().SetTrigger("Show");
	}
	
	public void ShowForValidateLevel(Callback callback)
	{
		this.callback = callback;
        GetComponent<Animator>().SetTrigger("Show");
	}

	public void Hide()
	{
		
	}

	public void Menu()
	{
		callback (ButtonName.Menu);
		Hide ();
	}

	public void Replay()
	{
		callback (ButtonName.Replay);
		Hide ();
	}

	public void Editor()
	{
		callback (ButtonName.BackToEditor);
		Hide ();
	}
}
