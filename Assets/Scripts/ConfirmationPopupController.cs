using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmationPopupController : MonoBehaviour {

	public delegate void Callback();

	public Text textMessage; 

	private Callback yesCallback;
	private Callback noCallback;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show(string message, Callback yesCallback, Callback noCallback)
	{
		textMessage.text = message;
		this.yesCallback = yesCallback;
		this.noCallback = noCallback;
        GetComponent<Animator>().SetTrigger("Show");
	}

	public void Hide()
	{
        GetComponent<Animator>().SetTrigger("Hide");
	}

	public bool IsShowing()
	{
        return !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hide") ;
	}

	public void Yes()
	{
		this.yesCallback ();
		Hide ();
	}

	public void No()
	{
		this.noCallback ();
		Hide ();
	}
}
