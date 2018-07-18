using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DropBubbleHandler : MonoBehaviour
{
    [HideInInspector]
    public ChooseBubbleController chooseBubbleController;
    public ReloadPopupController reloadPopupController;
    Sprite undefineSprite;
    void Start()
    {
        undefineSprite = GetComponent<Image>().sprite;
    }

    public void OnDrop(BaseEventData eventData)
    {
        BeUndefine();

        if (chooseBubbleController)
        {
            ItemGelembungController itemGelembung = GetComponent<ItemGelembungController>();

            ItemGelembungController itemGelembungDragged = DragAndInstansiate.itemBeingDragged.GetComponent<ItemGelembungController>();
            if (itemGelembungDragged.Type == BubbleType.White && chooseBubbleController.WhiteQuantity > 0)
            {
                itemGelembung.Type = itemGelembungDragged.Type;
                itemGelembung.PackageName = itemGelembungDragged.PackageName;
                itemGelembung.Define = true;
                itemGelembung.GetComponent<Image>().sprite = itemGelembungDragged.GetComponent<Image>().sprite;
                chooseBubbleController.WhiteQuantity--;
                Destroy(DragAndInstansiate.itemBeingDragged);
                DragAndInstansiate.itemBeingDragged = null;
            }
            else if (itemGelembungDragged.Type == BubbleType.Red && chooseBubbleController.RedQuantity > 0)
            {
                itemGelembung.Type = itemGelembungDragged.Type;
                itemGelembung.PackageName = itemGelembungDragged.PackageName;
                itemGelembung.Define = true;
                itemGelembung.GetComponent<Image>().sprite = itemGelembungDragged.GetComponent<Image>().sprite;
                chooseBubbleController.RedQuantity--;
                Destroy(DragAndInstansiate.itemBeingDragged);
                DragAndInstansiate.itemBeingDragged = null;
            }
            else if (itemGelembungDragged.Type == BubbleType.Orange && chooseBubbleController.OrangeQuantity > 0)
            {
                itemGelembung.Type = itemGelembungDragged.Type;
                itemGelembung.PackageName = itemGelembungDragged.PackageName;
                itemGelembung.Define = true;
                itemGelembung.GetComponent<Image>().sprite = itemGelembungDragged.GetComponent<Image>().sprite;
                chooseBubbleController.OrangeQuantity--;
                Destroy(DragAndInstansiate.itemBeingDragged);
                DragAndInstansiate.itemBeingDragged = null;
            }
        }else if(reloadPopupController)
        {
            ItemGelembungController itemGelembung = GetComponent<ItemGelembungController>();

            ItemGelembungController itemGelembungDragged = DragAndInstansiate.itemBeingDragged.GetComponent<ItemGelembungController>();
            if (itemGelembungDragged.Type == BubbleType.White && reloadPopupController.WhiteBubbleStock > 0)
            {
                itemGelembung.Type = itemGelembungDragged.Type;
                itemGelembung.PackageName = itemGelembungDragged.PackageName;
                itemGelembung.Define = true;
                itemGelembung.GetComponent<Image>().sprite = itemGelembungDragged.GetComponent<Image>().sprite;
                reloadPopupController.WhiteBubbleStock--;
                Destroy(DragAndInstansiate.itemBeingDragged);
                DragAndInstansiate.itemBeingDragged = null;
            }
            else if (itemGelembungDragged.Type == BubbleType.Red && reloadPopupController.RedBubbleStock > 0)
            {
                itemGelembung.Type = itemGelembungDragged.Type;
                itemGelembung.PackageName = itemGelembungDragged.PackageName;
                itemGelembung.Define = true;
                itemGelembung.GetComponent<Image>().sprite = itemGelembungDragged.GetComponent<Image>().sprite;
                reloadPopupController.RedBubbleStock--;
                Destroy(DragAndInstansiate.itemBeingDragged);
                DragAndInstansiate.itemBeingDragged = null;
            }
            else if (itemGelembungDragged.Type == BubbleType.Orange && reloadPopupController.OrangeBubbleStock > 0)
            {
                itemGelembung.Type = itemGelembungDragged.Type;
                itemGelembung.PackageName = itemGelembungDragged.PackageName;
                itemGelembung.Define = true;
                itemGelembung.GetComponent<Image>().sprite = itemGelembungDragged.GetComponent<Image>().sprite;
                reloadPopupController.OrangeBubbleStock--;
                Destroy(DragAndInstansiate.itemBeingDragged);
                DragAndInstansiate.itemBeingDragged = null;
            }
        }
    }

    public void BeUndefine()
    {
        if (chooseBubbleController)
        {
            ItemGelembungController itemGelembung = GetComponent<ItemGelembungController>();
            if (itemGelembung.Define)
            {
                itemGelembung.Define = false;
                itemGelembung.GetComponent<Image>().sprite = undefineSprite;
                if (itemGelembung.Type == BubbleType.White)
                {
                    chooseBubbleController.WhiteQuantity++;
                }
                else if (itemGelembung.Type == BubbleType.Red)
                {
                    chooseBubbleController.RedQuantity++;
                }
                else if (itemGelembung.Type == BubbleType.Orange)
                {
                    chooseBubbleController.OrangeQuantity++;
                }
            }
        }
        else if (reloadPopupController)
        {
            ItemGelembungController itemGelembung = GetComponent<ItemGelembungController>();
            if (itemGelembung.Define)
            {
                itemGelembung.Define = false;
                itemGelembung.GetComponent<Image>().sprite = undefineSprite;
                if (itemGelembung.Type == BubbleType.White)
                {
                    reloadPopupController.WhiteBubbleStock++;
                }
                else if (itemGelembung.Type == BubbleType.Red)
                {
                    reloadPopupController.RedBubbleStock++;
                }
                else if (itemGelembung.Type == BubbleType.Orange)
                {
                    reloadPopupController.OrangeBubbleStock++;
                }
            }
        }
    }
}
