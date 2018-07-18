using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BarrierController : MonoBehaviour {

    public string PackageName;
    public string Name;

    public bool IsEditMode = false;
    public bool isSelected = false;
    private bool isShowRotationSpeed = false;

    public bool isPreview = false;

    public GameObject selectedEffect;
    public bool isSpicky;
    [HideInInspector]
    public float MAX_ROTATION_SPEED = 10;
    [HideInInspector]
    public float MAX_ROTATION = 180;

    private float rotation = 90;
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
            transform.localRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, value));
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
        transform.localRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, rotation));
	}

    // Update is called once per frame
    void Update()
    {
        selectedEffect.SetActive(isSelected);
    }

    void FixedUpdate()
    {
        if (rotationSpeed != 0 && ((isSelected && isShowRotationSpeed) || (!IsEditMode && PlayManager.State == PlayManager.GameplayState.Playing) || isPreview))
        {
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed));
        }
    }

    public void Select()
    {
        RotationSpeedSrollbar.Selected = null;
        RotationScrollbar.Selected = null;
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
        RotationSpeedSrollbar.Selected = null;
        RotationScrollbar.Selected = null;
        isSelected = false;
        isShowRotationSpeed = false;
        transform.localRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, rotation));
    }
}
