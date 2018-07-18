using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PoraController : MonoBehaviour
{

    public bool isEditMode = false;
    public GameObject bubblePosition;
    public Vector3 MovementSpeed;
    Vector3 moveUntil;
    bool moveUp = false;
    bool moveDown = false;
    bool isPreparation = false;

    float transisionTime = 0.3f;

    private class BubbleScale
    {
        public Vector3 One = new Vector3(0.45f, 0.45f, 1);
        public Vector3 Two = new Vector3(0.55f, 0.55f, 1);
        public Vector3 Three = new Vector3(0.7f, 0.7f, 1);
        public Vector3 Four = new Vector3(0.85f, 0.85f, 1);
        public Vector3 Five = new Vector3(1, 1, 1);
    }
    BubbleScale Scale = new BubbleScale();

    public GameObject BubblePreviewWhite;
    public GameObject BubblePreviewRed;
    public GameObject BubblePreviewOrange;

    public GameObject BubbleStock;

    BubbleController.BubbleSize Size = BubbleController.BubbleSize.One;
    public int SizeInInt
    {
        set
        {
            switch (value)
            {
                case 1: Size = BubbleController.BubbleSize.One; break;
                case 2: Size = BubbleController.BubbleSize.Two; break;
                case 3: Size = BubbleController.BubbleSize.Three; break;
                case 4: Size = BubbleController.BubbleSize.Four; break;
                case 5: Size = BubbleController.BubbleSize.Five; break;
            }
        }
        get
        {
            switch (Size)
            {
                case BubbleController.BubbleSize.One: return 1;
                case BubbleController.BubbleSize.Two: return 2;
                case BubbleController.BubbleSize.Three: return 3;
                case BubbleController.BubbleSize.Four: return 4;
                default: return 5;
            }

        }
    }

    public bool IsPreparation
    {
        get
        {
            return isPreparation;
        }
    }

    // Use this for initialization
    void Start()
    {
        BubblePreviewWhite.transform.localScale = Vector3.zero ;
        BubblePreviewRed.transform.localScale = Vector3.zero;
        BubblePreviewOrange.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    Vector3 positionBeforeMove;
    void Update()
    {
        if (moveUp || moveDown)
        {
            float timeMove =(float) 0.09 * ((moveTo.y - transform.position.y)/(moveTo.y - positionBeforeMove.y));
            if (timeMove < 0.1)
            {
                timeMove = 0.1f;
            }
            Vector3 targetPosition = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, moveTo.y, timeMove), transform.position.z);
            if (targetPosition.y > -423 && targetPosition.y < 472)
            {
                transform.position = targetPosition;
            }
        }
    }


    public bool PrepareShooting()
    {
        if (!isPreparation)
        {
            isPreparation = true;
            Size = BubbleController.BubbleSize.One;
            ItemGelembungController itemGelembungController = BubbleStock.transform.GetChild(0).gameObject.GetComponent<ItemGelembungController>();
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.One;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.One;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.One;
            }
            Invoke("NextBubble", transisionTime);
            return true;
        }
        return false;
    }



    public void NextBubble()
    {
        ItemGelembungController itemGelembungController = BubbleStock.transform.GetChild(0).gameObject.GetComponent<ItemGelembungController>();
        SizeInInt++;
        if (Size == BubbleController.BubbleSize.One)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.One;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.One;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.One;
            }
        }
        else if (Size == BubbleController.BubbleSize.Two)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Two;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Two;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Two;
            }
        }
        else if (Size == BubbleController.BubbleSize.Three)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Three;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Three;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Three;
            }
        }
        else if (Size == BubbleController.BubbleSize.Four)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Four;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Four;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Four;
            }
        }
        else if (Size == BubbleController.BubbleSize.Five)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Five;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Five;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Five;
            }
        }
        if (Size != BubbleController.BubbleSize.Five)
        {
            Invoke("NextBubble", transisionTime);
        }
        else
        {
            Invoke("PrevBubble", transisionTime);
        }

    }

    public void PrevBubble()
    {
        SizeInInt--;
        ItemGelembungController itemGelembungController = BubbleStock.transform.GetChild(0).gameObject.GetComponent<ItemGelembungController>();
        if (Size == BubbleController.BubbleSize.One)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.One;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.One;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.One;
            }
        }
        else if (Size == BubbleController.BubbleSize.Two)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Two;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Two;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Two;
            }
        }
        else if (Size == BubbleController.BubbleSize.Three)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Three;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Three;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Three;
            }
        }
        else if (Size == BubbleController.BubbleSize.Four)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Four;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Four;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Four;
            }
        }
        else if (Size == BubbleController.BubbleSize.Five)
        {
            if (itemGelembungController.Type == BubbleType.White)
            {
                BubblePreviewWhite.transform.localScale = Scale.Five;
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                BubblePreviewRed.transform.localScale = Scale.Five;
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                BubblePreviewOrange.transform.localScale = Scale.Five;
            }
        }

        if (Size != BubbleController.BubbleSize.One)
        {
            Invoke("PrevBubble", transisionTime);
        }
        else
        {
            Invoke("NextBubble", transisionTime);
        }
    }

    public BubbleController.BubbleSize Shoot()
    {
        isPreparation = false;
        CancelInvoke("NextBubble");
        CancelInvoke("PrevBubble");
        BubblePreviewWhite.transform.localScale = Vector3.zero;
        BubblePreviewRed.transform.localScale = Vector3.zero;
        BubblePreviewOrange.transform.localScale = Vector3.zero;
        return Size;
    }

    public Vector2 GetShootPosition()
    {
        return bubblePosition.GetComponent<RectTransform>().position;
    }

    Vector3 moveTo;
    public void MoveDown(BaseEventData data)
    {
        if (isEditMode || PlayManager.State != PlayManager.GameplayState.Playing)
            return;

        PointerEventData pointer = (PointerEventData)data;
        moveTo = Camera.main.ScreenToWorldPoint(pointer.position);
        moveDown = true;
        positionBeforeMove = transform.position;
    }

    public void MoveUp(BaseEventData data)
    {
        if (isEditMode || PlayManager.State != PlayManager.GameplayState.Playing)
            return;

        PointerEventData pointer = (PointerEventData)data;
        moveTo = Camera.main.ScreenToWorldPoint(pointer.position);
        moveUp = true;
        positionBeforeMove = transform.position;
    }

    public void StopMove()
    {
        if (isEditMode || PlayManager.State != PlayManager.GameplayState.Playing)
            return;

        moveUp = false;
        moveDown = false;

    }

    Vector3 lastPosition;
    public void Drag(BaseEventData data)
    {
        if (isEditMode || PlayManager.State != PlayManager.GameplayState.Playing)
            return;

        PointerEventData pointer = (PointerEventData)data;
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, pointer.position.y, 0));
        Vector3 targetPosition = transform.position + (currentPosition - lastPosition);
        if (targetPosition.y > -423 && targetPosition.y < 472)
        {
            transform.position = targetPosition;
        }
        lastPosition = currentPosition;
    }

    public void BeginDrag(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData)data;
        lastPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, pointer.position.y, 0));
    }
}
