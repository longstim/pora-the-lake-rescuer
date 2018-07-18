using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using OnePF;
using Facebook.MiniJSON;

public class ItemStore : MonoBehaviour {

#pragma warning disable 0414
	[System.Serializable]
	public class ItemPurchase   //item purchasing IDs
	{
		public string IosStore;
		public string AndroidStore;
		public string WpStore;
	}

    public string ItemId;
    public string Name;
    public int Gems;
    public ItemPurchase ItemPurchaseStore;
    public Text priceText;
    public Image IconItem; 

    public MessagePopupController MessagePopup;
    public ConfirmationBuyingPopup ConfirmationPopup;

	void OnDisable() {
		OpenIABEventManager.queryInventorySucceededEvent -= OnQueryInventorySucceeded;
		OpenIABEventManager.queryInventoryFailedEvent -= OnQueryInventoryFailed;
		OpenIABEventManager.purchaseSucceededEvent -= OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent -= OnPurchaseFailed;
	}

    void OnEnable()
    {
        OpenIABEventManager.queryInventorySucceededEvent += OnQueryInventorySucceeded;
        OpenIABEventManager.queryInventoryFailedEvent += OnQueryInventoryFailed;
        OpenIABEventManager.purchaseSucceededEvent += OnPurchaseSucceded;
        OpenIABEventManager.purchaseFailedEvent += OnPurchaseFailed;
    }
	
	private void Awake()
	{
        
#if UNITY_ANDROID
        ConfigurationIAP.AllSKU.Add(ItemPurchaseStore.AndroidStore);
#elif UNITY_IOS
		ConfigurationIAP.AllSKU.Add(ItemPurchaseStore.IosStore);
#elif UNITY_WP8
	    ConfigurationIAP.AllSKU.Add(ItemPurchaseStore.WpStore);
#endif
    }
	
	// Use this for initialization
	private void Start () {
#if UNITY_ANDROID
        OpenIAB.mapSku(ItemPurchaseStore.AndroidStore, OpenIAB_Android.STORE_GOOGLE, ItemPurchaseStore.AndroidStore);
#elif UNITY_IOS
        OpenIAB.mapSku(ItemPurchaseStore.IosStore.ToString(), OpenIAB_iOS.STORE, ItemPurchaseStore.IosStore.ToString());
#elif UNITY_WP8
        OpenIAB.mapSku(ItemPurchaseStore.WpStore.ToString(), OpenIAB_WP8.STORE, ItemPurchaseStore.WpStore.ToString());
#endif
    }

	private void OnQueryInventorySucceeded(Inventory inventory)
	{
        SkuDetails skusDetail = inventory.GetSkuDetails(ItemPurchaseStore.AndroidStore);
        priceText.text = skusDetail.CurrencyCode + " " + skusDetail.PriceValue;

#if UNITY_ANDROID
        if (inventory.HasPurchase(ItemPurchaseStore.AndroidStore))
        {
            UserStockData userStock = UserStockData.Load();
            userStock.PlusMinGem(Gems);
            OpenIAB.consumeProduct(inventory.GetPurchase(ItemPurchaseStore.AndroidStore));
            MessagePopup.Show("You have bought " + Name);

            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:" + Name;
            command += "}";
            ServerStatistic.DoRequest(command);
        }
#elif UNITY_IOS
        if (inventory.HasPurchase(ItemPurchaseStore.IosStore))
        {
            UserStockData userStock = UserStockData.Load();
            userStock.PlusMinGem(Gems);
            OpenIAB.consumeProduct(inventory.GetPurchase(ItemPurchaseStore.IosStore));
            MessagePopup.Show("You have bought " + Name);

            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:" + Name;
            command += "}";
            ServerStatistic.DoRequest(command);
        }
#elif UNITY_WP8
        if (inventory.HasPurchase(ItemPurchaseStore.WpStore))
        {
            UserStockData userStock = UserStockData.Load();
            userStock.PlusMinGem(Gems);
            OpenIAB.consumeProduct(inventory.GetPurchase(ItemPurchaseStore.WpStore));
            MessagePopup.Show("You have bought " + Name);

            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:" + Name;
            command += "}";
            ServerStatistic.DoRequest(command);
        }
#endif
	}

    private void OnQueryInventoryFailed(string error)
    {
        Debug.Log("Query inventory failed: " + error);
    }

	
	public void Buy()
	{
#if UNITY_ANDROID
        OpenIAB.purchaseProduct (ItemPurchaseStore.AndroidStore);
#elif UNITY_IOS
        OpenIAB.purchaseProduct (ItemPurchaseStore.IosStore);
#elif UNITY_WP8
        OpenIAB.purchaseProduct (ItemPurchaseStore.WpStore);
#endif
	}

    public void onClick()
    {
        ConfirmationPopup.Show(IconItem.sprite,Gems+" Pearl",priceText.text, YesBuy, No);
    }

    public void YesBuy()
    {
        Buy();
    }

    public void No()
    {

    }


    private void OnPurchaseSucceded(Purchase purchase) //to VerifyDeveloperPayload to our own server
    {
#if UNITY_ANDROID
        if (purchase.Sku == ItemPurchaseStore.AndroidStore)
        {
            UserStockData userStock = UserStockData.Load();
            userStock.PlusMinGem(Gems);
            OpenIAB.consumeProduct(purchase);
            MessagePopup.Show("You have bought " + Name);

            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:" + Name;
            command += "}";
            ServerStatistic.DoRequest(command);
        }
#elif UNITY_IOS
        if (purchase.Sku == ItemPurchaseStore.IosStore)
        {
            UserStockData userStock = UserStockData.Load();
            userStock.PlusMinGem(Gems);
            OpenIAB.consumeProduct(purchase);
            MessagePopup.Show("You have bought " + Name);

            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:" + Name;
            command += "}";
            ServerStatistic.DoRequest(command);
        }
#elif UNITY_WP8
        if (purchase.Sku == ItemPurchaseStore.WpStore)
        {
            UserStockData userStock = UserStockData.Load();
            userStock.PlusMinGem(Gems);
            OpenIAB.consumeProduct(purchase);
            MessagePopup.Show("You have bought " + Name);

            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:" + Name;
            command += "}";
            ServerStatistic.DoRequest(command);
        }
#endif
    }


    private void OnPurchaseFailed(int errorCode, string error)
    {
        //MessagePopup.Show("Failed to buy item!");
		MessagePopup.Show("You haven't bought " + Name);
    }

}
