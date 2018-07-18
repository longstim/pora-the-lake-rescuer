using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class BuyUsingAdsItem : MonoBehaviour {

    public MessagePopupController messagePopUp;
    public int Gems;
	// Use this for initialization
	void Start () {
        if (!Advertisement.isInitialized)
        {
            Advertisement.allowPrecache = true;
            Advertisement.Initialize("27637", false);
        }
        else
        {
            Debug.Log("Platform not supported");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        if (Advertisement.isReady("rewardedVideoZone"))
        {
            Advertisement.Show("rewardedVideoZone", new ShowOptions
            {
                pause = true,
                resultCallback = result =>
                {
                    if (result.ToString() == "Finished")
                    {
                        FinishWatching();
                    }
                    else if (result.ToString() == "Skipped")
                    {
                        messagePopUp.Show("You have skipped the video!");
                        string command = "{";
                        command += "action:BUY_PEARL";
                        command += ",item:1_SKIP_PEARLD_ADS";
                        command += "}";
                        ServerStatistic.DoRequest(command);
                    }
                    else
                    {
                        messagePopUp.Show("Failed to play video!");
                        string command = "{";
                        command += "action:BUY_PEARL";
                        command += ",item:1_FAILED_PEARLD_ADS";
                        command += "}";
                        ServerStatistic.DoRequest(command);
                    }
                }
            });
        }
        else
        {
            messagePopUp.Show("Failed to play video!");
            string command = "{";
            command += "action:BUY_PEARL";
            command += ",item:1_FAILED_PEARLD_ADS";
            command += "}";
            ServerStatistic.DoRequest(command);
        }
    }

    private void FinishWatching()
    {
        UserStockData userStock = UserStockData.Load();
        userStock.PlusMinGem(Gems);
        messagePopUp.Show("You have got 1 Pearl!");

        string command = "{";
        command += "action:BUY_PEARL";
        command += ",item:1_SUCCESS_PEARLD_ADS";
        command += "}";
        ServerStatistic.DoRequest(command);
    }
}
