using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour {

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

    public enum GameplayState
    {
        Playing,
        Pause,
        Success,
        Failed,
        Reload,
        ChooseBubble,
		Tutorial
    }

    public enum GameplayMode
    {
        StaticLevel,
        Edited,
        ValidateLevel
    }

    public List<ScreenShot> ListOfClue;
    public GameObject BtnSharedClue;

    public SceneName CurrentScene;
    public static GameplayState State;
    public static GameplayMode Mode;

    public ParentGroup ParentObject;
    //public PlaceItem[] Places;
    public BubblePackPlay[] Bubbles;
    public RubbishPackPlay[] Rubbishes;
    public BarrierPackPlay[] Barriers;

    public PoraController poraController;
    public ChooseBubbleController chooseBubbleController;
    public PausePopupController pausePopupController;
    public SuccessPopupController successPopupController;
    public FailPopupController failedPopupController;
    public MessagePopupController messagePopupController;
    public ConfirmationPopupController confirmationPopupController;
    public ReloadPopupController reloadPopupController;
    public GameObject ShootButton;
    public GameObject ShotButtonWhiteBubble;
    public GameObject ShotButtonRedBubble;
    public GameObject ShotButtonOrangeBubble;
	public Animator tutorialAnimator;
	public Animator tutorialPora;
    public Animator ButtonShoot;
    public Animator NormalBarrier;
    public Animator BubbleUndifine;
	public GameObject closeForStart;
	public GameObject closeForDefine;

    public AudioSource[] audioSourcesEffect;
    public AudioSource[] audioSourcesMusic;

    public AudioSource soundSmall;
    public AudioSource soundMiddle;
    public AudioSource soundLarge;
    bool ValidationStatus = false;
    bool SaveStatus = false;
    List<CommunicationData> dataRecieveds;
    Level dataLevel;
    StaticLevel dataStaticLevel;

    public static int Score = 0;
    public static int MultiLiftScore = 0;
    public static int BubbleUsedScore = 0;
    public static int RubbishLiftScore = 0;
    public static int SkillScore = 0;
    public static int ScoreTime = 0;

    int Bintang = 0;
    float TimePlaying;
    bool HaveBeenReload = false;

    public GameObject Pora;
    public string StatusShareClue;
    public Animator LoadingData;

    public GameObject BtnShoot;

    public GameObject ShootButton1;
    public GameObject ShootButton1_1;
    public GameObject ShootButton2;
    public GameObject ShootButton3;
    public GameObject ShootButton4;
    public GameObject TextRedBubble;
    public GameObject TextOrangeBubble;
    public GameObject Background_RedBubble;
    public GameObject Background_OrangeBubble;
    public GameObject ChooseBubbleUndifines;

	// Use this for initialization
	void Start () {
        ListOfClue = new List<ScreenShot>();
        RefreshSound();

        Score = 0;
        MultiLiftScore = 0;
        BubbleUsedScore = 0;
        RubbishLiftScore = 0;
        SkillScore = 0;
        ScoreTime = 0;

        ReadMessage();
        ReadLevel();
        HaveBeenReload = false;
        successPopupController.InitDataLevel(dataStaticLevel);

        TutorialLevel();
	}

    public void RefreshSound()
    {
        UserSoundConfig config = UserSoundConfig.Load();

        for (int i = 0; i < audioSourcesEffect.Length; i++)
        {
            audioSourcesEffect[i].volume = config.SoundVolume;
            audioSourcesEffect[i].mute = !config.SoundStatus;
        }

        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        for (int i = 0; i < musics.Length; i++)
        {
            musics[i].GetComponent<AudioSource>().volume = config.MusicVolume;
            musics[i].GetComponent<AudioSource>().mute = !config.MusicStatus;
        }
    }

    private void ReadMessage()
    {
        SceneCommunication sceneCommunication = new SceneCommunication(CurrentScene);
        dataRecieveds = sceneCommunication.RetrieveMessages();
    }

    private void ReadLevel()
    {
        if (dataRecieveds.Count > 0)
        {

            if (dataRecieveds[0].Sender == SceneName.LevelEditorCreate || ((dataRecieveds[0].Sender == SceneName.PlayParbaba || dataRecieveds[0].Sender == SceneName.PlayBatuguru || dataRecieveds[0].Sender == SceneName.PlayParapat || dataRecieveds[0].Sender == SceneName.PlayTomok) && Mode == GameplayMode.ValidateLevel))
            {
                Mode = GameplayMode.ValidateLevel;
                dataLevel = (Level)dataRecieveds[0].Data;
                if (!dataLevel.HasUndefineBubble())
                {
                    RefreshLevel(dataLevel);
                }
                else
                {
                    chooseBubbleController.Show(dataStaticLevel, RefreshBubble);
                    State = GameplayState.ChooseBubble;
                    //RefreshBackground(dataLevel.Place);
                    RefreshRubbish(dataLevel.Rubbishes);
                    RefreshBarrier(dataLevel.Barriers);

                }


            }
            else if (dataRecieveds[0].Sender == SceneName.LevelEditorCreate || ((dataRecieveds[0].Sender == SceneName.PlayParbaba || dataRecieveds[0].Sender == SceneName.PlayBatuguru || dataRecieveds[0].Sender == SceneName.PlayParapat || dataRecieveds[0].Sender == SceneName.PlayTomok) && Mode == GameplayMode.Edited))
            {
                Mode = GameplayMode.Edited;
            }
            else if ((dataRecieveds[0].Sender == SceneName.ChooseLevelParbaba || dataRecieveds[0].Sender == SceneName.ChooseLevelBatuguru || dataRecieveds[0].Sender == SceneName.ChooseLevelParapat || dataRecieveds[0].Sender == SceneName.ChooseLevelTomok) || ((dataRecieveds[0].Sender == SceneName.PlayParbaba || dataRecieveds[0].Sender == SceneName.PlayBatuguru || dataRecieveds[0].Sender == SceneName.PlayParapat || dataRecieveds[0].Sender == SceneName.PlayTomok) && Mode == GameplayMode.StaticLevel))
            {
                Mode = GameplayMode.StaticLevel;
                dataStaticLevel = (StaticLevel)dataRecieveds[0].Data;
                if (!dataStaticLevel.HasUndefineBubble())
                {
                    RefreshLevel(dataStaticLevel);
                }
                else
                {
                    BtnShoot.SetActive(false);
                    chooseBubbleController.Show(dataStaticLevel, RefreshBubble);
                    //RefreshBackground(dataStaticLevel.Place);
                    RefreshRubbish(dataStaticLevel.Rubbishes);
                    RefreshBarrier(dataStaticLevel.Barriers);
                    State = GameplayState.ChooseBubble;
					/*if (dataStaticLevel.HighScore==0 && dataStaticLevel.Place == "Batuguru" && dataStaticLevel.Level == 11)
					{
						tutorialPora.SetTrigger("define");
						closeForDefine.SetActive(true);
						tutorialAnimator.SetTrigger("Show");
					}*/
                }

            }
        }
    }

    private void RefreshLevel(Level dataLevel)
    {
        //RefreshBackground(dataLevel.Place);
        RefreshRubbish(dataLevel.Rubbishes);
        RefreshBarrier(dataLevel.Barriers);
        RefreshBubble(dataLevel.Bubbles);
        State = GameplayState.Playing;
		/*if (Mode == GameplayMode.StaticLevel) {
            if (dataStaticLevel.HighScore==0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 1)
			{
				tutorialPora.SetTrigger("start");
				closeForStart.SetActive(true);
				tutorialAnimator.SetTrigger("Show");
                State = GameplayState.Tutorial;
			}
		}*/
    }

	public void EndTutorial()
	{
		State = GameplayState.Playing;
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
            else {
                for (int j = 0; j < Places[i].item.Length; j++)
                {
                    Destroy(Places[i].item[j]);
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
                            objek.GetComponent<BarrierController>().Rotation = penghalang.Rotation;
                            objek.GetComponent<BarrierController>().RotationSpeed = penghalang.RotationSpeed;
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
                if (gelembung.PackageName == Bubbles[i].PackageName)
                {
                    if (gelembung.Type == BubbleType.White)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].WhitePreview);
                        objek.transform.SetParent(ParentObject.BubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = new Vector3(objek.GetComponent<RectTransform>().localPosition.x, objek.GetComponent<RectTransform>().localPosition.y, 0);
                    }
                    else if (gelembung.Type == BubbleType.Red)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].RedPreview);
                        objek.transform.SetParent(ParentObject.BubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = new Vector3(objek.GetComponent<RectTransform>().localPosition.x, objek.GetComponent<RectTransform>().localPosition.y, 0);
                    }
                    else if (gelembung.Type == BubbleType.Orange)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].OrangePreview);
                        objek.transform.SetParent(ParentObject.BubblePreview.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = new Vector3(objek.GetComponent<RectTransform>().localPosition.x, objek.GetComponent<RectTransform>().localPosition.y, 0);
                    }
                }
            }
        }
        State = GameplayState.Playing;
    }

	bool ignoreEsc = true;

	public void AllowEscape()
	{
		ignoreEsc = false;
	}
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey (KeyCode.Escape) && !ignoreEsc) {
			if (State == GameplayState.Playing) {
				Pause();
			} else if(State == GameplayState.Pause){
				pausePopupController.Resume();
			}
			ignoreEsc = true;
			Invoke("AllowEscape",1);
		}
        DeactiveBubbleShooter();
        InputDetector();
        if (State == GameplayState.Playing)
        {
            TimePlaying += Time.deltaTime;

            if (SuccessChecking())
            {
                Success();
                CancelInvoke("Reload");
                CancelInvoke("Fail");
            }
            else if (FailedChecking() && Mode != GameplayMode.ValidateLevel && !HaveBeenReload)
            {
                //Reload();
                if (!IsInvoking("Reload"))
                {
                    Invoke("Reload", 2);
                }
            }
            else if (FailedChecking())
            {
                //Fail();
                if(!IsInvoking("Fail"))
                {
                    Invoke("Fail",2);
                }
            }
            else {
                if (IsInvoking("Reload"))
                {
                    CancelInvoke("Reload");
                }
                if (IsInvoking("Fail"))
                {
                    CancelInvoke("Fail");
                }
            }
        }
    }

    private void DeactiveBubbleShooter()
    {
        if (this.isAllBubbleOut())
        {
            ShootButton.GetComponent<Button>().interactable = false;
            ShotButtonWhiteBubble.SetActive(false);
            ShotButtonRedBubble.SetActive(false);
            ShotButtonOrangeBubble.SetActive(false);
        }
        else
        {
            ShootButton.GetComponent<Button>().interactable = true;
            GameObject itemPreview = ParentObject.BubblePreview.transform.GetChild(0).gameObject;
            ItemGelembungController itemGelembungController = itemPreview.GetComponent<ItemGelembungController>();
            if (itemGelembungController.Type == BubbleType.White)
            {
                ShotButtonWhiteBubble.SetActive(true);
                ShotButtonRedBubble.SetActive(false);
                ShotButtonOrangeBubble.SetActive(false);
            }
            else if (itemGelembungController.Type == BubbleType.Red)
            {
                ShotButtonWhiteBubble.SetActive(false);
                ShotButtonRedBubble.SetActive(true);
                ShotButtonOrangeBubble.SetActive(false);
            }
            else if (itemGelembungController.Type == BubbleType.Orange)
            {
                ShotButtonWhiteBubble.SetActive(false);
                ShotButtonRedBubble.SetActive(false);
                ShotButtonOrangeBubble.SetActive(true);
            }
        }
    }

    void InputDetector()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressBack();
        }
    }

    public void PrepareShooting()
    {
        if (ShootButton.GetComponent<Button>().interactable && State == GameplayState.Playing)
        {
            poraController.PrepareShooting();
        }
    }

    public void Shoot()
    {
        Debug.Log(poraController.IsPreparation);
        Debug.Log(State);
        if (poraController.IsPreparation && State == GameplayState.Playing)
        {
            BubbleController.BubbleSize size = poraController.Shoot();
            Vector2 bubblePosition = poraController.GetShootPosition();
            InstansiateBubble(size, bubblePosition);
            if (size == BubbleController.BubbleSize.One || size == BubbleController.BubbleSize.Two)
            {
                soundSmall.Play();
            }
            else if (size == BubbleController.BubbleSize.Three)
            {
                soundMiddle.Play();
            }
            else {
                soundLarge.Play();
            }
        }
        else
        {
        }
        ScreenShot Clue = new ScreenShot();
        Clue.Pora = new ScreenShot.CluePora(Pora.GetComponent<RectTransform>().localPosition);
        Clue.Place = new ScreenShot.PlaceClue(dataStaticLevel.Place);
        for (int i = 0; i < ParentObject.Barrier.transform.childCount; i++)
        {
            BarrierController controller = ParentObject.Barrier.transform.GetChild(i).gameObject.GetComponent<BarrierController>();

            ScreenShot.CluePenghalang CluePenghalang = new ScreenShot.CluePenghalang();
            CluePenghalang.Packagename = controller.PackageName;
            CluePenghalang.Nama = controller.Name;
            CluePenghalang.Posisi = controller.GetComponent<RectTransform>().localPosition;
            CluePenghalang.KecepatanRotasi = controller.RotationSpeed;
            CluePenghalang.Rotasi = controller.Rotation;
            Clue.Barries.Add(CluePenghalang);
        }
        for (int i = 0; i < ParentObject.Bubble.transform.childCount; i++)
        {
            BubbleController controller = ParentObject.Bubble.transform.GetChild(i).gameObject.GetComponent<BubbleController>();

            ScreenShot.ClueGelembung ClueGelembung = new ScreenShot.ClueGelembung();
            ClueGelembung.Posisi = controller.GetComponent<RectTransform>().localPosition;
            ClueGelembung.Size = controller.SizeInInt;
            ClueGelembung.Type = controller.Type;
            Clue.Bubbles.Add(ClueGelembung);
        }
        for (int i = 0; i < ParentObject.Rubbish.transform.childCount; i++)
        {
            RubbishController controller = ParentObject.Rubbish.transform.GetChild(i).gameObject.GetComponent<RubbishController>();

            ScreenShot.ClueSampah ClueSampah = new ScreenShot.ClueSampah();
            ClueSampah.KecepatanRotasi = controller.RotationSpeed;
            ClueSampah.Rotasi = controller.Rotation;
            ClueSampah.Packagename = controller.PackageName;
            ClueSampah.Posisi = controller.GetComponent<RectTransform>().localPosition;
            ClueSampah.Size = controller.SizeInInt;
            Clue.Rubbish.Add(ClueSampah);
        }
        ListOfClue.Add(Clue);
    }

    public void InstansiateBubble(BubbleController.BubbleSize size, Vector2 position)
    {
        GameObject itemPreview = ParentObject.BubblePreview.transform.GetChild(0).gameObject;
        ItemGelembungController itemGelembungController = itemPreview.GetComponent<ItemGelembungController>();
        for (int i = 0; i < Bubbles.Length; i++)
        {
            if (itemGelembungController.PackageName == Bubbles[i].PackageName)
            {
                if (itemGelembungController.Type == BubbleType.White)
                {
                    GameObject objek = (GameObject)Instantiate(Bubbles[i].White);
                    objek.transform.position = position;
                    objek.GetComponent<BubbleController>().Size = size;
                    objek.transform.SetParent(ParentObject.Bubble.transform);
                    Destroy(itemPreview);
                }
                else if (itemGelembungController.Type == BubbleType.Red)
                {
                    GameObject objek = (GameObject)Instantiate(Bubbles[i].Red);
                    objek.transform.position = position;
                    objek.GetComponent<BubbleController>().Size = size;
                    objek.transform.SetParent(ParentObject.Bubble.transform);
                    Destroy(itemPreview);
                }
                else if (itemGelembungController.Type == BubbleType.Orange)
                {
                    GameObject objek = (GameObject)Instantiate(Bubbles[i].Orange);
                    objek.transform.position = position;
                    objek.GetComponent<BubbleController>().Size = size;
                    objek.transform.SetParent(ParentObject.Bubble.transform);
                    Destroy(itemPreview);
                }
            }
        }
    }

    bool isAllBubbleOut()
    {
        return (ParentObject.BubblePreview.transform.childCount == 0);
    }

    bool isBubbleEmpty()
    {
        return (ParentObject.Bubble.transform.childCount == 0);
    }

    bool isAllBubbleStop()
    {
        for (int i = 0; i < ParentObject.Bubble.transform.childCount; i++)
        {
            if (!ParentObject.Bubble.transform.GetChild(i).gameObject.GetComponent<BubbleController>().IsStop())
            {
                return false;
            }
        }
        return true;
    }

    bool isRubbishEmpty()
    {   
        return ParentObject.Rubbish.transform.childCount == 0;
    }

    bool FailedChecking()
    {
        return (((isAllBubbleOut() && isAllBubbleStop()) || (isAllBubbleOut() && isBubbleEmpty())) && !isRubbishEmpty());
    }

    bool SuccessChecking()
    {
        return isRubbishEmpty();
    }

    public void Reload()
    {
        if (Mode == GameplayMode.StaticLevel)
        {
            if (!reloadPopupController.IsShowing)
                BtnShoot.SetActive(false);
                reloadPopupController.Show(dataStaticLevel, CallbackYesReload, CallbackNoReload);
        }
        State = GameplayState.Reload;
    }

    public void CallbackYesReload(List<Gelembung> daftarGelembung)
    {
        RefreshBubble(daftarGelembung);
        HaveBeenReload = true;
        State = GameplayState.Playing;
    }

    public void CallbackNoReload()
    {
        Invoke("Fail", 0.5f);
    }

    public void Fail()
    {
        if (Mode == GameplayMode.StaticLevel)
        {
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",timePlaying:" + TimePlaying.ToString(); ;
            command += ",status:4_FAILED";
            command += "}";
            ServerStatistic.DoRequest(command);
             

            failedPopupController.ShowForPlayingStaticLevel(FailCallback);
        }
        else if (Mode == GameplayMode.Edited)
        {
            failedPopupController.ShowForPlayingEditedLevel(FailCallback);
        }
        else if (Mode == GameplayMode.ValidateLevel)
        {
            failedPopupController.ShowForValidateLevel(FailCallback);
        }
        State = GameplayState.Failed;
    }

    public void FailCallback(FailPopupController.ButtonName button)
    {
        if (button == FailPopupController.ButtonName.Menu)
        {
            BackToMenu();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:5_FAILED_BACKTOMENU";
            command += "}";
            ServerStatistic.DoRequest(command);
             
        }
        else if (button == FailPopupController.ButtonName.Replay)
        {
            Replay();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:5_FAILED_REPLAY";
            command += "}";
            ServerStatistic.DoRequest(command);
             
        }
        else if (button == FailPopupController.ButtonName.BackToEditor)
        {
            BackToMenu();
        }
    }

    public void Success()
    {
        if (State != GameplayState.Success)
        {
            PlayManager.ScoreTime = (int)(1200 - (float.Parse(TimePlaying.ToString("F1")) * 10));
            PlayManager.Score += PlayManager.ScoreTime;
            

            if (Score < 0)
                Score = 0;
            
            State = GameplayState.Success;
            if (Mode == GameplayMode.StaticLevel)
            {
                UserScoreData userScore = UserScoreData.Load();
                int BintangSebelumnya = userScore.GetStar(dataStaticLevel.Place, dataStaticLevel.Level);
                dataStaticLevel.HighScore = Score;

                if (Score >= dataStaticLevel.ScoreFor3Star)
                {
                    Bintang = 3;
                }
                else if (Score >= dataStaticLevel.ScoreFor2Star)
                {
                    Bintang = 2;
                }
                else
                {
                    Bintang = 1;
                }

                UserStockData userStockDagta = UserStockData.Load();
                List<BubbleType> bubblesGet = new List<BubbleType>();
                int maksGold = 0; ;
                int goldGet = 0;

                if (Bintang == 3 && BintangSebelumnya == 3)
                {
                    maksGold = (int)(Score / 100f);
                    goldGet = maksGold;
                }
                else if (Bintang == 3 && BintangSebelumnya == 2)
                {
                    maksGold = (int)((Score / 100f) * 10);
                    goldGet = (int)(maksGold * ((11 / 11f) - (5 / 11f)));
                }
                else if (Bintang == 3 && BintangSebelumnya == 1)
                {
                    maksGold = (int)((Score / 100f) * 10);
                    goldGet = (int)(maksGold * ((11 / 11f) - (2 / 11f)));
                }
                else if (Bintang == 3 && BintangSebelumnya == 0)
                {
                    maksGold = (int)((Score / 100f) * 10);
                    goldGet = (int)(maksGold * (11 / 11f));
                }
                else if (Bintang == 2 && BintangSebelumnya >= 2)
                {
                    maksGold = (int)(Score / 100f);
                    goldGet = maksGold;
                }
                else if (Bintang == 2 && BintangSebelumnya == 1)
                {
                    maksGold = (int)((Score / 100f) * 10);
                    goldGet = (int)(maksGold * ((5 / 11f) - (2 / 11f)));
                }
                else if (Bintang == 2 && BintangSebelumnya == 0)
                {
                    maksGold = (int)((Score / 100f) * 10);
                    goldGet = (int)(maksGold * (5 / 11f));
                }
                else if (Bintang == 1 && BintangSebelumnya >= 1)
                {
                    maksGold = (int)(Score / 100f);
                    goldGet = maksGold;
                }
                else if (Bintang == 1 && BintangSebelumnya == 0)
                {
                    maksGold = (int)((Score / 100f) * 10);
                    goldGet = (int)(maksGold * (2 / 11f));
                }

                if (Bintang == 3)
                {
                    int indicator = Random.Range(1,282);
                    if(indicator >= 1 && indicator <= 94)
                    {
                        bubblesGet.Add(BubbleType.White);
                    }
                    else if(indicator >= 95 && indicator <= 188)
                    {
                        bubblesGet.Add(BubbleType.Red);
                    }
                    else if(indicator >=189  && indicator <= 282)
                    {
                        bubblesGet.Add(BubbleType.Orange);
                    }

                    indicator = Random.Range(1,282);
                    if(indicator >= 1 && indicator <= 94)
                    {
                        bubblesGet.Add(BubbleType.White);
                    }
                    else if(indicator >= 95 && indicator <= 188)
                    {
                        bubblesGet.Add(BubbleType.Red);
                    }
                    else if(indicator >=189  && indicator <= 282)
                    {
                        bubblesGet.Add(BubbleType.Orange);
                    }
                }
                else if (Bintang == 2)
                {
                    int indicator = Random.Range(1,282);
                    if(indicator >= 1 && indicator <= 94)
                    {
                        bubblesGet.Add(BubbleType.White);
                    }
                    else if(indicator >= 95 && indicator <= 188)
                    {
                        bubblesGet.Add(BubbleType.Red);
                    }
                    else if(indicator >=189  && indicator <= 282)
                    {
                        bubblesGet.Add(BubbleType.Orange);
                    }
                }

                dataStaticLevel.Save();
                userStockDagta.PlusMinGold(goldGet);

                int bubbleWhiteGet = 0;
                int bubbleRedGet = 0;
                int bubbleOrangeGet = 0;
                foreach(BubbleType typeGet in bubblesGet)
                {
                    if (typeGet == BubbleType.White)
                    {
                        userStockDagta.PlusMinBubble(1, 0, 0);
                        bubbleWhiteGet++;
                    }
                    else if (typeGet == BubbleType.Red)
                    {
                        userStockDagta.PlusMinBubble(0, 1, 0);
                        bubbleRedGet++;
                    }
                    else if (typeGet == BubbleType.Orange)
                    {
                        userStockDagta.PlusMinBubble(0, 0,1);
                        bubbleOrangeGet++;
                    }
                }

                //Statistik
                string command = "{";
                command += "action:PlAY_LEVEL";
                command += ",place:"+dataStaticLevel.Place;
                command += ",level:"+dataStaticLevel.Level;
                command += ",status:4_SUCCESS";
                command += ",timePlaying:" + TimePlaying.ToString();
                if (FB.IsLoggedIn)
                {
                    command += ",facebook_id:" + FB.UserId;
                }
                command += ",score:"+Score;
                command += ",goldGet:" + goldGet;
                command += ",bubbleGetWhite:" + bubbleWhiteGet;
                command += ",bubbleGetRed:" + bubbleRedGet;
                command += ",bubbleGetOrange:" + bubbleOrangeGet;
                command += "}";
                ServerStatistic.DoRequest(command);

                successPopupController.ShowForPlayingStaticLevel(Score, Bintang,goldGet, bubblesGet, SuccessCallback);
            }
            else if (Mode == GameplayMode.Edited)
            {
                if (Score > dataLevel.ScoreFor3Star)
                {
                    Bintang = 3;
                }
                else if (Score > dataLevel.ScoreFor2Star)
                {
                    Bintang = 2;
                }
                else
                {
                    Bintang = 1;
                }

                successPopupController.ShowForPlayingEditedLevel(Score, Bintang, SuccessCallback);
                Debug.Log("Edited");
            }
            else if (Mode == GameplayMode.ValidateLevel)
            {
                if (Score > dataLevel.ScoreFor3Star)
                {
                    Bintang = 3;
                }
                else if (Score > dataLevel.ScoreFor2Star)
                {
                    Bintang = 2;
                }
                else
                {
                    Bintang = 1;
                }
                successPopupController.ShowForValidateLevel(Score, Bintang, SuccessCallback);
                Debug.Log("ValidateLevel");
            }
        }
        Debug.Log("Score Time :"+PlayManager.ScoreTime);
        Debug.Log("Time Playing :" + TimePlaying);
        Debug.Log("Time Playing dengan 1 Decimal:" + float.Parse(TimePlaying.ToString("F1")) * 10);
    }

    public void SuccessCallback(SuccessPopupController.ButtonName buttonName)
    {
        if (buttonName == SuccessPopupController.ButtonName.Next)
        {
            NextLevel();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:5_SUCCESS_NEXT";
            command += "}";
            ServerStatistic.DoRequest(command);
        }
        else if (buttonName == SuccessPopupController.ButtonName.Menu)
        {
            BackToMenu();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:5_SUCCESS_BACKTOMENU";
            command += "}";
            ServerStatistic.DoRequest(command);
        }
        else if (buttonName == SuccessPopupController.ButtonName.Replay)
        {
            Replay();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",status:5_SUCCESS_REPLAY";
            command += "}";
            ServerStatistic.DoRequest(command);
        }
        else if (buttonName == SuccessPopupController.ButtonName.BackToEditor)
        {
            ValidationStatus = false;
            BackToMenu();
        }
        else if (buttonName == SuccessPopupController.ButtonName.Save)
        {
            ValidationStatus = true;
            SaveStatus = true;
            BackToMenu();
        }
    }

    public void Pause()
    {
        if (IsPlaying())
        {
            State = GameplayState.Pause;
            CancelInvoke("Reload");
            CancelInvoke("Fail");
            if (Mode == GameplayMode.StaticLevel)
            {
                pausePopupController.ShowForPlayingStaticLevel(PauseCallback);
                Debug.Log("ShowForPlayingStaticLevel");
            }
            else if (Mode == GameplayMode.Edited)
            {
                pausePopupController.ShowForPlayingEditedLevel(PauseCallback);
                Debug.Log("ShowForPlayingEditedLevel");
            }
            else if (Mode == GameplayMode.ValidateLevel)
            {
                pausePopupController.ShowForValidateLevel(PauseCallback);
                Debug.Log("ShowForValidateLevel");
            }
        }
    }

    private void UnPause()
    {
        if (IsPause())
        {
			pausePopupController.Hide();
            State = GameplayState.Playing;
        }
    }

    public static bool IsPause()
    {
        return State == GameplayState.Pause;
    }

    public static bool IsPlaying()
    {
        return State == GameplayState.Playing;
    }


    public void PauseCallback(PausePopupController.ButtonName button)
    {
        if (button == PausePopupController.ButtonName.Resume)
        {
            UnPause();
        }
        else if (button == PausePopupController.ButtonName.Menu)
        {
            BackToMenu();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",timePlaying:" + TimePlaying.ToString();
            command += ",status:4_PAUSE_BACKTOMENU";
            command += "}";
            ServerStatistic.DoRequest(command);
        }
        else if (button == PausePopupController.ButtonName.Replay)
        {
            Replay();
            //Statistik
            string command = "{";
            command += "action:PlAY_LEVEL";
            command += ",place:"+dataStaticLevel.Place;
            command += ",level:"+dataStaticLevel.Level;
            command += ",timePlaying:" + TimePlaying.ToString();
            command += ",status:4_PAUSE_REPLAY";
            command += "}";
            ServerStatistic.DoRequest(command);
        }
    }

    public void NextLevel()
    {
        int Level = dataStaticLevel.Level + 1;
        if (Level == 21)
        {
            BackToMenu();
        }
        else
        {
            BacksoundController.IsActive = false;
            BacksoundController.DestroyNow = true;
            BacksoundIntroController.DestroyNow = false;
            SceneCommunication scom = new SceneCommunication(CurrentScene);
            //scom.SendMessage(CurrentScene, new StaticLevel(dataStaticLevel.Season, dataStaticLevel.Place, dataStaticLevel.Level + 1));
            if (CurrentScene == SceneName.PlayBatuguru) {
                scom.SendMessage(SceneName.ChooseLevelBatuguru, new StaticLevel(dataStaticLevel.Season, dataStaticLevel.Place, dataStaticLevel.Level + 1));
                Application.LoadLevel(SceneName.ChooseLevelBatuguru.ToString());
            }
            else if (CurrentScene == SceneName.PlayTomok)
            {
                scom.SendMessage(SceneName.ChooseLevelTomok, new StaticLevel(dataStaticLevel.Season, dataStaticLevel.Place, dataStaticLevel.Level + 1));
                Application.LoadLevel(SceneName.ChooseLevelTomok.ToString());
            }
            else if (CurrentScene == SceneName.PlayParapat)
            {
                scom.SendMessage(SceneName.ChooseLevelParapat, new StaticLevel(dataStaticLevel.Season, dataStaticLevel.Place, dataStaticLevel.Level + 1));
                Application.LoadLevel(SceneName.ChooseLevelParapat.ToString());
            }
            else if (CurrentScene == SceneName.PlayParbaba)
            {
                scom.SendMessage(SceneName.ChooseLevelParbaba, new StaticLevel(dataStaticLevel.Season, dataStaticLevel.Place, dataStaticLevel.Level + 1));
                Application.LoadLevel(SceneName.ChooseLevelParbaba.ToString());
            }
        }
    }

    public void BackToMenu()
    {
        BacksoundController.IsActive = false;
        BacksoundController.DestroyNow = true;
        BacksoundIntroController.DestroyNow = false;
        if (Mode == GameplayMode.StaticLevel)
        {
            if (CurrentScene == SceneName.PlayParbaba)
            {
                SceneCommunication scom = new SceneCommunication(CurrentScene);
                scom.SendMessage(SceneName.ChooseLevelParbaba, dataStaticLevel.Place);
                scom.SendMessage(SceneName.ChooseLevelParbaba, dataStaticLevel.Level);
                Application.LoadLevel(SceneName.ChooseLevelParbaba.ToString());
            }
            else if (CurrentScene == SceneName.PlayBatuguru)
            {
                SceneCommunication scom = new SceneCommunication(CurrentScene);
                scom.SendMessage(SceneName.ChooseLevelBatuguru, dataStaticLevel.Place);
                scom.SendMessage(SceneName.ChooseLevelBatuguru, dataStaticLevel.Level);
                Application.LoadLevel(SceneName.ChooseLevelBatuguru.ToString());
            }
            else if (CurrentScene == SceneName.PlayParapat)
            {
                SceneCommunication scom = new SceneCommunication(CurrentScene);
                scom.SendMessage(SceneName.ChooseLevelParapat, dataStaticLevel.Place);
                scom.SendMessage(SceneName.ChooseLevelParapat, dataStaticLevel.Level);
                Application.LoadLevel(SceneName.ChooseLevelParapat.ToString());
            }
            else if (CurrentScene == SceneName.PlayTomok)
            {
                SceneCommunication scom = new SceneCommunication(CurrentScene);
                scom.SendMessage(SceneName.ChooseLevelTomok, dataStaticLevel.Place);
                scom.SendMessage(SceneName.ChooseLevelTomok, dataStaticLevel.Level);
                Application.LoadLevel(SceneName.ChooseLevelTomok.ToString());
            }
        }
        else if (Mode == GameplayMode.Edited)
        {
            Application.LoadLevel(SceneName.LevelEditorMenu.ToString());
        }
        else if (Mode == GameplayMode.ValidateLevel)
        {
            Level level = dataLevel;
            SceneCommunication scom = new SceneCommunication(CurrentScene);
            scom.SendMessage(SceneName.LevelEditorCreate, level);
            scom.SendMessage(SceneName.LevelEditorCreate, ValidationStatus);
            scom.SendMessage(SceneName.LevelEditorCreate, SaveStatus);
            Application.LoadLevel(SceneName.LevelEditorCreate.ToString());
        }
    }

    public void Replay()
    {
        SceneCommunication scom = new SceneCommunication(CurrentScene);
        if (Mode == GameplayMode.StaticLevel)
            scom.SendMessage(CurrentScene, dataStaticLevel);
        else if (Mode == GameplayMode.ValidateLevel || Mode == GameplayMode.Edited)
            scom.SendMessage(CurrentScene, dataLevel);

        scom.SendMessage(CurrentScene, false);
        Application.LoadLevel(CurrentScene.ToString());
    }

    public void PressBack()
    {
        if (!IsPause())
            Pause();
        else
            pausePopupController.Resume();
    }

    void OnApplicationPause(bool pauseStatus)
    {
		if (State == GameplayState.Playing)
        {
            Pause();
        }
    }

    public void ShareClueConfirmation()
    {
        BtnSharedClue.SetActive(false);
        confirmationPopupController.Show("Do you want to Share Clue?", ShareClue, NoShare);
    }

    public void NoShare()
    {
        BtnSharedClue.SetActive(true);
    }

    public void ShareClue() {
        string split = "#";
        string data = "";
        int a = 0;
        foreach(ScreenShot screen in ListOfClue){
            a++;
            data += ScreenShot.Serialize(screen)+(a==ListOfClue.Count?"": split);

        }
        string url = "http://nedstudio.net/ned/api/web/v1/clues/share-clue";
        WWWForm form = new WWWForm();
        form.AddField("clue_data", data);
        form.AddField("facebook_id", FB.UserId);
        form.AddField("app_token", UserDataManager.APP_TOKEN);
        form.AddField("level_id",dataStaticLevel.GlobalId);
        form.AddField("score", dataStaticLevel.ScoreFor3Star);
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
        LoadingData.SetTrigger("Show");
    }

    IEnumerator WaitForRequest(WWW www)
    {
         yield return www;
            
         // check for errors
         if (www.error == null)
         {
             messagePopupController.Show("Success Share Clue and You Earned 250 Coins!");
             UserStockData RewardShare = UserStockData.Load();
             RewardShare.PlusMinGold(250);
             Debug.Log("WWW Ok!: " + www.text);
             StatusShareClue = "OK";
             LoadingData.SetTrigger("Hide");
             BtnSharedClue.SetActive(false);
         }
         else
         {
             Debug.Log("WWW Error: " + www.error);
             LoadingData.SetTrigger("Hide");
             messagePopupController.Show("Failed To Share Clue. Check your Internet Connection!");
             BtnSharedClue.SetActive(true);
         }    
    }

    public void TutorialLevel() {
        if (Mode == GameplayMode.StaticLevel)
        {
            if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 1)
            {
                ButtonShoot.SetTrigger("Show");
                ShootButton1.SetActive(true);
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 2)
            {
                ButtonShoot.SetTrigger("Show");
                ShootButton2.SetActive(true);
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 3)
            {
                ButtonShoot.SetTrigger("Hold");
                ShootButton1.SetActive(true);
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 6)
            {
                NormalBarrier.SetTrigger("Show");
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 7)
            {
                ShootButton3.SetActive(true);
                TextRedBubble.SetActive(true);
                Background_RedBubble.SetActive(true);
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 12)
            {
                NormalBarrier.SetTrigger("Show2");
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Parbaba" && dataStaticLevel.Level == 18)
            {
                Background_OrangeBubble.SetActive(true);
                TextOrangeBubble.SetActive(true);
                ShootButton4.SetActive(true);
            }
            else if (dataStaticLevel.HighScore == 0 && dataStaticLevel.Place == "Batuguru" && dataStaticLevel.Level == 11)
            {
                BubbleUndifine.SetTrigger("Show");
                ChooseBubbleUndifines.SetActive(true);
            }
        }
    }
}
