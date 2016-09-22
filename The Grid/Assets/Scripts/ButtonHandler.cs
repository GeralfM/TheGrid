﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}
	public CursorHandler myCursor { get; set;}

	public GameObject mypopup;

	public Image myImage { get; set;}
	public float stockedTime { get; set;}

	public bool selected { get; set;}

	// Use this for initialization
	void Start () {
		selected = false;
		myImage = gameObject.GetComponent<Image> ();
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
		myCursor = GameObject.Find ("MainHandler").GetComponent<CursorHandler> ();
	}

	public void SetSelected(){
		selected = !selected;
		SetSelected (selected);
	}
	public void SetSelected(bool itIs){
		if (itIs)
			myImage.color = new Color (1, 1, 1, 1);
		else {
			myImage.color = new Color (1, 1, 1, 0.5f);
			if (gameObject.name == "ButtonBuildings") {
				mypopup.SetActive (false);
				myHandler.infoLocked = false;
			}
		}
		selected = itIs;
	}

	public void ChoiceTimeMethod(){
		bool anySelect = false;
		foreach (CaseHandler aCase in myHandler.myCases.Values)
			if (aCase.specialProperties ["Selected"])
				anySelect = true;
		
		if (anySelect)
			HandleObjectTime (myCursor.allSelected);
		else
			HandleTime ();
	}

	public void HandleObjectTime(List<CaseHandler> goals){
		foreach (CaseHandler aCase in goals) {
			if (gameObject.name == "ButtonBackward" && aCase.timeM <= 100f)
				aCase.timeM = aCase.timeM * 2f;
			else if (gameObject.name == "ButtonForward" )
				aCase.timeM = aCase.timeM / 2f;
			else if (gameObject.name == "ButtonPlay") {
				if (aCase.specialProperties ["Paused"])
					aCase.specialProperties ["Paused"] = false;
				else
					aCase.specialProperties ["Paused"] = true;
			}
			aCase.SynchroParams ();
		}
	}

	public void HandleTime(){
		if (gameObject.name == "ButtonBackward" && Time.timeScale >= 0.01f)
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
