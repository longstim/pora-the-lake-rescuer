using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserStockController : MonoBehaviour {

    public Text GemsText;
    public Text GoldText;
    public Text WhiteBubbleText;
    public Text RedBubbleText;
    public Text OrangeBubbleText;

	// Use this for initialization
	void Start () {
        UserStockData.Load();
	}
	
	// Update is called once per frame
	void Update () {
        GemsText.text = UserStockData.GemsStock.ToString("#,0");
        GoldText.text = UserStockData.GoldStock.ToString("#,0");
        if (WhiteBubbleText)
        {
            WhiteBubbleText.text = UserStockData.WhiteBubbleStock.ToString("#,0");
        }
        if (RedBubbleText)
        {
            RedBubbleText.text = UserStockData.RedBubbleStock.ToString("#,0");
        }
        if (OrangeBubbleText)
        {
            OrangeBubbleText.text = UserStockData.OrangeBubbleStock.ToString("#,0");
        }
	}
}
