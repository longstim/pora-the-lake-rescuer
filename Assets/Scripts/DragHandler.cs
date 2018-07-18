using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour,IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject itemBeingDragged;
    public Transform dragLayer;
    Vector3 starPosition;
    Vector3 lastPosition;

    public DragCallback dragBack;

    public void OnPointerDown(PointerEventData data)
    {
        if(gameObject.tag =="Rubbish" && !GetComponent<RubbishController>().isSelected)
        {
            GetComponent<RubbishController>().Select();
        }
        else if (gameObject.tag == "Barrier" && !GetComponent<BarrierController>().isSelected)
        {
            GetComponent<BarrierController>().Select();
        }
    }

    public static bool isDragging()
    {
        return itemBeingDragged != null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y)); ;
        lastPosition = new Vector3(lastPosition.x, lastPosition.y, 0);
        starPosition = transform.position;

        itemBeingDragged = gameObject;
        itemBeingDragged.transform.SetParent(dragLayer);
        itemBeingDragged.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
        itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, itemBeingDragged.transform.position.y, 30);
        itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = false;

        dragBack.OnBeginDragCallback(gameObject);

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
        currentPosition = new Vector3(currentPosition.x, currentPosition.y, 30);
        itemBeingDragged.transform.position += (currentPosition - lastPosition);
        lastPosition = currentPosition;

        dragBack.OnDragCallback(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemBeingDragged && itemBeingDragged.transform.parent == dragLayer)
        {
            itemBeingDragged.transform.position = starPosition;
        }
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        dragBack.OnEndDragCallback(gameObject);
        Debug.Log("asdas");
    }
}
