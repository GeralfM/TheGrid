using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}

	public string cursorState { get; set;}
	public bool isLeft { get; set;}
	public string secondaryState { get; set;}

	public CaseHandler firstSelected { get; set;}
	public List<CaseHandler> interSelected = new List<CaseHandler> ();
	public List<CaseHandler> allSelected = new List<CaseHandler> ();

	public GameObject firstCaseSelected { get; set;}
	public Vector2 FC_anchorMax { get; set;}
	public Vector2 FC_anchorMin { get; set;}
	public int hor { get; set;}
	public int ver { get; set;}

	// Use this for initialization
	void Start () {
		myHandler = gameObject.GetComponent<GeneralHandler> ();
		cursorState = "none";
	}

	//=======================FOR THE SELECTION FEATURE=====================================

	public void SelectCases(GameObject aCase){
		if (cursorState == "Select") {
			if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightControl)) {
				foreach (CaseHandler old in allSelected) {
					old.specialProperties ["Selected"] = false;
					Destroy (old.gameObject.transform.Find ("Selected").gameObject);
				}
				allSelected = new List<CaseHandler> ();
			}
		
			firstSelected = aCase.GetComponent<CaseHandler> ();
			myHandler.NewAttribute (firstSelected.gameObject, "Selected");
			interSelected.Add (firstSelected);
		}
	}
	public void DragCases(bool isDragging){
		if (cursorState == "Select") {
			if (isDragging)
				secondaryState = "Drag";
			else {
				secondaryState = null;
				allSelected.AddRange (interSelected);
				interSelected = new List<CaseHandler> ();
			}
		}
	}
	public void WasJustAClick(){
		if (cursorState == "Select" && secondaryState == null) {
			allSelected.AddRange (interSelected);
			interSelected = new List<CaseHandler> ();
		}
	}
	public void RecalculateSelected(GameObject caseOver){
		if (cursorState == "Select" && secondaryState == "Drag") {

			int i1 = firstSelected.hor;int j1 = firstSelected.ver;
			int i2 = caseOver.GetComponent<CaseHandler>().hor; int j2 = caseOver.GetComponent<CaseHandler>().ver;
			int mini = Mathf.Min (i1, i2); int minj = Mathf.Min (j1, j2);
			int maxi = Mathf.Max (i1, i2); int maxj = Mathf.Max (j1, j2);

			foreach (CaseHandler aCase in myHandler.myCases.Values) {
				if (aCase.hor >= mini && aCase.hor <= maxi && aCase.ver >= minj && aCase.ver <= maxj) {
					if (!aCase.specialProperties ["Selected"]) {
						myHandler.NewAttribute (aCase.gameObject, "Selected");
						interSelected.Add (aCase);
					}
				}
				else if(aCase.specialProperties ["Selected"] && !allSelected.Contains(aCase)){
					aCase.specialProperties ["Selected"] = false;
					Destroy (aCase.gameObject.transform.Find ("Selected").gameObject);
					interSelected.Remove (aCase);
				}
			}
		}

	}

	//=======================FOR THE COPY==================================================

	public void CopyCases(GameObject goal){
		if (firstCaseSelected == null) {
			firstCaseSelected = goal;
			myHandler.NewAttribute (goal, "Selected");
		}
		else if(goal!=firstCaseSelected){

			for (int i = 0; i < goal.transform.childCount; i++)
				if (new List<string>{ "Cloud" }.Contains (goal.transform.GetChild (i).gameObject.name)) 
					Destroy (goal.transform.GetChild (i).gameObject);
			
			CaseHandler goalCase = goal.GetComponent<CaseHandler> ();
			CaseHandler firstCase = firstCaseSelected.GetComponent<CaseHandler> ();
			foreach (string carac in firstCase.caracs.Keys)
				goalCase.caracs [carac] = firstCase.caracs [carac];
			if (goalCase.type != firstCase.type)
				goalCase.myAnim.CrossFade (firstCase.type, 0f);		

			Destroy(firstCaseSelected.transform.Find("Selected").gameObject);

			for (int i = 0; i < firstCaseSelected.transform.childCount; i++)
				if (new List<string>{ "Cloud", "Grass" }.Contains (firstCaseSelected.transform.GetChild (i).gameObject.name))
					myHandler.NewAttribute (goal, firstCaseSelected.transform.GetChild (i).gameObject.name);

			firstCaseSelected.GetComponent<CaseHandler> ().specialProperties ["Selected"] = false;
			firstCaseSelected = null;
		} 
	}

	//=======================FOR THE SWITCH==================================================

	public void SwitchCases(GameObject goal){
		if (firstCaseSelected == null) {
			firstCaseSelected = goal;
			myHandler.NewAttribute (goal, "Selected");
			FC_anchorMax = goal.GetComponent<RectTransform> ().anchorMax;
			FC_anchorMin = goal.GetComponent<RectTransform> ().anchorMin;
			hor = goal.GetComponent<CaseHandler> ().hor;
			ver = goal.GetComponent<CaseHandler> ().ver;
		}
		
		else {
			firstCaseSelected.GetComponent<RectTransform> ().anchorMax = goal.GetComponent<RectTransform> ().anchorMax;
			firstCaseSelected.GetComponent<RectTransform> ().anchorMin = goal.GetComponent<RectTransform> ().anchorMin;
			firstCaseSelected.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
			firstCaseSelected.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
			firstCaseSelected.GetComponent<CaseHandler> ().hor = goal.GetComponent<CaseHandler>().hor;
			firstCaseSelected.GetComponent<CaseHandler> ().ver = goal.GetComponent<CaseHandler>().ver;

			string position = firstCaseSelected.GetComponent<CaseHandler> ().hor + "" +
				firstCaseSelected.GetComponent<CaseHandler> ().ver;
			myHandler.myCases [position] = firstCaseSelected.GetComponent<CaseHandler>();

			goal.GetComponent<RectTransform> ().anchorMax = FC_anchorMax;
			goal.GetComponent<RectTransform> ().anchorMin = FC_anchorMin;
			goal.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
			goal.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
			goal.GetComponent<CaseHandler> ().hor = hor;
			goal.GetComponent<CaseHandler> ().ver = ver;

			position = goal.GetComponent<CaseHandler> ().hor + "" + goal.GetComponent<CaseHandler> ().ver;
			myHandler.myCases [position] = goal.GetComponent<CaseHandler>();

			myHandler.SetNeighbours ();
			Destroy(firstCaseSelected.transform.Find("Selected").gameObject);
			firstCaseSelected.GetComponent<CaseHandler> ().specialProperties ["Selected"] = false;

			firstCaseSelected = null;
		}
	}

	public void SetState(string state){
		if ((state != "Switch" || state != "Copy") && firstCaseSelected!=null) {
			Destroy(firstCaseSelected.transform.Find("Selected").gameObject);
			firstCaseSelected = null;
		}
		if (new List<string>{ "Switch", "Copy", "none" }.Contains (state) && allSelected.Count > 0) {
			firstSelected = null;
			foreach (CaseHandler old in allSelected) {
				old.specialProperties ["Selected"] = false;
				Destroy (old.gameObject.transform.Find ("Selected").gameObject);
			}
			allSelected = new List<CaseHandler> ();
		}
		cursorState = state;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) || Input.GetAxis("Mouse ScrollWheel") > 0f)
			isLeft=true;
		if(Input.GetMouseButtonDown(1) || Input.GetAxis("Mouse ScrollWheel") < 0f)
			isLeft=false;
	}
}
