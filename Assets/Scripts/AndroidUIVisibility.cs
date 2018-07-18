using UnityEngine;
using System.Collections;

public class AndroidUIVisibility : MonoBehaviour
{

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

		//int SYSTEM_UI_FLAG_IMMERSIVE = 2048;
		int SYSTEM_UI_FLAG_IMMERSIVE_STICKY = 4096;
		int SYSTEM_UI_FLAG_HIDE_NAVIGATION = 2;
		int SYSTEM_UI_FLAG_FULLSCREEN = 4;
		//int SYSTEM_UI_FLAG_LAYOUT_STABLE = 256;
		//int SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION = 512;
		//int SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN = 1024;
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
		AndroidJavaObject decorView = window.Call<AndroidJavaObject>("getDecorView");
		decorView.Call("setSystemUiVisibility",SYSTEM_UI_FLAG_FULLSCREEN | SYSTEM_UI_FLAG_HIDE_NAVIGATION | SYSTEM_UI_FLAG_IMMERSIVE_STICKY);
#endif
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
