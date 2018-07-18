using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedBubbleController : MonoBehaviour
{

    BubbleController bubble;
    // Use this for initialization
    void Start()
    {
        bubble = GetComponent<BubbleController>();
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        bubble.HaveBeenBouncing = true;
        bubble.SudahBerhenti = true;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<BarrierController>() && coll.gameObject.GetComponent<BarrierController>().isSpicky)
        {
            bubble.BrokenOut();
        }
        else if (coll.gameObject.GetComponent<RubbishController>())
        {
            RubbishController sampahController = coll.gameObject.GetComponent<RubbishController>();
            if (!sampahController.IsLifted() && bubble.CanLift(sampahController) && !bubble.thrownRubbishes.Contains(sampahController))
            {
                bubble.LiftRubbish(sampahController);
            }
            else if (!sampahController.IsLifted() && bubble.CanLiftWithThrowFirst(sampahController) && !bubble.thrownRubbishes.Contains(sampahController))
            {
                bubble.ThrowFirstRubbish();
                bubble.LiftRubbish(sampahController);
            }
        }
    }

}
