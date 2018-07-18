using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotationScrollbar : MonoBehaviour {

    private static GameObject willChange;

    private static GameObject selected;
    public static GameObject Selected {
        set { 
            selected = value;
            willChange = null;
            if (selected != null)
            {
                float temp = 0;
                if (selected.tag == "Rubbish")
                {
                    RubbishController rubbish = selected.GetComponent<RubbishController>();
                    temp = (rubbish.Rotation / rubbish.MAX_ROTATION) * 0.45f;
                    Debug.Log(rubbish.Rotation);
                }
                else if (selected.tag == "Barrier")
                {
                    BarrierController barrier = selected.GetComponent<BarrierController>();
                    temp = (barrier.Rotation / barrier.MAX_ROTATION) * 0.45f;
                    Debug.Log(barrier.Rotation);
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
        get { return selected;}
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
                    rubbish.Rotation = 0;
                }
                else if (willChange.tag == "Barrier")
                {
                    BarrierController barrier = willChange.GetComponent<BarrierController>();
                    barrier.Rotation = 0;
                }
            }
            else if (scrollBar.value > 0.55)
            {
                if (willChange.tag == "Rubbish")
                {
                    RubbishController rubbish = willChange.GetComponent<RubbishController>();
                    rubbish.Rotation = (scrollBar.value - 0.55f) / 0.45f * rubbish.MAX_ROTATION;
                }
                else if (willChange.tag == "Barrier")
                {
                    BarrierController barrier = willChange.GetComponent<BarrierController>();
                    barrier.Rotation = (scrollBar.value - 0.55f) / 0.45f * barrier.MAX_ROTATION;
                }
            }
            else if (scrollBar.value < 0.45)
            {
                if (willChange.tag == "Rubbish")
                {
                    RubbishController rubbish = willChange.GetComponent<RubbishController>();
                    rubbish.Rotation = (scrollBar.value - 0.45f) / 0.45f * rubbish.MAX_ROTATION;
                }
                else if (willChange.tag == "Barrier")
                {
                    BarrierController barrier = willChange.GetComponent<BarrierController>();
                    barrier.Rotation = (scrollBar.value - 0.45f) / 0.45f * barrier.MAX_ROTATION;
                }

            }
        }
    }
}
