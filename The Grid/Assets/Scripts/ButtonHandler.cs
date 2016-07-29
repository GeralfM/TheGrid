using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}

	public Image myImage { get; set;}
	public float stockedTime { get; set;}

	// Use this for initialization
	void Start () {
		myImage = gameObject.GetComponent<Image> ();
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
	}

	public void SetSelected(bool itIs){
		if (itIs)
			myImage.color = new Color (1, 1, 1, 1);
		else
			myImage.color = new Color (1, 1, 1, 0.5f);
	}

	public void HandleTime(){
		if (gameObject.name == "ButtonBackward")
			Time.timeScale = Time.timeScale / 2f;
		else if (gameObject.name == "ButtonForward")
			Time.timeScale = Time.timeScale * 2f;
		else if (gameObject.name == "ButtonPlay") {
			if (Time.timeScale != 0) {
				stockedTime = Time.timeScale;
				Time.timeScale = 0;
				myImage.sprite = Resources.Load<Sprite> ("Sprites/Buttons/Play");
			} else {
				Time.timeScale = stockedTime;
				myImage.sprite = Resources.Load<Sprite> ("Sprites/Buttons/Pause");
			}
		}
		myHandler.PrintTime ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
