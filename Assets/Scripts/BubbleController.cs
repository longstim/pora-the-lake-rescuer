using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BubbleController : MonoBehaviour {

    public enum BubbleSize
    {
        One,
        Two,
        Three,
        Four,
        Five
    }
    public enum BubbleType
    {
        White,
        Red,
        Orange
    }
    private class BubbleSpeed
    {
        public Vector2 One = new Vector2(15500, 0);
        public Vector2 Two = new Vector2(12500, 0);
        public Vector2 Three = new Vector2(10000, 0);
        public Vector2 Four = new Vector2(7500, 0);
        public Vector2 Five = new Vector2(4500, 0);
    }
    private class BubbleScale
    {
        public Vector3 One = new Vector3(0.45f, 0.45f, 1);
        public Vector3 Two = new Vector3(0.55f, 0.55f, 1);
        public Vector3 Three = new Vector3(0.7f, 0.7f, 1);
        public Vector3 Four = new Vector3(0.85f, 0.85f, 1);
        public Vector3 Five = new Vector3(1, 1, 1);
    }
    
    public BubbleType Type;
    public BubbleSize Size;
    public int SizeInInt
    {
        set
        {
            switch (value)
            { 
                case 1: Size = BubbleSize.One; break;
                case 2: Size = BubbleSize.Two; break;
                case 3: Size = BubbleSize.Three; break;
                case 4: Size = BubbleSize.Four; break;
                case 5: Size = BubbleSize.Five; break;
            }
            RefreshScale();
        }
        get
        {
            switch (Size)
            {
                case BubbleSize.One: return 1;
                case BubbleSize.Two: return 2;
                case BubbleSize.Three: return 3;
                case BubbleSize.Four: return 4;
                default: return 5;
            }

        }
    }

    private BubbleScale Scale = new BubbleScale();
    private BubbleSpeed Speed = new BubbleSpeed();
    private bool isNewBubble;

    [HideInInspector]
    public List<RubbishController> rubbishes = new List<RubbishController>();
    [HideInInspector]
    public List<RubbishController> thrownRubbishes = new List<RubbishController>();
    [HideInInspector]
    public List<DistanceJoint2D> distanceJoints = new List<DistanceJoint2D>();
    
    public CircleCollider2D detectorCollider;
    public CircleCollider2D realCollider;

    public GameObject whiteBubbleDecoration;
    public GameObject redBubbleDecoration;
    public GameObject orangeBubbleDecoration;

    public bool HaveBeenBouncing = false;
    private bool HaveBeenLiftRubbish = false;

    public static int UseBubbleScore = -100;
    public static int MultiliftScore = 250;
    public static int SkillScore = 150;
    public static int RedSkillScore = 50;

	// Use this for initialization
	void Start () {
        isNewBubble = true;
        PlayManager.Score += UseBubbleScore;
        PlayManager.BubbleUsedScore += UseBubbleScore;
        switch (Size)
        {
            case BubbleSize.One: rigidbody2D.AddForce(Speed.One); break;
            case BubbleSize.Two: rigidbody2D.AddForce(Speed.Two); break;
            case BubbleSize.Three: rigidbody2D.AddForce(Speed.Three); break;
            case BubbleSize.Four: rigidbody2D.AddForce(Speed.Four); break;
            case BubbleSize.Five: rigidbody2D.AddForce(Speed.Five); break;
        }
        switch (Type)
        {
            case BubbleType.White: gameObject.AddComponent<WhiteBubbleContoller>(); break;
            case BubbleType.Red: gameObject.AddComponent<RedBubbleController>(); break;
            case BubbleType.Orange: gameObject.AddComponent<OrangeBubbleController>(); break;

        }
        RefreshScale();
        rigidbody2D.gravityScale = -5;
	}

    private void RefreshScale()
    {
        switch (Size)
        {
            case BubbleSize.One: transform.localScale = Scale.One; break;
            case BubbleSize.Two: transform.localScale = Scale.Two; break;
            case BubbleSize.Three: transform.localScale = Scale.Three; break;
            case BubbleSize.Four: transform.localScale = Scale.Four; break;
            case BubbleSize.Five: transform.localScale = Scale.Five; break;

        }
    }

    public void Init(BubbleType type, BubbleSize size, Vector3 position)
    {
        this.Type = type;
        this.Size = size;
        transform.position = position;
    }
	
	// Update is called once per frame
	void Update () {
        CheckToDestroy();
	}

    public bool isSticky = false;
    public bool SudahBerhenti = false;
    void LateUpdate(){
        int divisor = 20;
        int gravityScale = -70;

        if (!isNewResume && PlayManager.State == PlayManager.GameplayState.Playing)
        {
            if (isSticky)
            {

            }
            else if (isNewBubble && rigidbody2D.velocity.x >= 0)
            {
                isNewBubble = false;
            }
            else if (Size == BubbleSize.One && rigidbody2D.velocity.x < Speed.One.x / divisor)
            {
                rigidbody2D.gravityScale = gravityScale;
            }
            else if (Size == BubbleSize.Two && rigidbody2D.velocity.x < Speed.Two.x / divisor)
            {
                rigidbody2D.gravityScale = gravityScale;
            }
            else if (Size == BubbleSize.Three && rigidbody2D.velocity.x < Speed.Three.x / divisor)
            {
                rigidbody2D.gravityScale = gravityScale;
            }
            else if (Size == BubbleSize.Four && rigidbody2D.velocity.x < Speed.Four.x / divisor)
            {
                rigidbody2D.gravityScale = gravityScale;
            }
            else if (Size == BubbleSize.Five && rigidbody2D.velocity.x < Speed.Five.x / divisor)
            {
                rigidbody2D.gravityScale = gravityScale;
            }

            if (rigidbody2D.velocity.x < 200 && !SudahBerhenti && !isNewBubble)
            {
                rigidbody2D.velocity = new Vector3(0, rigidbody2D.velocity.y);
                SudahBerhenti = true;
            }
        }
        else if (!isNewPause && PlayManager.State != PlayManager.GameplayState.Playing)
        {
            isNewPause = true;
            isNewResume = true;
            lastestSpeed = rigidbody2D.velocity;
            lastesGravity = rigidbody2D.gravityScale;
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.gravityScale = 0;
        }
        else if (isNewResume && PlayManager.State == PlayManager.GameplayState.Playing)
        {
            rigidbody2D.velocity = lastestSpeed;
            rigidbody2D.gravityScale = lastesGravity;
            isNewPause = false;
            isNewResume = false;
        }
    }

    bool isNewPause = false;
    bool isNewResume = false;
    Vector2 lastestSpeed;
    float lastesGravity;

    private int SizeOfAllRubbishes()
    {
        int totalSize = 0;

        foreach (RubbishController RubbishController in rubbishes)
            totalSize += RubbishController.SizeInInt;

        return totalSize;
    }

    private int SizeOfFirstRubbish()
    {
        if (rubbishes.Count > 0)
            return rubbishes[0].SizeInInt;
        return 0;
    }

    public bool CanLift(RubbishController RubbishController)
    {
        return SizeInInt >= this.SizeOfAllRubbishes() + RubbishController.SizeInInt;
    }

    public bool CanLiftWithThrowFirst(RubbishController RubbishController)
    {
        return SizeInInt >= this.SizeOfAllRubbishes() + RubbishController.SizeInInt - this.SizeOfFirstRubbish();
    }

    public void ThrowAllRubbishes()
    {
        foreach (RubbishController RubbishController in rubbishes)
        {
            Destroy(RubbishController.GetComponent<DistanceJoint2D>());
            Physics2D.IgnoreCollision(realCollider, RubbishController.collider2D);
            Physics2D.IgnoreCollision(detectorCollider, RubbishController.collider2D);
            thrownRubbishes.Add(RubbishController);
        }
        rubbishes.Clear();

        foreach (DistanceJoint2D distanceJoint in distanceJoints)
        {
            Destroy(distanceJoint);
        }
        distanceJoints.Clear();
    }

    public void ThrowFirstRubbish()
    {
        if (rubbishes.Count > 0)
        {
            Destroy(rubbishes[0].GetComponent<DistanceJoint2D>());
            Physics2D.IgnoreCollision(realCollider, rubbishes[0].collider2D);
            Physics2D.IgnoreCollision(detectorCollider, rubbishes[0].collider2D);
            thrownRubbishes.Add(rubbishes[0]);
            rubbishes.RemoveAt(0);

            Destroy(distanceJoints[0]);
            distanceJoints.RemoveAt(0);
        }
    }

    public void ThrowRubbish(RubbishController RubbishController)
    {
        int index = rubbishes.IndexOf(RubbishController);
        if (index != -1)
        {
            Destroy(rubbishes[index].GetComponent<DistanceJoint2D>());
            thrownRubbishes.Add(rubbishes[index]);
            rubbishes.RemoveAt(index);
            Destroy(distanceJoints[index]);
            distanceJoints.RemoveAt(index);
        }
    }

    public void LiftRubbish(RubbishController rubbishController)
    {
        if (rubbishController.IsLifted())
        {
            rubbishController.ThrowFromAnyBubble();
            PlayManager.Score += BubbleController.SkillScore;
            PlayManager.SkillScore += BubbleController.SkillScore;
        }

        DistanceJoint2D djSampah = rubbishController.gameObject.AddComponent<DistanceJoint2D>();
        djSampah.connectedBody = rigidbody2D;
        djSampah.distance = 0f;
        djSampah.collideConnected = false;
        djSampah.maxDistanceOnly = true;
        rubbishes.Add(rubbishController);
        

        DistanceJoint2D djGelembung = gameObject.AddComponent<DistanceJoint2D>();
        djGelembung.connectedBody = rubbishController.rigidbody2D;
        djGelembung.distance = 0f;
        djGelembung.collideConnected = false;
        djGelembung.maxDistanceOnly = true;
        distanceJoints.Add(djGelembung);

        if (HaveBeenBouncing && !HaveBeenLiftRubbish)
        {
            PlayManager.Score += BubbleController.RedSkillScore;
            PlayManager.SkillScore += BubbleController.RedSkillScore;
            HaveBeenLiftRubbish = true;
        }
    }

    public void BrokenOut()
    {
        transform.parent.GetComponent<AudioSource>().Play();
        ThrowAllRubbishes();
        Destroy(gameObject);
        int jumlah = Random.Range(1*SizeInInt,SizeInInt*2);
        int rangePosition = (12 * SizeInInt);
        if (Type == BubbleType.White)
        {
            for (int i = 0; i < jumlah; i++)
            {
                GameObject bubbleDecoration=(GameObject)Instantiate(whiteBubbleDecoration);
                bubbleDecoration.transform.position = new Vector3(Random.Range(transform.position.x - rangePosition, transform.position.x + rangePosition), Random.Range(transform.position.y - rangePosition, transform.position.y + rangePosition), transform.position.z);
                bubbleDecoration.transform.SetParent(transform.parent.parent);
            }
        }
        else if (Type == BubbleType.Red)
        {
            for (int i = 0; i < jumlah; i++)
            {
                GameObject bubbleDecoration = (GameObject)Instantiate(redBubbleDecoration);
                bubbleDecoration.transform.position = new Vector3(Random.Range(transform.position.x - rangePosition, transform.position.x + rangePosition), Random.Range(transform.position.y - rangePosition, transform.position.y + rangePosition), transform.position.z);
                bubbleDecoration.transform.SetParent(transform.parent.parent);
            }
        }
        else if (Type == BubbleType.Orange)
        {
            for (int i = 0; i < jumlah; i++)
            {
                GameObject bubbleDecoration = (GameObject)Instantiate(orangeBubbleDecoration);
                bubbleDecoration.transform.position = new Vector3(Random.Range(transform.position.x - rangePosition, transform.position.x + rangePosition), Random.Range(transform.position.y - rangePosition, transform.position.y + rangePosition), transform.position.z);
                bubbleDecoration.transform.SetParent(transform.parent.parent);
            }
        }
    }

    public bool IsStop()
    {
        return (rigidbody2D.velocity.x < 10 && rigidbody2D.velocity.y < 10 && !isNewBubble);
    }

    void CheckToDestroy()
    {
        if (transform.position.y > 1119)
        {
            DistanceJoint2D[] distanceJoints = GetComponents<DistanceJoint2D>();
            if (distanceJoints.Length > 1)
            {
                PlayManager.Score += ((distanceJoints.Length - 1) * BubbleController.MultiliftScore);
                PlayManager.MultiLiftScore += ((distanceJoints.Length - 1) * BubbleController.MultiliftScore);
            }
            for (int i = 0; i < distanceJoints.Length; i++)
            {
                if (distanceJoints[i].connectedBody)
                    Destroy(distanceJoints[i].connectedBody.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
