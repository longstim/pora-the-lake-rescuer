using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotationSpeedSrollbar : MonoBehaviour {

    private static GameObject willChange;
    private static GameObject selected;
    public static GameObject Selected
    {
        set
        {
            selected = value;
            willChange = null;
            if (value != null)
            {
                float temp = 0;
                if (Selected.tag == "Rubbish")
                {
                    RubbishController rubbish = Selected.GetComponent<RubbishController>();
                    temp = (rubbish.RotationSpeed / rubbish.MAX_ROTATION_SPEED) * 0.45f;
                }
                else if (Selected.tag == "Barrier")
                {
                    BarrierController barrier = Selected.GetComponent<BarrierController>();
                    temp = (barrier.RotationSpeed / barrier.MAX_ROTATION_SPEED) * 0.45f;
                }
                if (temp > 0)
                {
                    scrollBar.value = temp + 0.55f;
                }
                else if (temp < 0)
                {
                    scrollBar.value = 0.45f + temp;
                }
                else
                {
                    scrollBar.value = 0.5f;
                }
                willChange = selected;
            }
        }
        get { return selected; }
    }

    public static Scrollbar scrollBar;
    // Use this for initialization
    void Start()
    {
        scrollBar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(!(Selected == null));
    }

    public void OnChange(float value)
    {
        if (willChange != null)
        {
            if (scrollBar.value <= 0.55 && scrollBar.value >= 0.45)
            {
                if (willChange.tag == "Rubbish")
                {
                    RubbishController rubbish = willChange.GetComponent<RubbishController>();
                    rubbish.RotationSpeed = 0;
                }
                else if (Selected.tag == "Barrier")
                {
                    BarrierController barrier = willChange.GetComponent<BarrierController>();
                    barrier.RotationSpeed = 0;
                }
            }
            else if (scrollBar.value > 0.55)
            {
                if (willChange.tag == "Rubbish")
                {
                    RubbishController rubbish = willChange.GetComponent<RubbishController>();
                    rubbish.RotationSpeed = (scrollBar.value - 0.55f) / 0.45f * rubbish.MAX_ROTATION_SPEED;
                }
                else if (willChange.tag == "Barrier")
                {
                    BarrierController barrier = willChange.GetComponent<BarrierController>();
                    barrier.RotationSpeed = (scrollBar.value - 0.55f) / 0.45f * barrier.MAX_ROTATION_SPEED;
                }
            }
            else if (scrollBar.value < 0.45)
            {
                if (willChange.tag == "Rubbish")
                {
                    RubbishController rubbish = willChange.GetComponent<RubbishController>();
                    rubbish.RotationSpeed = (scrollBar.value - 0.45f) / 0.45f * rubbish.MAX_ROTATION_SPEED;
                }
                else if (willChange.tag == "Barrier")
                {
                    BarrierController barrier = willChange.GetComponent<BarrierController>();
                    barrier.RotationSpeed = (scrollBar.value - 0.45f) / 0.45f * barrier.MAX_ROTATION_SPEED;
                }
            }
        }
    }
}
