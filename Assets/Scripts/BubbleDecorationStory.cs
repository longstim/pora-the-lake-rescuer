using UnityEngine;
using System.Collections;

public class BubbleDecorationStory : Singleton<BubbleDecorationStory> {

    public GameObject BubblePrefab;
    public Transform BatasKiri;
    public Transform BatasKanan;
    public Transform BatasAtas;
    public Transform BatasPecah;
	public float ScaleBubble1;
	public float ScaleBubble2;

    void Start()
    {
        bubbleUpInvoke();
    }

    void bubbleUp()
    {
        float rangePositionX = Random.Range(BatasKiri.localPosition.x, BatasKanan.localPosition.x);
        GameObject actualBubbleUp = GameObject.Instantiate(BubblePrefab, new Vector2(rangePositionX, BubblePrefab.transform.localPosition.y), Quaternion.identity) as GameObject;
        actualBubbleUp.transform.SetParent(transform, false);
		float rangeScale = Random.Range(ScaleBubble1, ScaleBubble2);
        actualBubbleUp.transform.localScale = new Vector3(rangeScale, rangeScale, 0);

        bubbleUpInvoke();
    }

    void bubbleUpInvoke()
    {
        if (!IsInvoking("bubbleUp"))
        {
            Invoke("bubbleUp", Random.Range(0.025f, 1.5f));
        }
    }
}
