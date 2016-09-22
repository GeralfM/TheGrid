using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsHandler : MonoBehaviour {

	public GeneralHandler myHandler{ get; set;}
	public CursorHandler myCursor{ get; set;}

	public Dictionary<string,List<string>> neededToBuild = new Dictionary<string, List<string>> ();

	// Use this for initialization
	void Start () {
		myHandler = gameObject.GetComponent<GeneralHandler> ();
		myCursor = gameObject.GetComponent<CursorHandler> ();

		neededToBuild.Add ("Wind_Turbine", new List<string>{ "Dirt", "Stone", "Diamond" });
	}

	public bool MayConstruct(string typeCase){
		switch (myCursor.secondaryState) {
		case "Wind_Turbine":
			return(myHandler.energy >= 5 && neededToBuild[myCursor.secondaryState].Contains(typeCase));
		default:
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
