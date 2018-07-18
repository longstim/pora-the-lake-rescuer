using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OnePF;

public class ConfigurationIAP : MonoBehaviour {
    [HideInInspector]
    public string GOOGLE_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkELY20huD9Bt0c5AT8Kep9pV7gpVjU7l/cjX0ZpjJq3qYgZhYowbj/aG2YS4Z7bQFb/LfwKcCI/kdd7XNrJfQoNMYkEUTK/leartms1TvTcs2IPrV/x7VlL+UsRpjG0zn70fysJi8HSz5qM9bF13gWHO5YBITtiMbHwpzT/Y37LN5jjm0NlW94AjmkuVh0dgefaOL/cp9yn8F1C+04hgLLZv2vshTCXdi2mzzcffayB4cTycDmVfBCvXxUNanZOVUy42uP1J1e1AD8xlGmuktHX9B2+R9duJPi4RJJ2mvvz5YjPdNITLM1VfAV5ZX1NHHkgXbpUbg52a7OSGBWWy+QIDAQAB";

    public static List<string> AllSKU = new List<string>();

    public GameObject openIABObject;
	void OnDisable()
	{
		OpenIABEventManager.billingSupportedEvent += OnBillingSupported;
		OpenIABEventManager.billingNotSupportedEvent += OnBillingNotSupported;
		OpenIABEventManager.consumePurchaseSucceededEvent += OnConsumePurchaseSucceeded;
		OpenIABEventManager.consumePurchaseFailedEvent += OnConsumePurchaseFailed;
		OpenIABEventManager.transactionRestoredEvent += OnTransactionRestored;
		OpenIABEventManager.restoreSucceededEvent += OnRestoreSucceeded;
		OpenIABEventManager.restoreFailedEvent += OnRestoreFailed;
	}

    void OnEnable()
    {
        OpenIABEventManager.billingSupportedEvent += OnBillingSupported;
        OpenIABEventManager.billingNotSupportedEvent += OnBillingNotSupported;
        OpenIABEventManager.consumePurchaseSucceededEvent += OnConsumePurchaseSucceeded;
        OpenIABEventManager.consumePurchaseFailedEvent += OnConsumePurchaseFailed;
        OpenIABEventManager.transactionRestoredEvent += OnTransactionRestored;
        OpenIABEventManager.restoreSucceededEvent += OnRestoreSucceeded;
        OpenIABEventManager.restoreFailedEvent += OnRestoreFailed;
    }

	// Use this for initialization
	public void Start () {
		
		// Set some library options
		var options = new OnePF.Options();
		
		options.checkInventory = false; //hanya jalan di android saja
		options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
		
		// Add Google Play public key
        options.storeKeys.Add(OpenIAB_Android.STORE_GOOGLE, GOOGLE_KEY);

		OpenIAB.init(options);
		
	}

	void OnDestroy() {
        OpenIAB.unbindService();
		Destroy (openIABObject);
	}


    private void OnBillingSupported()
    {
        Debug.Log("Billing is supported");
        OpenIAB.queryInventory(AllSKU.ToArray());
    }

    private void OnBillingNotSupported(string error)
    {
        Debug.Log("Billing not supported: " + error);

    }

    private void OnQueryInventoryFailed(string error)
    {
        Debug.Log("Query inventory failed: " + error);
    }

    private void OnConsumePurchaseSucceeded(Purchase purchase)
    {
        Debug.Log("Consume purchase succeded: " + purchase.ToString());
    }

    private void OnConsumePurchaseFailed(string error)
    {
        Debug.Log("Consume purchase failed: " + error);
    }

    private void OnTransactionRestored(string sku)
    {
        Debug.Log("Transaction restored: " + sku);
    }

    private void OnRestoreSucceeded()
    {
        Debug.Log("Transactions restored successfully");
    }

    private void OnRestoreFailed(string error)
    {
        Debug.Log("Transaction restore failed: " + error);
    }



}
