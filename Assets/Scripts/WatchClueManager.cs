using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public class WatchClueManager : MonoBehaviour {

    string clueLevel;
	public GameObject PoraPosisi;

	[System.Serializable]
	public class RubbishPackPlay
	{
		public string PackageName;
		public GameObject[] Size1;
		public GameObject[] Size2;
		public GameObject[] Size3;
		public GameObject[] Size4;
		public GameObject[] Size5;
	}
	
	
	[System.Serializable]
	public class BarrierPackPlay
	{
		public string PackageName;
		public GameObject[] Barriers;
	}
	
	[System.Serializable]
	public class BubblePackPlay
	{
		public string PackageName;
		public GameObject WhitePreview;
		public GameObject RedPreview;
		public GameObject OrangePreview;
		
	}
	
	[System.Serializable]
	public class ParentClue
	{
		public GameObject Bubble;
		public GameObject Barrier;
		public GameObject Rubbish;
		public GameObject BubblePreview;
	}

	public GameObject ParentScrenshot;
	public GameObject TemplateScreenshot;
	//public ParentClue ParentObject;
	public BubblePackPlay[] Bubbles;
	public RubbishPackPlay[] Rubbishes;
	public BarrierPackPlay[] Barriers;

    public ConfirmationPopupController confirmationPopupController;
    public MessagePopupController messagePopupController;
    public WatchCluePopUp watchCluePopUp;

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show(string globalId)
	{
        clueLevel = globalId;
        WatchClueConfirmation();
	}

	public void WatchClue() {
        GetComponent<Animator>().SetTrigger("Show");
		string url = "http://nedstudio.net/ned/api/web/v1/clues/watch-clue";
		WWWForm form = new WWWForm();
		form.AddField("facebook_id", FB.UserId);
		form.AddField("app_token", UserDataManager.APP_TOKEN);
		form.AddField("level_id",clueLevel);
		WWW www = new WWW(url, form);

        UserStockData Transaction = UserStockData.Load();
		for (int i =1 ; i<ParentScrenshot.transform.childCount ; i++)
		{
			Destroy(ParentScrenshot.transform.GetChild(i).gameObject);
		}
        if (Transaction.Gold >= 500)
        {
            StartCoroutine(WaitForRequest(www));
        }
        else {
            messagePopupController.Show("You need " + (500 - Transaction.Gold) + " more coins!");
        }

	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		//string data = www.text;
		var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
		Debug.Log(www.text);
		if (www.error == null) {
			UserStockData Transaction = UserStockData.Load();
            if (Transaction.PlusMinGold(-500))
            {
                var data = dict["data"] as List<object>;

                Dictionary<string, object> clue_data = data[0] as Dictionary<string, object>;

                string[] separator = new string[1];
                separator[0] = "#";

                string[] sc = (clue_data["clue_data"]).ToString().Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                Debug.Log(sc.Length);

                List<ScreenShot> ListOFClue = new List<ScreenShot>();
                for (int i = 0; i < sc.Length; i++)
                {
                    ListOFClue.Add(ScreenShot.Deserialize(sc[i]));
                }

                RefreshClue(ListOFClue);
            }
            else {
                messagePopupController.Show("You need " + (500 - Transaction.Gold) + " more coins!");
            }
		} 
		else {
            messagePopupController.Show("Failed to connect to server!");
		}
	}

	public void RefreshClue(List<ScreenShot> listOfScreenshot)
	{
		Debug.Log ("tes");
		for(int i=0; i < listOfScreenshot.Count; i++)
		{
			GameObject scObject = (GameObject)Instantiate(TemplateScreenshot);
			scObject.SetActive(true);
			scObject.transform.SetParent(ParentScrenshot.transform);
			scObject.transform.localPosition = new Vector3();
			scObject.transform.localScale = new Vector3(1f, 1f , 1f);
			RefreshRubbish(scObject,listOfScreenshot[i].Rubbish);
			RefreshBarrier(scObject,listOfScreenshot[i].Barries);
			RefreshBubble(scObject,listOfScreenshot[i].Bubbles);
			RefreshPora(scObject,listOfScreenshot[i].Pora);

		}

	}

	private void RefreshRubbish(GameObject scObject, List<ScreenShot.ClueSampah> daftarSampah)
	{
		foreach (ScreenShot.ClueSampah sampah in daftarSampah)
		{
			for (int i = 0; i < Rubbishes.Length; i++)
			{
				if (sampah.Packagename == Rubbishes[i].PackageName)
				{
					GameObject objek;
					switch (sampah.Size) // Nanti minta dari bastian di buat ukuran sampahnya
					{
					case 1:
						objek = (GameObject)Instantiate(Rubbishes[i].Size1[Random.Range(0, Rubbishes[i].Size1.Length - 1)]);
						break;
					case 2:
						objek = (GameObject)Instantiate(Rubbishes[i].Size2[Random.Range(0, Rubbishes[i].Size2.Length - 1)]);
						break;
					case 3:
						objek = (GameObject)Instantiate(Rubbishes[i].Size3[Random.Range(0, Rubbishes[i].Size3.Length - 1)]);
						break;
					case 4:
						objek = (GameObject)Instantiate(Rubbishes[i].Size4[Random.Range(0, Rubbishes[i].Size4.Length - 1)]);
						break;
					default:
						objek = (GameObject)Instantiate(Rubbishes[i].Size5[Random.Range(0, Rubbishes[i].Size5.Length - 1)]);
						break;
					}
					objek.GetComponent<RubbishController>().enabled= false;
					objek.GetComponent<Animator>().enabled = false;
					objek.transform.SetParent(scObject.transform);
					objek.transform.localScale = new Vector3(1, 1, 1);
					objek.GetComponent<RectTransform>().localPosition = sampah.Posisi;
					Destroy(objek.transform.GetChild(0).gameObject);
				}
			}
		}
	}
	
	private void RefreshBarrier(GameObject scObject, List<ScreenShot.CluePenghalang> daftarPenghalang)
	{
		foreach (ScreenShot.CluePenghalang penghalang in daftarPenghalang)
		{
			for (int i = 0; i < Barriers.Length; i++)
			{
				if (penghalang.Packagename == Barriers[i].PackageName)
				{
					for (int j = 0; j < Barriers[i].Barriers.Length; j++)
					{
						if (penghalang.Nama == Barriers[i].Barriers[j].GetComponent<BarrierController>().Name)
						{
							GameObject objek = (GameObject)Instantiate(Barriers[i].Barriers[j]);
							BarrierController barrier = objek.GetComponent<BarrierController>();
							barrier.isPreview = true;
							barrier.Rotation = penghalang.Rotasi;
							objek.transform.position = penghalang.Posisi;
							Debug.Log(penghalang.Posisi);
							objek.transform.SetParent(scObject.transform);
							objek.transform.localScale = new Vector3(1, 1, 1);
							objek.GetComponent<RectTransform>().localPosition = penghalang.Posisi;
						}
					}
				}
			}
		}
	}
	
	private void RefreshBubble(GameObject scObject, List<ScreenShot.ClueGelembung> daftarGelembung)
	{
		foreach (ScreenShot.ClueGelembung gelembung in daftarGelembung)
		{
			Debug.Log("test 1");
			for (int i = 0; i < Bubbles.Length; i++)
			{	Debug.Log("test 2");
				Debug.Log(gelembung.PackageName);
					Debug.Log("test 3");
					if (gelembung.Type == BubbleController.BubbleType.White)
					{
						Debug.Log("test 4");
						GameObject objek = (GameObject)Instantiate(Bubbles[i].WhitePreview);
						objek.transform.SetParent(scObject.transform);
						objek.transform.localScale = new Vector3(1f, 1, 1);
						objek.GetComponent<RectTransform>().localPosition = gelembung.Posisi;
                        switch (gelembung.Size)
                        {
                            case 1:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                                break;
                            case 2:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f);
                                break;
                            case 3:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.4f, 1.4f, 1f);
                                break;
                            case 4:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.6f, 1.6f, 1f);
                                break;
                            default:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.8f, 1.8f, 1f);
                                break;

                        }
						Debug.Log(gelembung.Posisi);
					}
					else if (gelembung.Type == BubbleController.BubbleType.Red)
					{
						GameObject objek = (GameObject)Instantiate(Bubbles[i].RedPreview);
						objek.transform.SetParent(scObject.transform);
						objek.transform.localScale = new Vector3(1f, 1, 1);
						objek.GetComponent<RectTransform>().localPosition = gelembung.Posisi;
                        switch (gelembung.Size)
                        {
                            case 1:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                                break;
                            case 2:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f);
                                break;
                            case 3:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.4f, 1.4f, 1f);
                                break;
                            case 4:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.6f, 1.6f, 1f);
                                break;
                            default:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.8f, 1.8f, 1f);
                                break;

                        }
						Debug.Log(gelembung.Posisi);
					}
					else if (gelembung.Type == BubbleController.BubbleType.Orange)
					{
						GameObject objek = (GameObject)Instantiate(Bubbles[i].OrangePreview);
						objek.transform.SetParent(scObject.transform);
						objek.transform.localScale = new Vector3(1f, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = gelembung.Posisi;
                        switch (gelembung.Size)
                        {
                            case 1:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                                break;
                            case 2:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f);
                                break;
                            case 3:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.4f, 1.4f, 1f);
                                break;
                            case 4:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.6f, 1.6f, 1f);
                                break;
                            default:
                                objek.GetComponent<RectTransform>().localScale = new Vector3(1.8f, 1.8f, 1f);
                                break;

                        }
						Debug.Log(gelembung.Posisi);
					}
			}
		}
	}

	private void RefreshPora(GameObject scObject, ScreenShot.CluePora Pora){
		GameObject PoraScreenShot = new GameObject ();
		PoraScreenShot = (GameObject)Instantiate (PoraPosisi);
		PoraScreenShot.transform.SetParent (scObject.transform);
		PoraScreenShot.transform.localScale = new Vector3 (1f, 1f, 1f);
		PoraScreenShot.transform.localPosition = Pora.Posisi;
		PoraScreenShot.GetComponentInChildren<Animator> ().enabled = false;
		PoraScreenShot.transform.GetChild (0).gameObject.SetActive (false);
		PoraScreenShot.transform.GetChild (1).gameObject.SetActive (false);
		PoraScreenShot.transform.GetChild (3).gameObject.SetActive (false);
		Debug.Log ("Pora Position :" + Pora.Posisi);
	}

	public void Next(){
		ParentScrenshot.GetComponent<RectTransform> ().localPosition += new Vector3 (-1362, 0,0);
	}

	public void Before(){
		ParentScrenshot.GetComponent<RectTransform> ().localPosition += new Vector3 (1362, 0,0);
	}

    public void WatchClueConfirmation()
    {
        watchCluePopUp.Show(WatchClue, NoWatch);
    }

    public void NoWatch()
    {

    }

}
