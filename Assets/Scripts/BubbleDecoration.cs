using UnityEngine;
using System.Collections;

public class BubbleDecoration : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckToDestroy();
	}

    void CheckToDestroy()
    {
        if (transform.position.y > 1119)
        {
            Destroy(gameObject);
        }
    }
}
