using UnityEngine;
using System.Collections;

public class PausePopupController : MonoBehaviour {

	public enum ButtonName{
		Resume,
		Menu,
		Replay,
		SoundOn,
		SoundOff,
		Instruction
	}
	public delegate void Callback(ButtonName button);
    public GameObject UnpauseButton;
    public GameObject PauseButton;
    public GameObject BlurImage;
    public GameObject ButtonSoundOn;
    public GameObject ButtonSoundoff;
	private Callback callback;
    public PlayManager playManager;
    public Animator animatorTutorial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ShowForPlayingStaticLevel(Callback callback)
	{
		this.callback = callback;
        UnpauseButton.SetActive(true);
        BlurImage.SetActive(true);
        PauseButton.SetActive(false);
        GetComponent<Animator>().SetTrigger("Show");
        UserSoundConfig config = UserSoundConfig.Load();
        if (config.SoundStatus == true || config.MusicStatus == true)
        {
            ButtonSoundOn.SetActive(true);
            ButtonSoundoff.SetActive(false);
        }
        else {
            ButtonSoundOn.SetActive(false);
            ButtonSoundoff.SetActive(true);
        }
	}

	public void ShowForPlayingEditedLevel(Callback callback)
	{
		this.callback = callback;
        UnpauseButton.SetActive(true);
        BlurImage.SetActive(true);
        GetComponent<Animator>().SetTrigger("Show");
	}

	public void ShowForValidateLevel(Callback callback)
	{
		this.callback = callback;
        UnpauseButton.SetActive(true);
        GetComponent<Animator>().SetTrigger("Show");
	}

    public void Show()
    {
        GetComponent<Animator>().SetTrigger("Show");
    }

	public void Hide()
	{
        UnpauseButton.SetActive(false);
        PauseButton.SetActive(true);
        BlurImage.SetActive(false);
        GetComponent<Animator>().SetTrigger("Hide");
	}

	public bool IsShowing(){
        return GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Show");
	}

	public void Resume()
	{
        if (animatorTutorial.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
        {
            callback(ButtonName.Resume);
            Hide();
        }
	}

	public void Menu()
	{
		callback (ButtonName.Menu);
		Hide ();
	}

	public void Replay()
	{
		callback (ButtonName.Replay);
		Hide ();
	}

	public void Instruction()
	{
		callback (ButtonName.Instruction);
		Hide ();
	}

	public void SoundOn()
	{
        UserSoundConfig config = UserSoundConfig.Load();
        config.MusicStatus = true;
        config.SoundStatus = true;
        config.Save();
        if (config.SoundStatus == true || config.MusicStatus == true)
        {
            ButtonSoundOn.SetActive(true);
            ButtonSoundoff.SetActive(false);
        }
        else
        {
            ButtonSoundOn.SetActive(false);
            ButtonSoundoff.SetActive(true);
        }
        playManager.RefreshSound();
	}

	public void SoundOff()
	{
        UserSoundConfig config = UserSoundConfig.Load();
        config.MusicStatus = false;
        config.SoundStatus = false;
        config.Save();
        if (config.SoundStatus == true || config.MusicStatus == true)
        {
            ButtonSoundOn.SetActive(true);
            ButtonSoundoff.SetActive(false);
        }
        else
        {
            ButtonSoundOn.SetActive(false);
            ButtonSoundoff.SetActive(true);
        }
        playManager.RefreshSound();
	}

}
