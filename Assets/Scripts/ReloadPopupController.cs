using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using UnityEngine.Advertisements;

public class ReloadPopupController : MonoBehaviour {

	public delegate void CallbackYes(List<Gelembung> daftarGelembung);
	public delegate void CallbackNo();

	public enum ButtonName
	{
		No,
		FinishIt
	}

	public Button buttonYes;
	public GameObject PanelPreview;

	private bool isShowing;
	public bool IsShowing {
		get {
			return isShowing;
		}
	}

	CallbackYes callbackYes;
	CallbackNo callbackNo;

    int MaksBubble;

    public Text TextWhiteBubble;
    public Text TextRedBubble;
    public Text TextOrangeBubble;

    public Animator tutorialAnimator;
    public Animator tutorialPora;
    public GameObject closeForDefine;

    public GameObject WhiteBubbleButton;
    public GameObject RedBubbleButton;
    public GameObject OrangeBubbleButton;
    public Button ReloadButton;
    public GameObject UndefineBubble;

    public GameObject BtnShoot;

    public int whiteBubbleStock;
    public int redBubbleStock;
    public int orangeBubbleStock;

    public int WhiteBubbleStock
    {
        get { return whiteBubbleStock; }
        set
        {
            whiteBubbleStock = value;
            AfterChangeQuantity();
        }
    }

    public int RedBubbleStock
    {
        get { return redBubbleStock; }
        set
        {
            redBubbleStock = value;
            AfterChangeQuantity();
        }
    }

    public int OrangeBubbleStock
    {
        get { return orangeBubbleStock; }
        set
        {
            orangeBubbleStock = value;
            AfterChangeQuantity();
        }
    }

    void AfterChangeQuantity()
    {
        TextWhiteBubble.text = WhiteBubbleStock.ToString();
        TextRedBubble.text = RedBubbleStock.ToString();
        TextOrangeBubble.text = OrangeBubbleStock.ToString();

        WhiteBubbleButton.SetActive(!(WhiteBubbleStock == 0));
        RedBubbleButton.SetActive(!(RedBubbleStock == 0));
        OrangeBubbleButton.SetActive(!(OrangeBubbleStock == 0));

        ReloadButton.interactable = PanelPreview.transform.childCount > 0;
    }

    public GameObject ConfirmationRoload;
    public GameObject ReloadView;
    public MessagePopupController messagePopup;
    public GameObject ReloadPopUp;

    StaticLevel dataStaticLevel;
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

    public void DeleteCallback(ItemGelembungController itemGel)
    {
        if (itemGel.Type == BubbleType.White)
        {
            WhiteBubbleStock++;
            TextWhiteBubble.text = WhiteBubbleStock.ToString();
        }
        else if (itemGel.Type == BubbleType.Red)
        {
            RedBubbleStock++;
            TextRedBubble.text = WhiteBubbleStock.ToString();
        }
        else if (itemGel.Type == BubbleType.Orange)
        {
            OrangeBubbleStock++;
            TextOrangeBubble.text = WhiteBubbleStock.ToString();
        }

        ReloadButton.interactable = PanelPreview.transform.childCount > 0;

        WhiteBubbleButton.SetActive(!(WhiteBubbleStock == 0));
        RedBubbleButton.SetActive(!(WhiteBubbleStock == 0));
        OrangeBubbleButton.SetActive(!(WhiteBubbleStock == 0));
    }

	// Update is called once per frame
	void Update () {
        
	}

	public void Show(StaticLevel dataStaticLevel, CallbackYes callbackYes,CallbackNo callbackNo){
        UserStockData.Load();

        this.dataStaticLevel = dataStaticLevel;

        WhiteBubbleStock = UserStockData.WhiteBubbleStock;
        RedBubbleStock = UserStockData.RedBubbleStock;
        OrangeBubbleStock = UserStockData.OrangeBubbleStock;

        TextWhiteBubble.text = WhiteBubbleStock.ToString();
        TextRedBubble.text = RedBubbleStock.ToString();
        TextOrangeBubble.text = OrangeBubbleStock.ToString();

		this.callbackYes = callbackYes;
		this.callbackNo = callbackNo;
        GetComponent<Animator>().SetTrigger("Show");
        isShowing = true;

        MaksBubble = dataStaticLevel.Bubbles.Count;
        for (int i = 0; i < MaksBubble; i++)
        {
            GameObject objek = (GameObject)Instantiate(UndefineBubble);
            objek.transform.SetParent(PanelPreview.transform);
            objek.transform.localScale = new Vector3(1, 1, 1);
            objek.transform.localPosition = new Vector3(objek.transform.localPosition.x, objek.transform.localPosition.y, 0);
            objek.GetComponent<DropBubbleHandler>().reloadPopupController = this;
            objek.GetComponent<ItemGelembungController>().Define = false;
        }

        WhiteBubbleButton.SetActive(!(WhiteBubbleStock == 0));
        RedBubbleButton.SetActive(!(RedBubbleStock == 0));
        OrangeBubbleButton.SetActive(!(OrangeBubbleStock == 0));
	}

    private void Hide()
    {
        GetComponent<Animator>().SetTrigger("Hide");
		ReloadView.SetActive(false);
		ReloadPopUp.SetActive(true);
        isShowing = false;
    }

	public void Yes()
	{
        BtnShoot.SetActive(true);
		List<Gelembung> daftarGelembung = new List<Gelembung> ();
		for (int i=0; i < PanelPreview.transform.childCount; i++) {
			ItemGelembungController controller = PanelPreview.transform.GetChild(i).gameObject.GetComponent<ItemGelembungController>();
            if (controller.Define)
            {
                daftarGelembung.Add(new Gelembung(controller.PackageName, controller.Type));
            }
		}

        UseBubble(daftarGelembung);
        callbackYes(daftarGelembung);
        Hide();
	}

    private void UseBubble(List<Gelembung> daftarGelembung)
    {
        int WhiteBubble = 0;
        int RedBubble = 0;
        int OrangeBubble = 0;

        foreach (Gelembung gel in daftarGelembung)
        {
            if (gel.Type == BubbleType.White)
            {
                WhiteBubble++;
            }
            else if (gel.Type == BubbleType.Red)
            {
                RedBubble++;
            }
            else if (gel.Type == BubbleType.Orange)
            {
                OrangeBubble++;
            }
        }
        UserStockData data = UserStockData.Load();
        data.PlusMinBubble(-WhiteBubble, -RedBubble, -OrangeBubble);

        //Statistik
        string command = "{";
        command += "action:PlAY_LEVEL";
        command += ",place:"+dataStaticLevel.Place;
        command += ",level:"+dataStaticLevel.Level;
        command += ",status:3_RELOAD";
        command += ",WhiteBubble:"+WhiteBubble;
        command += ",RedBubble:"+RedBubble;
        command += ",OrangeBubble:"+OrangeBubble;
        command += "}";
        ServerStatistic.DoRequest(command);
         
         
    }

	public void No()
	{
        //Statistik
        string command = "{";
        command += "action:PlAY_LEVEL";
        command += ",place:"+dataStaticLevel.Place;
        command += ",level:"+dataStaticLevel.Level;
        command += ",status:1_COMFIRM_RELOAD_NO";
        command += "}";
        ServerStatistic.DoRequest(command);
        
		callbackNo ();
        Hide();
	}

    public void ReloadUsingGold()
    {
        UserStockData data = UserStockData.Load();
        if (!data.PlusMinGold(-40))
        {
            messagePopup.Show("You dont have enough coin, let buy more!");
        }
        else {
            ReloadView.SetActive(true);
            ReloadPopUp.SetActive(false);
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:2_COMFIRM_RELOAD_USING_COINS";
            command += "}";
            ServerStatistic.DoRequest(command);
            
        }

        if (!PlayerPrefs.HasKey("hadwatch"))
        {
            tutorialPora.SetTrigger("define");
            closeForDefine.SetActive(true);
            tutorialAnimator.SetTrigger("Show");
            PlayerPrefs.SetString("hadwatch","");
            PlayerPrefs.Save();
        }
    }

    public void ReloadUsingWatchAds()
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
                        CallbackReloadUsingAds();
                    }
                    else if (result.ToString() == "Skipped")
                    {
                        messagePopup.Show("You have skiped the ads!");
                        //Statistik
                        string command = "{";
                        command += "action:PlAY_LEVEL";
                        command += ",place:"+dataStaticLevel.Place;
                        command += ",level:"+dataStaticLevel.Level;
                        command += ",status:2_SKIP_COMFIRM_RELOAD_USING_ADS";
                        command += "}";
                        ServerStatistic.DoRequest(command);
                         
                    }
                    else
                    {
                        messagePopup.Show("Video is not available");
                        //Statistik
                        string command = "{";
                        command += "action:PlAY_LEVEL";
                        command += ",place:"+dataStaticLevel.Place;
                        command += ",level:"+dataStaticLevel.Level;
                        command += ",status:2_FAILED_COMFIRM_RELOAD_USING_ADS";
                        command += "}";
                        ServerStatistic.DoRequest(command);
                         
                    }
                }
            });
        }
        else {
			messagePopup.Show("Video is not available");

            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:2_FAILED_COMFIRM_RELOAD_USING_ADS";
            command += "}";
            ServerStatistic.DoRequest(command);
            
        }
        
    }

    public void CallbackReloadUsingAds()
    {
        ConfirmationRoload.SetActive(true);
        ReloadView.SetActive(true);
        ReloadPopUp.SetActive(false);

        //Statistik
        string command = "{";
        command += "action:PlAY_LEVEL";
        command += ",place:"+dataStaticLevel.Place;
        command += ",level:"+dataStaticLevel.Level;
        command += ",status:2_SUCCESS_COMFIRM_RELOAD_USING_COINS";
        command += "}";
        ServerStatistic.DoRequest(command);

        if (!PlayerPrefs.HasKey("hadwatch"))
        {
            tutorialPora.SetTrigger("define");
            closeForDefine.SetActive(true);
            tutorialAnimator.SetTrigger("Show");
            PlayerPrefs.SetString("hadwatch", "");
            PlayerPrefs.Save();
        }
         
    }

    public void NewBuy(int BubbleWhite, int RedBubble, int OrangeBubble)
    {
        WhiteBubbleStock += BubbleWhite;
        RedBubbleStock += RedBubble;
        OrangeBubbleStock += OrangeBubble;

        TextWhiteBubble.text = WhiteBubbleStock.ToString();
        TextRedBubble.text = RedBubbleStock.ToString();
        TextOrangeBubble.text = OrangeBubbleStock.ToString();

        WhiteBubbleButton.SetActive(!(WhiteBubbleStock == 0));
        RedBubbleButton.SetActive(!(RedBubbleStock == 0));
        OrangeBubbleButton.SetActive(!(OrangeBubbleStock == 0));
    }
}
