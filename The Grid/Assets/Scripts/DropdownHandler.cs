using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DropdownHandler : MonoBehaviour {

	public GeneralHandler myHandler{ get; set;}
	public CursorHandler myCursor{ get; set;}

	public int selection{ get; set;}

	// Use this for initialization
	void Start () {
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
		myCursor = GameObject.Find ("MainHandler").GetComponent<CursorHandler> ();
		selection = 0;
	}

	public void SetSelection(){
		selection = gameObject.GetComponent<Dropdown> ().value;
		myHandler.infoLocked = false;
		switch (selection) {
		case 0:
			myCursor.cursorState="none";
			break;
		case 1:
			myHandler.PrintInfos ("Wind Turbine\nCost : 5 energy", "Slowly recharges energy");
			myCursor.secondaryState="Wind_Turbine";
			break;
		default:
			break;
		}
		if (selection != 0) {
			myHandler.infoLocked = true;
			myCursor.cursorState = "Building";
		} else {
			myCursor.cursorState = "none";
			myCursor.secondaryState="none";
		}
			
		GoDisable();
	}

	public void GoDisable(){
		Invoke ("Disable", 0.2f);
	}
	public void Disable(){
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
