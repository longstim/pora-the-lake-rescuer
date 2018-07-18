using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirtualCoinStore : MonoBehaviour {

    public string Id;
    public string Name;
    public int Gold;
    public int Price;

    public Image icon;
    public Text textName;
    public Text textPrice;

    public ConfirmationBuyingPopup confirmationPopup;
    public MessagePopupController messagePopup;
	// Use this for initialization
	void Start () {
        textName.text = Name;
        textPrice.text = Price.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        confirmationPopup.Show(icon.sprite,Name,Price + " Pearls", YesToBuy, NoToBuy);
    }

    private void YesToBuy()
    {
        UserStockData userStock = UserStockData.Load();

        if (userStock.PlusMinGem(-Price))
        {
            if (userStock.PlusMinGold(Gold))
            {
                messagePopup.Show("You have bought " + Name);
                string command = "{";
                command += "action:BUY_COIN";
                command += ",item:" + Name;
                command += "}";
                ServerStatistic.DoRequest(command);
            }
            else {
                userStock.PlusMinGem(Price);
            }
        }else{
            messagePopup.Show("You need " + (Price - userStock.Gems) + " more pearls!");
        }
    }

    private void NoToBuy()
    {

    }

}
