using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragAndInstansiate: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject itemInstansiated;
    public Transform dragLayer;
    public static GameObject itemBeingDragged;

    void Start()
    {

    }

    public static bool isDragging()
    {
        return itemBeingDragged != null;
    }

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = (GameObject)Instantiate(itemInstansiated);

        itemBeingDragged.transform.SetParent(dragLayer);
        itemBeingDragged.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
        itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, itemBeingDragged.transform.position.y, 30);
        itemBeingDragged.transform.localScale = new Vector3(1, 1, 1);

    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        //itemBeingDragged.transform.position = eventData.position;
        itemBeingDragged.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
        itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, itemBeingDragged.transform.position.y, 30);
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemBeingDragged != null)
        {
            Destroy(itemBeingDragged);
        }

    }

    #endregion
}
