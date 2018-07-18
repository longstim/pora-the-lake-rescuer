using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Facebook;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class ItemLeaderboard : MonoBehaviour {

    public Text textPosition;
    public Text textName;
    public Text textScore;
    public PoraController poraProfile;

    WatchClueManager watchClueManager;
    string globaId;

    public GameObject ButtonWatchClue;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(int position, string name, int score, string headwear, string bodywear, string gun)
    {
        textPosition.text = position.ToString();
        textName.text = name.ToString();
        textScore.text = score.ToString();
    }

    public void WatchClue()
    {
        watchClueManager.Show(globaId);
    }

    public void ShowWatchClue(WatchClueManager watchClueManager, string clueId)
    {
        globaId = clueId;
        this.watchClueManager = watchClueManager;
        Debug.Log(clueId);
        Debug.Log(ButtonWatchClue != null);
        if (ButtonWatchClue != null)
            ButtonWatchClue.SetActive(true);
    }
}
