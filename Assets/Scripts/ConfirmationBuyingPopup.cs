using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmationBuyingPopup : MonoBehaviour {

    public delegate void Callback();

    public Image image;
    public Text Name;
    public Text Price;

    private Callback yesCallback;
    private Callback noCallback;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(Sprite icon, string name, string price, Callback yesCallback, Callback noCallback)
    {
        this.yesCallback = yesCallback;
        this.noCallback = noCallback;
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Show"))
        {
            GetComponent<Animator>().SetTrigger("Show");
        }
        image.sprite = icon;
        image.SetNativeSize();
        Name.text = name;
        Price.text = price;
    }

    public void Hide()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hide"))
        {
            GetComponent<Animator>().SetTrigger("Hide");
        }
    }

    public bool IsShowing()
    {
        return GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Show");
    }

    public void Yes()
    {
        if (this.yesCallback != null)
        {
            this.yesCallback();
        }
        Hide();
    }

    public void No()
    {
        if (this.noCallback != null)
        {
            this.noCallback();
        }
        Hide();
    }
}
