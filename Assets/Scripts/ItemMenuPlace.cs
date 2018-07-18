using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemMenuPlace : MonoBehaviour {

	[HideInInspector]
	public bool OnFocus = false;

	public float timeScale = 0.1f;
	public Vector3 BasicScale = new Vector3(0.7f,0.7f,0.7f);
	public Vector3 FocusScale = new Vector3(1,1,1);


    public string Place;
	public Image background; 
	public Sprite SpriteBackground;
    public Text textStar;

    public GameObject StarArea;
    public ChoosePlaceManager choosePlaceManager;

	// Use this for initialization
	void Start () {
		transform.localScale = BasicScale;
        textStar.text = UserScoreData.Load().GetPlaceStar(Place).ToString();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currentPosition = transform.position;
		//Debug.Log (touchPosition);

		Collider2D collider = Physics2D.OverlapPoint(new Vector2(currentPosition.x,currentPosition.y));

		if (collider && collider.name == "FocusDetector") {
			OnFocus = true;
			background.sprite = SpriteBackground;
            choosePlaceManager.SelectedChange(Place);
		} else {
			OnFocus = false;
		}
		if (OnFocus) {
			transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, FocusScale.x, timeScale), Mathf.Lerp (transform.localScale.y, FocusScale.y, timeScale), Mathf.Lerp (transform.localScale.z, FocusScale.z, timeScale));
            StarArea.SetActive(true);
            choosePlaceManager.SelectedChange(Place);
		} else {
			transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, BasicScale.x, timeScale), Mathf.Lerp (transform.localScale.y, BasicScale.y, timeScale), Mathf.Lerp (transform.localScale.z, BasicScale.z, timeScale));
            StarArea.SetActive(false);
		}
	}
}
