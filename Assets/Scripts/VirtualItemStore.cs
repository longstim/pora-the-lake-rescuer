using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirtualItemStore : MonoBehaviour {

    public enum PaymentType
    {
        Gold,
        Gem
    }

    public string Id;
    public string Name;
    public string[] ItemAlias;
    public PaymentType Payment;
    public int Price;

    public Image IconItem;
    public Text textName;
    public Text textPrice;

    public GameObject EffectedItem;
    public GameObject newLable;

    public ConfirmationBuyingPopup confirmationPopup;
    public MessagePopupController messagePopup;
	// Use this for initialization
	void Start () {
        this.textName.text = Name;
        this.textPrice.text = Price.ToString("#,0");

        if (UserItemStock.Items.Contains(ItemAlias[0]))
        {
            textPrice.text = "Bought";
        }
        
        if (!PlayerPrefs.HasKey("newPaskah") && newLable)
        {
            newLable.SetActive(true);
            PlayerPrefs.SetString("newPaskah", "OK");
            PlayerPrefs.Save();
        }
        else if (newLable)
        {
            newLable.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
	}

    public void OnClick()
    {
        UserItemStock userItems = new UserItemStock();
        userItems.Load();

        if (userItems.HaveBuy(ItemAlias[0]))
        {
            messagePopup.Show("You have bought this Item!");
        }
        else
        {
            if (Payment == PaymentType.Gem)
            {
                confirmationPopup.Show(IconItem.sprite, Name, textPrice.text+" Pearls", YesToBuy, NoToBuy);
            }
            else if (Payment == PaymentType.Gold)
            {
                confirmationPopup.Show(IconItem.sprite, Name, textPrice.text + " Coins", YesToBuy, NoToBuy);
            }
        }
    }

    private void YesToBuy()
    {
        UserStockData userStock = UserStockData.Load();

        UserItemStock userItems = new UserItemStock();
        userItems.Load();

        if (Payment == PaymentType.Gem)
        {
            if (userStock.PlusMinGem(-Price))
            {
                for (int i = 0; i < ItemAlias.Length; i++)
                {
                    userItems.Add(ItemAlias);
                }
				messagePopup.Show("You have bought " + Name);

                string command = "{";
                command += "action:BUY_ITEM";
                command += ",item:" + Name;
                command += "}";
                ServerStatistic.DoRequest(command);

				if (UserItemStock.Items.Contains(ItemAlias[0]))
				{
					textPrice.text = "Bought";
					EffectedItem.SetActive(true);
				}
			}else{
				messagePopup.Show("You need "+(Price - userStock.Gems)+" more pearls!");
            }
        }else if (Payment == PaymentType.Gold)
        {
            if (userStock.PlusMinGold(-Price))
            {
                for (int i = 0; i < ItemAlias.Length; i++)
                {
                    userItems.Add(ItemAlias[i]);
                }
				messagePopup.Show("You have bought " + Name);

                string command = "{";
                command += "action:BUY_ITEM";
                command += ",item:" + Name;
                command += "}";
                ServerStatistic.DoRequest(command);

                if (UserItemStock.Items.Contains(ItemAlias[0]))
                {
                    textPrice.text = "Bought";
                    EffectedItem.SetActive(true);
                }
				//gameObject.SetActive(!UserItemStock.Items.Contains(ItemAlias[0]));
            }else{
                messagePopup.Show("You need "+(Price - userStock.Gold)+" more coins!");
            }
        }
    }

    private void NoToBuy()
    {

    }
}
