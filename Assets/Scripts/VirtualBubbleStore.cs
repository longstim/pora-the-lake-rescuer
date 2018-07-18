using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirtualBubbleStore : MonoBehaviour {

    public enum PaymentType
    {
        Gold,
        Gem
    }

    public string Id;
    public string Name;
    public int WhiteBubble;
    public int RedBubble;
    public int OrangeBubble;
    public PaymentType Payment;
    public int Price;

    public Text textName;
    public Text textPrice;

    public ConfirmationPopupController confirmationPopup;
    public MessagePopupController messagePopup;
    public ReloadPopupController reloadPopup;
	public PrePlayPopupManager preplayPopup;
	// Use this for initialization
	void Start () {
        this.textName.text = Name;
        this.textPrice.text = Price.ToString("#,0");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        confirmationPopup.Show("Do you want to buy " + Name + "?", YesToBuy, NoToBuy);
    }

    private void YesToBuy()
    {
        UserStockData userStock = UserStockData.Load();

        if (Payment == PaymentType.Gold)
        {
            if (userStock.PlusMinGold(-Price))
            {
                if (userStock.PlusMinBubble(WhiteBubble, RedBubble, OrangeBubble))
                {
                    messagePopup.Show("You have bought " + Name);
                    string command = "{";
                    command += "action:BUY_BUBBLE";
                    command += ",item:" + Name;
                    command += "}";
                    ServerStatistic.DoRequest(command);
                    if (reloadPopup)
                    {
                        reloadPopup.NewBuy(WhiteBubble, RedBubble, OrangeBubble);
                    }
                }
                else
                {
                    userStock.PlusMinGold(Price);
                }
            }
            else {
                messagePopup.Show("You need " + (Price - userStock.Gold) + " more coins!");
            }
        }
        else if (Payment == PaymentType.Gem)
        {
            if(userStock.PlusMinGem(-Price))
            {
                if (userStock.PlusMinBubble(WhiteBubble, RedBubble, OrangeBubble))
                {
                    messagePopup.Show("You have bought " + Name);
                    string command = "{";
                    command += "action:BUY_BUBBLE";
                    command += ",item:" + Name;
                    command += "}";
                    ServerStatistic.DoRequest(command);
                    if (reloadPopup)
                    {
                        reloadPopup.NewBuy(WhiteBubble, RedBubble, OrangeBubble);
                    }
                }
                else
                {
                    userStock.PlusMinGem(Price);
                }
            }
            else
            {
                messagePopup.Show("You need " + (Price - userStock.Gems) + " more pearls!");
            }
        }
    }

    private void NoToBuy()
    {

    }
}
