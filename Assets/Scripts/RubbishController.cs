using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RubbishController : MonoBehaviour {

    public static int RubbishScore = 500;

    public enum RubbishSize { 
        One,
        Two,
        Three,
        Four,
        Five
    }
    [HideInInspector]
    public bool IsEditMode = false;
    public bool isSelected = false;
    private bool isShowRotationSpeed = false;

    public string PackageName;
    public RubbishSize Size;
    public int SizeInInt
    {
        set
        {
            switch (value)
            {
                case 1: Size = RubbishSize.One; break;
                case 2: Size = RubbishSize.Two; break;
                case 3: Size = RubbishSize.Three; break;
                case 4: Size = RubbishSize.Four; break;
                case 5: Size = RubbishSize.Five; break;
            }
        }
        get
        {
            switch (Size)
            {
                case RubbishSize.One: return 1;
                case RubbishSize.Two: return 2;
                case RubbishSize.Three: return 3;
                case RubbishSize.Four: return 4;
                default: return 5;
            }

        }
    }
    public GameObject selectedEffect;
    public float MAX_ROTATION_SPEED = 10;
    public float MAX_ROTATION = 180;

    private float rotation = 0;
    [HideInInspector]
    public float Rotation
    {
        get
        {
            return rotation;
        }
        set
        {
            rotation = value;
            isShowRotationSpeed = false;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, value));
        }
    }
    private float rotationSpeed = 0;
    [HideInInspector]
    public float RotationSpeed
    {
        get
        {
            return rotationSpeed;
        }
        set
        {
            rotationSpeed = value;
            if (rotationSpeed == 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, rotation));
                isShowRotationSpeed = false;
            }
            else
            {
                isShowRotationSpeed = true;
            }
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        selectedEffect.SetActive(isSelected);
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPoint.y > Screen.height && (Application.loadedLevelName == "PlayParbaba" || Application.loadedLevelName == "PlayBatuguru" || Application.loadedLevelName == "PlayParapat" || Application.loadedLevelName == "PlayTomok"))
        {
            Invoke("DestroyMe", 1.2f);
        }
	}

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (rotationSpeed != 0 && ((isSelected && isShowRotationSpeed) || (!IsEditMode && PlayManager.State == PlayManager.GameplayState.Playing)))
        {
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed));
        }

        if (!IsEditMode)
        {
            if (this.IsLifted())
            {
                collider2D.isTrigger = true;
                //rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.mass = 0f;
                if (GetComponent<DistanceJoint2D>() && GetComponent<DistanceJoint2D>().connectedBody)
                {
                    transform.position = GetComponent<DistanceJoint2D>().connectedBody.transform.position;
                }
                //float time = 0.1f;
                //transform.position = new Vector3(Mathf.Lerp(transform.position.x, GetComponent<DistanceJoint2D>().connectedBody.position.x, time), Mathf.Lerp(transform.position.y, GetComponent<DistanceJoint2D>().connectedBody.position.y, time), transform.position.z);
            }
            else
            {
                rigidbody2D.velocity = Vector2.zero;
                collider2D.isTrigger = false;
                rigidbody2D.mass = 1000;
            }
        }
    }

    public bool IsLifted()
    {
        return GetComponent<DistanceJoint2D>() != null;
    }

    public void ThrowFromAnyBubble()
    {
        if (IsLifted())
        {
            DistanceJoint2D dj = GetComponent<DistanceJoint2D>();
            if (dj.connectedBody)
            {
                if (dj.connectedBody.GetComponent<BubbleController>())
                {
                    dj.connectedBody.GetComponent<BubbleController>().ThrowRubbish(this);
                }
            }
        }
    }

    public void Select()
    {
        GameObject[] rubbishes = GameObject.FindGameObjectsWithTag("Rubbish");
        for (int i = 0; i < rubbishes.Length; i++)
        {
            rubbishes[i].GetComponent<RubbishController>().Unselect();
        }
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        for (int i = 0; i < barriers.Length; i++)
        {
            barriers[i].GetComponent<BarrierController>().Unselect();
        }
        isSelected = true;
        isShowRotationSpeed = true;
        RotationSpeedSrollbar.Selected = gameObject;
        RotationScrollbar.Selected = gameObject;
    }

    public void Unselect()
    {
        if(isSelected)
        {
            RotationSpeedSrollbar.Selected = null;
            RotationScrollbar.Selected = null;
        }
        isSelected = false;
        isShowRotationSpeed = false;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, rotation));
    }

    void OnDestroy()
    {
        PlayManager.Score += RubbishController.RubbishScore;
        PlayManager.RubbishLiftScore += RubbishController.RubbishScore;
    }
}
