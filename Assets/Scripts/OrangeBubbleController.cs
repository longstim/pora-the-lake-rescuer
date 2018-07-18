using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrangeBubbleController : MonoBehaviour
{

    BubbleController bubble;
    private DistanceJoint2D orangeJoint;
    private float AfterMergingSpeed = 1.1f;

    // Use this for initialization
    void Start()
    {
        bubble = GetComponent<BubbleController>();
    }

    // Update is called once per frame
    bool stop = false;
    void LateUpdate()
    {
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
        else if (coll.gameObject.GetComponent<BarrierController>())
        {
            Stop();
        }
        else if (coll.gameObject.GetComponent<RubbishController>())
        {
            RubbishController sampahController = coll.gameObject.GetComponent<RubbishController>();
            if (bubble.CanLift(sampahController) && !bubble.thrownRubbishes.Contains(sampahController) && !bubble.rubbishes.Contains(sampahController))
            {
                bubble.LiftRubbish(sampahController);
            }
            else if (bubble.CanLiftWithThrowFirst(sampahController) && !bubble.thrownRubbishes.Contains(sampahController) && !bubble.rubbishes.Contains(sampahController))
            {
                bubble.ThrowFirstRubbish();
                bubble.LiftRubbish(sampahController);
            }
            else if (!bubble.rubbishes.Contains(sampahController))
            {
                this.StickyOnRubbish(coll.gameObject);
            }
        }
        else if (coll.gameObject.GetComponent<OrangeBubbleController>())
        {
            this.MergeBubble(coll.gameObject.GetComponent<BubbleController>());
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<OrangeBubbleController>())
        {
            this.MergeBubble(coll.gameObject.GetComponent<BubbleController>());
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (GetComponent<DistanceJoint2D>() && !(GetComponent<DistanceJoint2D>().connectedBody.GetComponent<DistanceJoint2D>()))
        {
            Destroy(GetComponent<DistanceJoint2D>());
            rigidbody2D.gravityScale = -70;
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

    private void StickyOnRubbish(GameObject sampah)
    {
        orangeJoint = gameObject.AddComponent<DistanceJoint2D>();
        orangeJoint.connectedBody = sampah.rigidbody2D;
        orangeJoint.collideConnected = true;
        rigidbody2D.gravityScale = 0;
        bubble.isSticky = true;
        this.Stop();
    }

    private void MergeBubble(BubbleController gelembungController)
    {

        if (Mathf.Sqrt(Mathf.Pow(rigidbody2D.velocity.x, 2) + Mathf.Pow(rigidbody2D.velocity.y, 2)) > Mathf.Sqrt(Mathf.Pow(gelembungController.rigidbody2D.velocity.x, 2) + Mathf.Pow(gelembungController.rigidbody2D.velocity.y, 2)))
        {
            int newSize = bubble.SizeInInt + gelembungController.SizeInInt;
            if (newSize > 5)
                newSize = 5;
            bubble.SizeInInt = newSize;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * AfterMergingSpeed, rigidbody2D.velocity.y * AfterMergingSpeed);

            PlayManager.Score += BubbleController.SkillScore;
            PlayManager.SkillScore += BubbleController.SkillScore;
        }
        else if (Mathf.Sqrt(Mathf.Pow(rigidbody2D.velocity.x, 2) + Mathf.Pow(rigidbody2D.velocity.y, 2)) == Mathf.Sqrt(Mathf.Pow(gelembungController.rigidbody2D.velocity.x, 2) + Mathf.Pow(gelembungController.rigidbody2D.velocity.y, 2)) && transform.position.x < gelembungController.transform.position.x)
        {
            int newSize = bubble.SizeInInt + gelembungController.SizeInInt;
            if (newSize > 5)
                newSize = 5;
            bubble.SizeInInt = newSize;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * AfterMergingSpeed, rigidbody2D.velocity.y * AfterMergingSpeed);

            PlayManager.Score += BubbleController.SkillScore;
            PlayManager.SkillScore += BubbleController.SkillScore;
        }
        else{
            DestroyOnlyBubble();
        }
    }

    public void DestroyOnlyBubble()
    {
        Destroy(orangeJoint);
        bubble.ThrowAllRubbishes();
        Destroy(gameObject);
    }
}
