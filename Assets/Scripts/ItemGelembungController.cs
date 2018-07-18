using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemGelembungController : MonoBehaviour,IPointerDownHandler {

    public delegate void Callback(ItemGelembungController itemGel);

	public string PackageName;
	public BubbleType Type;
    [HideInInspector]
    public bool Define = true;

    public Callback deleteCallback = null;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<CanvasGroup>())
        {
            if (deleteCallback != null)
            {
                deleteCallback(this);
            }
            Destroy(gameObject);
        }
    }

    public void OnClickToogleDefine(bool newStatus)
    {
        Define = newStatus;
    }
}
