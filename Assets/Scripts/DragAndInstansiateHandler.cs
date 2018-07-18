using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragAndInstansiateHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject itemInstansiated;
    public Transform dragLayer;
    public static GameObject itemBeingDragged;

    public DragCallback dragBack;

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
        itemBeingDragged.AddComponent<CanvasGroup>();

        if (itemBeingDragged.tag == "Rubbish")
        {
            itemBeingDragged.GetComponent<RubbishController>().Select();
            itemBeingDragged.GetComponent<RubbishController>().IsEditMode = true;
        }
        else if (itemBeingDragged.tag == "Barrier")
        {
            itemBeingDragged.GetComponent<BarrierController>().Select();
            itemBeingDragged.GetComponent<BarrierController>().IsEditMode = true;
        }

        itemBeingDragged.transform.SetParent(dragLayer);
        itemBeingDragged.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x,eventData.position.y));
        itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, itemBeingDragged.transform.position.y,30);
        itemBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = false;

        dragBack.OnBeginDragCallback(gameObject);
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        //itemBeingDragged.transform.position = eventData.position;
        itemBeingDragged.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,0));
        itemBeingDragged.transform.position = new Vector3(itemBeingDragged.transform.position.x, itemBeingDragged.transform.position.y, 30);

        dragBack.OnDragCallback(gameObject);
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemBeingDragged.transform.parent == dragLayer)
        {
            Destroy(itemBeingDragged);
        }
        else
        {
            DragHandler draghander = itemBeingDragged.AddComponent<DragHandler>();
            draghander.dragLayer = dragLayer;
            draghander.dragBack = dragBack;
        }

        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        dragBack.OnEndDragCallback(gameObject);
    }

    #endregion
}
