using UnityEngine;
using System.Collections;

public class StorePopupController : MonoBehaviour {
    
    public delegate void Callback();

    private Callback callback;
    Animator animator;

    public GameObject PanelBubble;
    public GameObject PanelTrash;
    public GameObject PanelCustomization;
    public GameObject PanelGoldandGems;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Show(string itemStore)
    {
        if (itemStore == "Gold" || itemStore == "Gems")
        {
            PanelBubble.SetActive(false);
            PanelTrash.SetActive(false);
            PanelCustomization.SetActive(false);
            PanelGoldandGems.SetActive(true);
        }
        else if (itemStore == "Bubble")
        {
            PanelBubble.SetActive(true);
            PanelTrash.SetActive(false);
            PanelCustomization.SetActive(false);
            PanelGoldandGems.SetActive(false);
        }
        else if (itemStore == "Rubbish" || itemStore == "Barrier")
        {
            PanelBubble.SetActive(false);
            PanelTrash.SetActive(true);
            PanelCustomization.SetActive(false);
            PanelGoldandGems.SetActive(false);
        }
        else if (itemStore == "Customization")
        {
            PanelBubble.SetActive(false);
            PanelTrash.SetActive(false);
            PanelCustomization.SetActive(true);
            PanelGoldandGems.SetActive(false);
        }
        animator.SetTrigger("Show");
    }

    public void Show(string message, Callback callback)
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Ok()
    {
        callback();
        Hide();
    }
}
