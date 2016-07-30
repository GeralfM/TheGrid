using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}
	public CursorHandler myCursor { get; set;}

	public Image myImage { get; set;}
	public float stockedTime { get; set;}

	// Use this for initialization
	void Start () {
		myImage = gameObject.GetComponent<Image> ();
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
		myCursor = GameObject.Find ("MainHandler").GetComponent<CursorHandler> ();
	}

	public void SetSelected(bool itIs){
		if (itIs)
			myImage.color = new Color (1, 1, 1, 1);
		else
			myImage.color = new Color (1, 1, 1, 0.5f);
	}

	public void ChoiceTimeMethod(){
		if (myCursor.cursorState == "Select")
			HandleObjectTime (myCursor.allSelected);
		else
			HandleTime ();
	}

	public void HandleObjectTime(List<CaseHandler> goals){
		foreach (CaseHandler aCase in goals) {
			if (gameObject.name == "ButtonBackward")
				aCase.timeM = aCase.timeM * 2f;
			else if (gameObject.name == "ButtonForward")
				aCase.timeM = aCase.timeM / 2f;
			else if (gameObject.name == "ButtonPlay") {
				if (aCase.timeStocked == 0f) {
					aCase.timeStocked = aCase.timeM;
					aCase.timeM = Mathf.Infinity;
				} else {
					aCase.timeM = aCase.timeStocked;
					aCase.timeStocked = 0f;
				}
			}
		}
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
