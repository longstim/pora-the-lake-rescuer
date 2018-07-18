using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhiteBubbleContoller : MonoBehaviour {

    BubbleController bubble;
	// Use this for initialization
	void Start () {
        bubble = GetComponent<BubbleController>();
	}
	
	// Update is called once per frame
    bool stop = false;
    
	void LateUpdate () {
        if (stop)
        {
            rigidbody2D.velocity = Vector3.zero;
            stop = false;
        }
	}


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<BarrierController>() && coll.gameObject.GetComponent<BarrierController>().isSpicky)
        {
            bubble.BrokenOut();
        }
        else if(coll.gameObject.GetComponent<BarrierController>())
        {
            Stop();
        }
        else if (coll.gameObject.GetComponent<RubbishController>())
        {
            RubbishController sampahController = coll.gameObject.GetComponent<RubbishController>();
            if (bubble.CanLift(sampahController) && !bubble.thrownRubbishes.Contains(sampahController))
            {
                bubble.LiftRubbish(sampahController);
            }
            else if (bubble.CanLiftWithThrowFirst(sampahController) && !bubble.thrownRubbishes.Contains(sampahController))
            {
                bubble.ThrowFirstRubbish();
                bubble.LiftRubbish(sampahController);
            }
            else {
                Stop();
                Physics2D.IgnoreCollision(bubble.realCollider, coll);
            }
        }
    }    

    private void Stop()
    {
        if (!bubble.SudahBerhenti)
        {
            stop = true;
            bubble.SudahBerhenti = true;
        }
    }
}
