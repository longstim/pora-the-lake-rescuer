using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PrePlayPopupManager : MonoBehaviour
{

    Animator animator;
    StaticLevel dataLevel;
    LeaderboardManager leaderboard;

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
    public class ParentGroup
    {
        public GameObject Bubble;
        public GameObject Barrier;
        public GameObject Rubbish;
        public GameObject BubblePreview;
    }

    [System.Serializable]
    public class PlaceItem
    {
        public string Name = "";
        public GameObject[] item;
    }

    [System.Serializable]
    public class BubblePackPlay
    {
        public string PackageName;
        public GameObject WhitePreview;
        public GameObject White;
        public GameObject RedPreview;
        public GameObject Red;
        public GameObject OrangePreview;
        public GameObject Orange;
    }


    public ParentGroup ParentObject;
    //public PlaceItem[] Places;
    public GameObject BubbleUndefine;
    public BubblePackPlay[] Bubbles;
    public RubbishPackPlay[] Rubbishes;
    public BarrierPackPlay[] Barriers;

    public Text LevelName;
    public Text WhiteBubbleQuantity;
    public Text RedBubbleQuantity;
    public Text OrangeBubbleQuantity;

    public Text MyPublicHightScore;
    public Text MyFriendHightScore;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        leaderboard = GetComponent<LeaderboardManager>();
    }

    // Update is called once per frame
    void Update()
    {
		if (asynOperation != null) {
			if (asynOperation.progress >= 0.9f && transisionAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Loading")) {
				Invoke ("ActivateScene", 1f);
				
			}
		}

        WhiteBubbleQuantity.text = UserStockData.WhiteBubbleStock.ToString();
        RedBubbleQuantity.text = UserStockData.RedBubbleStock.ToString();
        OrangeBubbleQuantity.text = UserStockData.OrangeBubbleStock.ToString();
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Show(string season, string place, int level)
    {
        UserStockData.Load();
        this.dataLevel = new StaticLevel(season,place,level);
        
        leaderboard.Show(dataLevel);

        RefreshLevel(dataLevel);
        animator.SetTrigger("Show");
        //GetComponent<LeaderboardManager>().Show((StaticLevel)dataLevel);
        MyPublicHightScore.text = dataLevel.HighScore.ToString("#,0");
        MyFriendHightScore.text = dataLevel.HighScore.ToString("#,0");

        LevelName.text = "Level " + dataLevel.Level;

    }


    public void Show(StaticLevel dataLevel)
    {
        leaderboard.Show(dataLevel);

        UserStockData.Load();
        this.dataLevel = dataLevel;
        RefreshLevel(dataLevel);
        animator.SetTrigger("Show");
        //GetComponent<LeaderboardManager>().Show(dataLevel);
        MyPublicHightScore.text = dataLevel.HighScore.ToString("#,0");
        MyFriendHightScore.text = dataLevel.HighScore.ToString("#,0");

        LevelName.text = "Level " + dataLevel.Level;
    }

    public void Show(string globaLevelId)
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
        leaderboard.Hide();
    }

    private void RefreshLevel(Level dataLevel)
    {
        RemoveAll();
        //RefreshBackground(dataLevel.Place);
        RefreshRubbish(dataLevel.Rubbishes);
        RefreshBubble(dataLevel.Bubbles);
        RefreshBarrier(dataLevel.Barriers);
    }

    public void RemoveAll()
    {
        for (int i = 0; i < ParentObject.Rubbish.transform.childCount; i++)
        {
            Destroy(ParentObject.Rubbish.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ParentObject.Barrier.transform.childCount; i++)
        {
            Destroy(ParentObject.Barrier.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ParentObject.Bubble.transform.childCount; i++)
        {
            Destroy(ParentObject.Bubble.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < ParentObject.BubblePreview.transform.childCount; i++)
        {
            Destroy(ParentObject.BubblePreview.transform.GetChild(i).gameObject);
        }
    }

    /*
    private void RefreshBackground(string Place)
    {
        for (int i = 0; i < Places.Length; i++)
        {
            if (Places[i].Name == Place)
            {
                for (int j = 0; j < Places[i].item.Length; j++)
                {
                    Places[i].item[j].SetActive(true);
                }
            }
        }
    }
     */
    private void RefreshRubbish(List<Sampah> daftarSampah)
    {
        foreach (Sampah sampah in daftarSampah)
        {
            for (int i = 0; i < Rubbishes.Length; i++)
            {
                if (sampah.PackageName == Rubbishes[i].PackageName)
                {
                    GameObject objek;
                    switch (sampah.Size)
                    {
                        case RubbishController.RubbishSize.One:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size1[Random.Range(0, Rubbishes[i].Size1.Length - 1)]);
                            break;
                        case RubbishController.RubbishSize.Two:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size2[Random.Range(0, Rubbishes[i].Size2.Length - 1)]);
                            break;
                        case RubbishController.RubbishSize.Three:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size3[Random.Range(0, Rubbishes[i].Size3.Length - 1)]);
                            break;
                        case RubbishController.RubbishSize.Four:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size4[Random.Range(0, Rubbishes[i].Size4.Length - 1)]);
                            break;
                        default:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size5[Random.Range(0, Rubbishes[i].Size5.Length - 1)]);
                            break;
                    }
                    objek.GetComponent<RubbishController>().Rotation = sampah.Rotation;
                    objek.GetComponent<RubbishController>().RotationSpeed = sampah.RotationSpeed;
                    objek.transform.SetParent(ParentObject.Rubbish.transform);
                    objek.transform.localScale = new Vector3(1, 1, 1);
                    objek.GetComponent<RectTransform>().localPosition = sampah.Position;
                }
            }
        }
    }

    private void RefreshBarrier(List<Penghalang> daftarPenghalang)
    {
        foreach (Penghalang penghalang in daftarPenghalang)
        {
            for (int i = 0; i < Barriers.Length; i++)
            {
                if (penghalang.PackageName == Barriers[i].PackageName)
                {
                    for (int j = 0; j < Barriers[i].Barriers.Length; j++)
                    {
                        if (penghalang.Name == Barriers[i].Barriers[j].GetComponent<BarrierController>().Name)
                        {
                            GameObject objek = (GameObject)Instantiate(Barriers[i].Barriers[j]);
                            BarrierController barrier = objek.GetComponent<BarrierController>();
                            barrier.Rotation = penghalang.Rotation;
                            barrier.RotationSpeed = penghalang.RotationSpeed;
                            barrier.isPreview = true;
                            //objek.transform.position = penghalang.Position;
                            objek.transform.SetParent(ParentObject.Barrier.transform);
                            objek.transform.localScale = new Vector3(1, 1, 1);
                            objek.GetComponent<RectTransform>().localPosition = penghalang.Position;
                        }
                    }
                }
            }
        }
    }

    private void RefreshBubble(List<Gelembung> daftarGelembung)
    {
        foreach (Gelembung gelembung in daftarGelembung)
        {
            for (int i = 0; i < Bubbles.Length; i++)
            {
                if (!gelembung.Define)
                {
                    GameObject objek = (GameObject)Instantiate(BubbleUndefine);
                    objek.transform.SetParent(ParentObject.BubblePreview.transform);
                    objek.transform.localScale = new Vector3(1, 1, 1);
                    Destroy(objek.GetComponent<DropBubbleHandler>());
                    Destroy(objek.GetComponent<EventTrigger>());
                }
                else if (gelembung.PackageName == Bubbles[i].PackageName)
                {
                    if (gelembung.Type == BubbleType.White)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].WhitePreview);
                        objek.transform.SetParent(ParentObject.BubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (gelembung.Type == BubbleType.Red)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].RedPreview);
                        objek.transform.SetParent(ParentObject.BubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (gelembung.Type == BubbleType.Orange)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].OrangePreview);
                        objek.transform.SetParent(ParentObject.BubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }
    }
	AsyncOperation asynOperation = null;
	public Animator transisionAnimator ;


	IEnumerator LoadScene()
	{

        if (dataLevel.Place == "Parbaba")
        {
            asynOperation = Application.LoadLevelAsync(SceneName.PlayParbaba.ToString());   
        }
        else if (dataLevel.Place == "Batuguru")
        {
            asynOperation = Application.LoadLevelAsync(SceneName.PlayBatuguru.ToString());
        }
        else if (dataLevel.Place == "Parapat")
        {
            asynOperation = Application.LoadLevelAsync(SceneName.PlayParapat.ToString());
        }
        else if (dataLevel.Place == "Tomok")
        {
            asynOperation = Application.LoadLevelAsync(SceneName.PlayTomok.ToString());
        }
		asynOperation.allowSceneActivation = false;
		yield return asynOperation;
	}
	public void ActivateScene()
	{
		asynOperation.allowSceneActivation = true;
	}

    public void Play()
    {
		//sceneCommunication sceneCommunication = new SceneCommunication(SceneName.ChooseLevel);
		//sceneCommunication.SendMessage(SceneName.ChoosePlace, dataLevel);
		transisionAnimator.SetTrigger ("Close");
		StartCoroutine ("LoadScene");

        BacksoundController.IsActive = false;
        BacksoundController.DestroyNow = true;
        BacksoundIntroController.DestroyNow = true;
        if (dataLevel.Place == "Parbaba")
        {
            SceneCommunication scom = new SceneCommunication(SceneName.ChooseLevelParbaba);
            scom.SendMessage(SceneName.PlayParbaba, dataLevel);
        }
        else if (dataLevel.Place == "Batuguru")
        {
            SceneCommunication scom = new SceneCommunication(SceneName.ChooseLevelBatuguru);
            scom.SendMessage(SceneName.PlayBatuguru, dataLevel);
        }
        else if (dataLevel.Place == "Parapat")
        {
            SceneCommunication scom = new SceneCommunication(SceneName.ChooseLevelParapat);
            scom.SendMessage(SceneName.PlayParapat, dataLevel);
        }
        else if (dataLevel.Place == "Tomok")
        {
            SceneCommunication scom = new SceneCommunication(SceneName.ChooseLevelTomok);
            scom.SendMessage(SceneName.PlayTomok, dataLevel);
        }
       // Application.LoadLevel(SceneName.Play.ToString());
    }	
}
