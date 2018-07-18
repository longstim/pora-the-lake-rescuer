using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemMenuLevel : MonoBehaviour
{

    public Sprite FinishedSprite;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    public PrePlayPopupManager PrePlayPopup;

    public string Season;
    public string Place;
    public int Level;

    public string RequiredPlace;
    public int RequiredLevel;
    public GameObject[] Rubbishes;

    StaticLevel dataLevel;

    public bool canPlay = false;
    // Use this for initialization

    void OnEnable()
    {
        LoadingPopupController.OnReadAllScoresSuccess += LoadingPopupController_OnReadAllScoresSuccess;
    }

    void Disable()
    {
        LoadingPopupController.OnReadAllScoresSuccess -= LoadingPopupController_OnReadAllScoresSuccess;
    }

    void LoadingPopupController_OnReadAllScoresSuccess()
    {
        Refresh();
    }

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        dataLevel = new StaticLevel(Season, Place, Level);

        UserScoreData userScoreData = UserScoreData.Load();
        if (RequiredLevel < 1)
        {
            RequiredLevel = 1;
        }
        if (userScoreData.GetScore(RequiredPlace, RequiredLevel) > 0 || (Place == "Parbaba" && Level == 1) || canPlay)
        {
            canPlay = true;
            Image imageComponent = GetComponent<Image>();
            imageComponent.sprite = FinishedSprite;
        }
        if (dataLevel.HighScore > 0)
        {
            HideRubbishes();
        }
        ShowStar();
    }


    public void ShowStar()
    {
        Star1.SetActive(dataLevel.HighScore > 0);
        Star2.SetActive(dataLevel.HighScore >= dataLevel.ScoreFor2Star);
        Star3.SetActive(dataLevel.HighScore >= dataLevel.ScoreFor3Star);
    }

    public void HideRubbishes()
    {
        for (int i = 0; i < Rubbishes.Length; i++)
        {
            Rubbishes[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (canPlay)
        {
            PrePlayPopup.Show(dataLevel);
        }
    }

}
