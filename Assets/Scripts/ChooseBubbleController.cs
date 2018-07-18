using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChooseBubbleController : MonoBehaviour {

    public delegate void ChooseBubbleCallback(List<Gelembung> dataLevel);

    [System.Serializable]
    public class BubblePackPlay
    {
        public string PackageName;
        public GameObject WhitePreview;
        public GameObject RedPreview;
        public GameObject OrangePreview;
    }

    [System.Serializable]
    public class BubblePackText
    {
        public Text White;
        public Text Red;
        public Text Orange;
    }

    [System.Serializable]
    public class BubblePackItem
    {
        public GameObject WhiteBlank;
        public GameObject White;
        public GameObject RedBlank;
        public GameObject Red;
        public GameObject OrangeBlank;
        public GameObject Orange;

        public GameObject WhiteBlank_Anim;
        public GameObject White_Anim;
        public GameObject RedBlank_Anim;
        public GameObject Red_Anim;
        public GameObject OrangeBlank_Anim;
        public GameObject Orange_Anim;
    }
    public BubblePackPlay[] Bubbles;
    public BubblePackText QuantityText;
    public BubblePackItem BubbleItem;
    public GameObject BubbleUndefine;
    
    public GameObject bubblePreview;

    public GameObject BtnShoot;
    StaticLevel dataLevel;
    ChooseBubbleCallback callback;
	// Use this for initialization

    [HideInInspector]
    public int whiteQuantity = 0;
    public int WhiteQuantity
    {
        get { return whiteQuantity; }
        set
        {
            whiteQuantity = value;
            AfterChangeQuantity();
        }
    }
    [HideInInspector]
    public int redQuantity = 0;
    public int RedQuantity
    {
        get { return redQuantity; }
        set
        {
            redQuantity = value;
            AfterChangeQuantity();
        }
    }
    [HideInInspector]
    public int orangeQuantity = 0;
    public int OrangeQuantity {
        get { return orangeQuantity; }
        set { 
            orangeQuantity = value;
            AfterChangeQuantity();
        }
    }

    public Button OkButton;
	void Start () {

	}

    bool isShowing = false;
	// Update is called once per frame
	void Update () {
        
	}

    public void AfterChangeQuantity()
    {
        QuantityText.White.text = WhiteQuantity.ToString();
        QuantityText.Red.text = RedQuantity.ToString();
        QuantityText.Orange.text = OrangeQuantity.ToString();

        BubbleItem.White.SetActive(WhiteQuantity > 0);
        BubbleItem.Red.SetActive(RedQuantity > 0);
        BubbleItem.Orange.SetActive(OrangeQuantity > 0);

        BubbleItem.White_Anim.SetActive(WhiteQuantity > 0);
        BubbleItem.Red_Anim.SetActive(RedQuantity > 0);
        BubbleItem.Orange_Anim.SetActive(OrangeQuantity > 0);

        BubbleItem.WhiteBlank_Anim.SetActive(!(WhiteQuantity > 0));
        BubbleItem.RedBlank_Anim.SetActive(!(RedQuantity > 0));
        BubbleItem.OrangeBlank_Anim.SetActive(!(OrangeQuantity > 0));

        bool allDefine = true;
        for (int i = 0; i<bubblePreview.transform.childCount; i++)
        {
            allDefine = allDefine && bubblePreview.transform.GetChild(i).GetComponent<ItemGelembungController>().Define;
            if (!allDefine)
                break;
        }
        OkButton.interactable = allDefine && bubblePreview.transform.childCount > 0;
    }

    /*
    public void Show(Level dataLevel, ChooseBubbleCallback callback)
    {
        this.dataLevel = dataLevel;
        RefreshBubble(dataLevel.Bubbles);
        GetComponent<Animator>().SetTrigger("Show");
        this.callback = callback;
        isShowing = true;
    }
     * */

    public void Show(StaticLevel dataLevel, ChooseBubbleCallback callback)
    {
        this.dataLevel = dataLevel;
        this.WhiteQuantity = dataLevel.WhiteQuantity;
        this.RedQuantity = dataLevel.RedQuantity;
        this.OrangeQuantity = dataLevel.OrangeQuantity;
        RefreshBubble(dataLevel.Bubbles);
        GetComponent<Animator>().SetTrigger("Show");
        this.callback = callback;
        isShowing = true;
    }

    public void Ok()
    {
        BtnShoot.SetActive(true);
        isShowing = false;
        List<Gelembung> listOfBubbble = new List<Gelembung>();
        string bubbleString ="";
        for (int i = 0; i < bubblePreview.transform.childCount; i++)
        {
            ItemGelembungController controller = bubblePreview.transform.GetChild(i).gameObject.GetComponent<ItemGelembungController>();
            listOfBubbble.Add(new Gelembung(controller.PackageName, controller.Type));
            bubbleString+=controller.Type.ToString();
        }
        string command = "{";
        command += "action:PlAY_LEVEL";
        command += ",place:" + dataLevel.Place;
        command += ",level:" + dataLevel.Level;
        command += ",bubbleUsed:" + bubbleString;
        command += ",status:CHOOSE_BUBBLE";
        command += "}";
        ServerStatistic.DoRequest(command);
        callback(listOfBubbble);
        GetComponent<Animator>().SetTrigger("Hide");
    }

    private void RefreshBubble(List<Gelembung> daftarGelembung)
    {
        foreach (Gelembung gelembung in daftarGelembung)
        {
            if (!gelembung.Define)
            {
                GameObject objek = (GameObject)Instantiate(BubbleUndefine);
                objek.transform.SetParent(bubblePreview.transform);
                objek.transform.localScale = new Vector3(1, 1, 1);
                objek.transform.localPosition = new Vector3(objek.transform.localPosition.x, objek.transform.localPosition.y, 0);
                objek.GetComponent<DropBubbleHandler>().chooseBubbleController = this;
                objek.GetComponent<ItemGelembungController>().Define = false;
                objek.GetComponent<ItemGelembungController>().PackageName = gelembung.PackageName;
                objek.GetComponent<ItemGelembungController>().Type = gelembung.Type;
            }
            else
            {
                for (int i = 0; i < Bubbles.Length; i++)
                {
                    if (gelembung.PackageName == Bubbles[i].PackageName)
                    {
                        if (gelembung.Type == BubbleType.White)
                        {
                            GameObject objek = (GameObject)Instantiate(Bubbles[i].WhitePreview);
                            objek.transform.SetParent(bubblePreview.transform);
                            objek.transform.localScale = new Vector3(1, 1, 1);
                            objek.transform.localPosition = new Vector3(objek.transform.localPosition.x, objek.transform.localPosition.y, 0);
                            Destroy(objek.GetComponent<Toggle>());
                        }
                        else if (gelembung.Type == BubbleType.Red)
                        {
                            GameObject objek = (GameObject)Instantiate(Bubbles[i].RedPreview);
                            objek.transform.SetParent(bubblePreview.transform);
                            objek.transform.localScale = new Vector3(1, 1, 1);
                            objek.transform.localPosition = new Vector3(objek.transform.localPosition.x, objek.transform.localPosition.y, 0);
                            Destroy(objek.GetComponent<Toggle>());
                        }
                        else if (gelembung.Type == BubbleType.Orange)
                        {
                            GameObject objek = (GameObject)Instantiate(Bubbles[i].OrangePreview);
                            objek.transform.SetParent(bubblePreview.transform);
                            objek.transform.localScale = new Vector3(1, 1, 1);
                            objek.transform.localPosition = new Vector3(objek.transform.localPosition.x, objek.transform.localPosition.y, 0);
                            Destroy(objek.GetComponent<Toggle>());
                        }
                    }
                }
            }
        }
    }
}
