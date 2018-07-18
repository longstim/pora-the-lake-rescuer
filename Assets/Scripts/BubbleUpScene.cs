using UnityEngine;
using System.Collections;

public class BubbleUp : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		destroyBubbleUp ();
		bubblePecah ();
	}

	void destroyBubbleUp()
	{
		if (this.transform.localPosition.y  > Singleton<BubbleDecorationStory>.Instance.BatasAtas.transform.localPosition.y)
		{
			Destroy(gameObject);
		}
	}
	void bubblePecah()
	{
		if (this.transform.localPosition.y  > Singleton<BubbleDecorationStory>.Instance.BatasPecah.transform.localPosition.y)
		{
			GetComponent<Animator>().SetTrigger("Pecah");
		}
	}
}
