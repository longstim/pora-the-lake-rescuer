using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessagePopupController : MonoBehaviour {

	public delegate void Callback();

	public Text textMessage;

	private Callback callback;
    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(string message)
    {
        textMessage.text = message;
        animator.SetTrigger("Show");
    }

    public void Show(string message, Callback callback)
    {
        textMessage.text = message;
        animator.SetTrigger("Show");
        this.callback = callback;
    }

   

	public void Ok()
	{
		callback ();

	}
}
