using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DropToDestroyHandler : MonoBehaviour,IDropHandler {

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Image>().enabled = transform.childCount > 0;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragHandler.itemBeingDragged)
        {
            if (DragHandler.itemBeingDragged.GetComponent<DragHandler>())
            {
                DragHandler.itemBeingDragged.GetComponent<DragHandler>().dragBack.OnEndDragCallback(DragHandler.itemBeingDragged);
            }
            Destroy(DragHandler.itemBeingDragged);
        }
    }
}
