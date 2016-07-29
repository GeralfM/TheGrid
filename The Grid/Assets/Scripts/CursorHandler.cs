using UnityEngine;
using System.Collections;

public class CursorHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}

	public string cursorState { get; set;}
	public bool isLeft { get; set;}

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

			firstCaseSelected = null;
		}
	}

	public void SetState(string state){
		if (state != "Switch" && firstCaseSelected!=null) {
			Destroy(firstCaseSelected.transform.Find("Selected").gameObject);
			firstCaseSelected = null;
		}
		cursorState = state;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
			isLeft=true;
		if(Input.GetMouseButtonDown(1))
			isLeft=false;
	}
}
