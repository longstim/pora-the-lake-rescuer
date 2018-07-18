using UnityEngine;
using System.Collections;

public class DragCallback : MonoBehaviour {

    bool statuSlideMenu;
    public Animator SlideMenuAnimator;

    public GameObject[] ShowOnDrag;
    public GameObject[] HideOnDrag;

    public LevelEditorCreateManager levelEditorCreateManager;

    public void Start()
    {
        for (int i = 0; i < HideOnDrag.Length; i++)
        {
            HideOnDrag[i].SetActive(true);
        }
        for (int i = 0; i < ShowOnDrag.Length; i++)
        {
            ShowOnDrag[i].SetActive(false);
        }
    }

    public void OnBeginDragCallback(GameObject item)
    {
        statuSlideMenu = SlideMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Show");
        if (statuSlideMenu)
        {
            SlideMenuAnimator.SetTrigger("Hide");
        }
        for (int i = 0; i < HideOnDrag.Length; i++)
        {
            HideOnDrag[i].SetActive(false);
        }
        for (int i = 0; i < ShowOnDrag.Length; i++)
        {
            ShowOnDrag[i].SetActive(true);
        }
    }

    public void OnDragCallback(GameObject item)
    {
        levelEditorCreateManager.ValidatedStatus = false;
    }

    public void OnEndDragCallback(GameObject item)
    {
        if (statuSlideMenu)
        {
            SlideMenuAnimator.SetTrigger("Show");
        }
        for (int i = 0; i < HideOnDrag.Length; i++)
        {
            HideOnDrag[i].SetActive(true);
        }
        for (int i = 0; i < ShowOnDrag.Length; i++)
        {
            ShowOnDrag[i].SetActive(false);
        }
    }
}
