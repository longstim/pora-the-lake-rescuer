using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public class SuccessPopupController : MonoBehaviour {

    public static string LevelName;

	public enum ButtonName{
		Menu,
		Replay,
		Next,
		Save,
		BackToEditor
	}
	public delegate void Callback(ButtonName button);

	public int StartScore = 0;
	public int ScoreIncrement = 17;
	public Text TextScore;
    public GameObject ButtonSave;
    public GameObject ButtonNext;
    public Text GoldBonusText;
    public Text WhiteBubbleBonusText;
    public Text RedBubbleBonusText;
    public Text OrangeBubbleBonusText;
	public Text MultiLift;
	public Text BubbleScore;
	public Text TimeScore;
	public Text SkillScore;
	public Text RubbishScore;
    public GameObject GoldBonus;
    public GameObject WhiteBubbleBonus;
    public GameObject RedBubbleBonus;
    public GameObject OrangeBubbleBonus;
    public GameObject BtnShareClue;
    public GameObject BtnNextFor3;
    public GameObject BtnNext;
	public Animator pora;
    public AudioSource slideStar;
    public AudioSource star1Audio;
    public AudioSource star2Audio;
    public AudioSource star3Audio;
    public AudioSource scoreAudio;
	int score;
    int scoreProgress = 0;
	int multiscore;
    private int ScoreProgress
    {
		get {
			return scoreProgress;
		}
		set {
            scoreProgress = value;
            TextScore.text = scoreProgress.ToString();
		}
	}

	int Bintang;
    int WhiteBubble;
    int RedBubble;
    int OrangeBubble;
	Callback callback;

    public StaticLevel dataLevelStatic;
    public bool ResultCheckClue;
    int CountRequestConnect = 1;

	// Use this for initialization
    Animator animator;
    UserSoundConfig config;
	void Start () {
        animator=GetComponent<Animator>();
        config = UserSoundConfig.Load();
		pora.SetTrigger ("happy");
        scoreAudio.volume = config.SoundVolume;
        scoreAudio.mute = !config.SoundStatus;
        CountRequestConnect = 1;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Show"))
        {
            if (ScoreProgress + ScoreIncrement < score)
            {
                ScoreProgress += ScoreIncrement;
                if (!scoreAudio.mute && !scoreAudio.isPlaying)
                {
                    scoreAudio.Play();
                }
            }
            else
            {
                scoreAudio.Stop();
                ScoreProgress = score;
                if (Bintang == 1)
                {
                    animator.SetTrigger("Star1");
                }
                else if (Bintang == 2)
                {
                    animator.SetTrigger("Star2");
                }
                else if (Bintang == 3)
                {
                    animator.SetTrigger("Star3");
                }
            }
        }
	}

	public void ShowForPlayingStaticLevel(int score, int bintang,int goldGet, List<BubbleType> bubbleGet, Callback callback)
	{
        this.ScoreProgress = StartScore;
		this.score = score;
		this.Bintang = bintang;
		this.callback = callback;
        Debug.Log("Check CLue :"+ResultCheckClue.ToString());
        GetComponent<Animator>().SetTrigger("Show");

        if (bintang == 3 && FB.IsLoggedIn && ResultCheckClue.ToString() == "False") {
            BtnShareClue.SetActive(true);
            BtnNextFor3.SetActive(true);
        }
        if (bintang == 3 && FB.IsLoggedIn && ResultCheckClue.ToString() == "True")
        {
            BtnNext.SetActive(true);
        }
        else if (bintang == 3 && !FB.IsLoggedIn) {
           BtnNext.SetActive(true);
        }
        else if (Bintang < 3)
        {
            BtnNext.SetActive(true);
        }

        ButtonSave.SetActive(false);
        ButtonNext.SetActive(true);

        foreach (BubbleType typeGet in bubbleGet)
        {
            if (typeGet == BubbleType.White)
            {
                WhiteBubble++;
            }
            else if (typeGet == BubbleType.Red)
            {
                RedBubble++;
            }
            else if (typeGet == BubbleType.Orange)
            {
                OrangeBubble++;
            }
        }

		MultiLift.text = PlayManager.MultiLiftScore.ToString();
		BubbleScore.text = PlayManager.BubbleUsedScore.ToString();
		SkillScore.text = PlayManager.SkillScore.ToString();
		TimeScore.text = PlayManager.ScoreTime.ToString();
		RubbishScore.text = PlayManager.RubbishLiftScore.ToString();
		GoldBonusText.text = goldGet.ToString() ;
        WhiteBubbleBonusText.text = WhiteBubble.ToString();
        RedBubbleBonusText.text = RedBubble.ToString();
        OrangeBubbleBonusText.text = OrangeBubble.ToString();

        GoldBonus.SetActive(goldGet != 0);
        WhiteBubbleBonus.SetActive(WhiteBubble != 0);
        RedBubbleBonus.SetActive(RedBubble != 0);
        OrangeBubbleBonus.SetActive(OrangeBubble != 0);
	}

	public void ShowForPlayingEditedLevel(int score, int bintang, Callback callback)
	{
        this.ScoreProgress = StartScore;
		this.score = score;
		this.Bintang = bintang;
		this.callback = callback;
        GetComponent<Animator>().SetTrigger("Show");

        ButtonSave.SetActive(false);
        ButtonNext.SetActive(false);

        GetComponent<AudioSource>().Play();
	}

	public void ShowForValidateLevel(int score, int bintang, Callback callback)
	{
        this.ScoreProgress = StartScore;
		this.score = score;
		this.Bintang = bintang;
		this.callback = callback;
        GetComponent<Animator>().SetTrigger("Show");

        ButtonSave.SetActive(true);
        ButtonNext.SetActive(false);

        GoldBonus.SetActive(false);
        WhiteBubbleBonus.SetActive(false);
        RedBubbleBonus.SetActive(false);
        OrangeBubbleBonus.SetActive(false);

        GetComponent<AudioSource>().Play();
	}


	public void Hide()
	{
		gameObject.SetActive (false);
	}

	public void Save()
	{
		callback (ButtonName.Save);
		Hide ();
	}

	public void Editor()
	{
		callback (ButtonName.BackToEditor);
		Hide ();
	}

	public void Replay()
	{
		callback (ButtonName.Replay);
		Hide ();
	}

	public void NextLevel()
	{
		callback (ButtonName.Next);
		Hide ();
	}

	public void Menu()
	{
		callback (ButtonName.Menu);
		Hide ();
	}

    public void LevelNameInput(Text inputName)
    {
        LevelName = inputName.text;
    }

    public void PlaySlideStar()
    {
        slideStar.Play();
    }

    public void PlayStar1()
    {
        star1Audio.Play();
    }
    public void PlayStar2()
    {
        star2Audio.Play();
    }
    public void PlayStar3()
    {
        star3Audio.Play();
    }

    public void HideBtnShareClue() {
        BtnShareClue.SetActive(false);
    }

    public void ShowBtnShareClue()
    {
        BtnShareClue.SetActive(true);
    }

    public void CheckClue()
    {
        string url = "http://nedstudio.net/ned/api/web/v1/clues/ask-clue";
        WWWForm form = new WWWForm();
        form.AddField("facebook_id", FB.UserId);
        form.AddField("app_token", "kshfishmkmwsdui");
        form.AddField("level_id", dataLevelStatic.GlobalId);
        Debug.Log(dataLevelStatic.Place);
        Debug.Log(dataLevelStatic.GlobalId);
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequestCheckClue(www));
    }

    IEnumerator WaitForRequestCheckClue(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
            ResultCheckClue = ((bool)dict["data"]);
            Debug.Log("WWW Ok!: " + www.text);
            Debug.Log(ResultCheckClue.ToString());
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
            if (CountRequestConnect < 3)
            {
                CheckClue();
            }
            else {
                var dict = Json.Deserialize(www.text) as Dictionary<string, object>;
                ResultCheckClue = ((bool)dict["data"]);
            }

        }
    }

    public void InitDataLevel(StaticLevel dataLevelStatic) {
        this.dataLevelStatic = dataLevelStatic;
        CountRequestConnect = 1;
        CheckClue();
    }

    public void AfterClickClue()
    {
        BtnNextFor3.SetActive(false);
        BtnNext.SetActive(true);
    }

}
