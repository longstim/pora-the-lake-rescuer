using UnityEngine;
using System.Collections;
using System.IO;
public class  ShareScreenshotButtonController : MonoBehaviour {
    
    private bool isProcessing = false;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (isProcessing)
        {
            transform.localScale = Vector3.zero;
        }
    }

    public void OnClick()
    {
        if (!isProcessing)
        {
            isProcessing = true;
            StartCoroutine("ShareScreenshot");
        }
    }

    public IEnumerator ShareScreenshot()
    {

        yield return new WaitForEndOfFrame();
        
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,true);

        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height),0,0);

        screenTexture.Apply();

        byte[] dataToSave = screenTexture.EncodeToPNG();

        string destination = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

        File.WriteAllBytes(destination, dataToSave);

#if UNITY_ANDROID
        if(!Application.isEditor)
        {
        // block to open the file and share it ————START
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://" + destination);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "testo");
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
        intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "testo");
        //intentObject.Call<AndroidJavaObject>("setType", "text/html");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        // option one:
        //currentActivity.Call("startActivity", intentObject);

        // option two:
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Let people to know by :");
        currentActivity.Call("startActivity", jChooser);

        // block to open the file and share it ————END
        }
#endif
        isProcessing = false;
    }
}