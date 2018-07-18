using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DropHandler : MonoBehaviour,IDropHandler{

    public Transform RubbishLayer;
    public Transform BarrierLayer;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().enabled = transform.parent.childCount > 2;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragAndInstansiateHandler.isDragging() && DragAndInstansiateHandler.itemBeingDragged.GetComponent<RubbishController>())
        {
            DragAndInstansiateHandler.itemBeingDragged.transform.SetParent(RubbishLayer);
            DragAndInstansiateHandler.itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (DragAndInstansiateHandler.isDragging() && DragAndInstansiateHandler.itemBeingDragged.GetComponent<BarrierController>())
        {
            DragAndInstansiateHandler.itemBeingDragged.transform.SetParent(BarrierLayer);
            DragAndInstansiateHandler.itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (DragHandler.isDragging() && DragHandler.itemBeingDragged.GetComponent<RubbishController>())
        {
            DragHandler.itemBeingDragged.transform.SetParent(RubbishLayer);
            DragHandler.itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (DragHandler.isDragging() && DragHandler.itemBeingDragged.GetComponent<BarrierController>())
        {
            DragHandler.itemBeingDragged.transform.SetParent(BarrierLayer);
            DragHandler.itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
