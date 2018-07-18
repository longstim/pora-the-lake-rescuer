using UnityEngine;
using System.Collections;


public class MainSceneLoader : MonoBehaviour
{
    public AsyncOperation asynOperation = null;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("LoadScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadScene()
    {
        asynOperation = Application.LoadLevelAsync(SceneName.Main.ToString());
        asynOperation.allowSceneActivation = false;
        yield return asynOperation;
    }

    public void ActivateScene()
    {
        asynOperation.allowSceneActivation = true;
    }
}
