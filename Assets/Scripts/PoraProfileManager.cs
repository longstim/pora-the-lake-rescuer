using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoraProfileManager : MonoBehaviour {

    [System.Serializable]
    public class PoraItem
    {
        public string Id;
        public GameObject[] ObjectItems;
    }

    static List<string> UsedItems = new List<string>();

    public bool IsLeaderboard = false;
    public AudioSource SoundNgantuk;
    public AudioSource SoundBangun;

	// Use this for initialization
	void Start () {
        UserSoundConfig config = UserSoundConfig.Load();
        SoundNgantuk.volume = config.SoundVolume;
        SoundNgantuk.mute = !config.SoundStatus && !IsLeaderboard;

        SoundBangun.volume = config.SoundVolume;
        SoundBangun.mute = !config.SoundStatus && !IsLeaderboard;
	}

    public void PlaySoundNgantuk()
    {
        UserSoundConfig config = UserSoundConfig.Load();
        SoundNgantuk.volume = config.SoundVolume;
        SoundNgantuk.mute = !config.SoundStatus && !IsLeaderboard;

        if (!SoundNgantuk.isPlaying)
        {
            SoundNgantuk.Play();
        }
    }

    public void PlaySoundBangun()
    {
        UserSoundConfig config = UserSoundConfig.Load();

        SoundBangun.volume = config.SoundVolume;
        SoundBangun.mute = !config.SoundStatus && !IsLeaderboard;

        if (!SoundBangun.isPlaying)
        {
            SoundBangun.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
