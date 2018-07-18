using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DropBubbleAutoHandler : MonoBehaviour {
    public GameObject BubblePreview;
    
    public void OnDrop(BaseEventData eventData)
    {
        for (int i = 0; i < BubblePreview.transform.childCount; i++)
        { 
            DropBubbleHandler dropHandler= BubblePreview.transform.GetChild(i).GetComponent<DropBubbleHandler>();
            ItemGelembungController itemGelembung = BubblePreview.transform.GetChild(i).GetComponent<ItemGelembungController>();
            if (dropHandler && itemGelembung.Define == false)
            {
                dropHandler.OnDrop(eventData);
                break;
            }
        }
    }

}
