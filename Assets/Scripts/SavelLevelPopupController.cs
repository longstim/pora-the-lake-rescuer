using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SavelLevelPopupController : MonoBehaviour {

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
    public class StarScoreText
    {
        public Text Star1;
        public Text Star2;
        public Text Star3;
    }

	public Text fieldName;
    public BubblePackText QuantityText;
    public StarScoreText StarScoreField;
    public GameObject bubblePreview;
    public GameObject loadingMessage;
    public GameObject MessageNotValid;
    public GameObject ButtonSave;

    public BubblePackPlay[] Bubbles;



    Level dataLevel;
    StaticLevel dataStaticLevel;
	// Use this for initialization
	void Start () {
        loadingMessage.SetActive(false);
        MessageNotValid.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show(Level level){
        loadingMessage.SetActive(false);
        MessageNotValid.SetActive(false);
        dataStaticLevel = null;
        dataLevel = level;
        fieldName.text = dataLevel.Name;
        RefreshBubble(dataLevel.Bubbles);
        GetComponent<Animator>().SetTrigger("Show");
	}

    public void Show(StaticLevel level)
    {
        loadingMessage.SetActive(false);
        MessageNotValid.SetActive(false);
        dataLevel = null;
        dataStaticLevel = level;
        fieldName.text = dataStaticLevel.Name;
        RefreshBubble(dataStaticLevel.Bubbles);
        GetComponent<Animator>().SetTrigger("Show");
        QuantityText.White.text = dataStaticLevel.WhiteQuantity.ToString();
        QuantityText.White.text = dataStaticLevel.WhiteQuantity.ToString();
        QuantityText.Orange.text = dataStaticLevel.OrangeQuantity.ToString();

        StarScoreField.Star1.text = dataStaticLevel.ScoreFor1Star.ToString();
        StarScoreField.Star2.text = dataStaticLevel.ScoreFor2Star.ToString();
        StarScoreField.Star3.text = dataStaticLevel.ScoreFor3Star.ToString();
    }

	public void Hide()
	{
        GetComponent<Animator>().SetTrigger("Hide");
	}
	
	public void Save()
	{
        if (fieldName.text != "")
        {
            loadingMessage.SetActive(true);
            if (dataLevel != null) {
                dataLevel.Name = fieldName.text;
                dataLevel.Creator = "NED Studio";
                dataLevel.Bubbles.Clear();
                for (int i = 0; i < bubblePreview.transform.childCount; i++)
                {
                    ItemGelembungController controller = bubblePreview.transform.GetChild(i).gameObject.GetComponent<ItemGelembungController>();
                    dataLevel.Bubbles.Add(new Gelembung(controller.PackageName, controller.Type, controller.Define));
                }
                dataLevel.Save();
            }
            else if (dataStaticLevel != null)
            {
                dataStaticLevel.Name = fieldName.text;
                dataStaticLevel.Creator = "NED Studio";
                dataStaticLevel.Bubbles.Clear();
                for (int i = 0; i < bubblePreview.transform.childCount; i++)
                {
                    ItemGelembungController controller = bubblePreview.transform.GetChild(i).gameObject.GetComponent<ItemGelembungController>();
                    dataStaticLevel.Bubbles.Add(new Gelembung(controller.PackageName, controller.Type, controller.Define));
                }
                dataStaticLevel.ScoreFor1Star = int.Parse(StarScoreField.Star1.text);
                dataStaticLevel.ScoreFor2Star = int.Parse(StarScoreField.Star2.text);
                dataStaticLevel.ScoreFor3Star = int.Parse(StarScoreField.Star3.text);
                dataStaticLevel.SaveLevel();
            }
            Application.LoadLevel(SceneName.LevelEditorCreate.ToString());
        }
        else {
            OnNameChange(fieldName.text);
        }
	}

	public void Cencel()
	{
        Hide();
	}

    private void RefreshBubble(List<Gelembung> daftarGelembung)
    {
        for (int i = 0; i < bubblePreview.transform.childCount; i++)
        {
            Destroy(bubblePreview.transform.GetChild(i));
        }

        foreach (Gelembung gelembung in daftarGelembung)
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
                        objek.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    }
                    else if (gelembung.Type == BubbleType.Red)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].RedPreview);
                        objek.transform.SetParent(bubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    }
                    else if (gelembung.Type == BubbleType.Orange)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].OrangePreview);
                        objek.transform.SetParent(bubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    }
                }
            }
        }
    }

    public void OnNameChange(string name)
    {
        Debug.Log(name);
        if (name.Equals(""))
        {
            MessageNotValid.GetComponent<Text>().text = "Name can not empty!";
            MessageNotValid.SetActive(true);
            ButtonSave.GetComponent<Button>().interactable = false;
        }
        else {
            MessageNotValid.SetActive(false);
            ButtonSave.GetComponent<Button>().interactable = true;
        }
    }

    public void PlusWhiteBubble(int quantity)
    {
        dataStaticLevel.WhiteQuantity += quantity;
        QuantityText.White.text = dataStaticLevel.WhiteQuantity.ToString();
    }

    public void PlusRedBubble(int quantity)
    {
        dataStaticLevel.RedQuantity += quantity;
        QuantityText.Red.text = dataStaticLevel.RedQuantity.ToString();
    }

    public void PlusOrangeBubble(int quantity)
    {
        dataStaticLevel.OrangeQuantity += quantity;
        QuantityText.Orange.text = dataStaticLevel.OrangeQuantity.ToString();
    }
}
