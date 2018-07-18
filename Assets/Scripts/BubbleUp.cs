using UnityEngine;
using System.Collections;

public class BubbleUpScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		destroyBubbleUp ();
		bubblePecah ();
	}

	void destroyBubbleUp()
	{
		if (this.transform.localPosition.y  > Singleton<BubbleDecorationStoryScene>.Instance.BatasAtas.transform.localPosition.y)
		{
			Destroy(gameObject);
		}
	}
	void bubblePecah()
	{
		if (this.transform.localPosition.y  > Singleton<BubbleDecorationStoryScene>.Instance.BatasPecah.transform.localPosition.y)
		{
			GetComponent<Animator>().SetTrigger("Pecah");
		}
	}
}
