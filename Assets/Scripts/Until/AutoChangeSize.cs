using UnityEngine;
using System.Collections;

public class AutoChangeSize : MonoBehaviour {
	
	public float normalAspect;
	public float NormalSize;
    Camera myCamera;
	void Start()
	{
		myCamera = GetComponent<Camera> ();
        myCamera.orthographicSize = NormalSize * ((1920 * myCamera.pixelHeight) / (1080 * myCamera.pixelWidth));        
	}

    void Update()
    {
        myCamera.orthographicSize = NormalSize * ((1920 * myCamera.pixelHeight) / (1080 * myCamera.pixelWidth));
    }
}
