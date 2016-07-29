using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GeneralHandler : MonoBehaviour {

	public GameObject firstCase;
	public GameObject firstAttribute;

	public Text typeCase{ get; set;}
	public Text caraCase{ get; set;}
	public Text timeText{ get; set;}
	public Dictionary<string,CaseHandler> myCases = new Dictionary<string, CaseHandler>(); 

	public bool isDay { get; set;}
	public int hour { get; set;}

	void Awake () {

		isDay = true; hour = 0;

		for (int i = 0; i < 12; i++) {
			for (int j = 0; j < 8; j++) {
				GameObject newCase = Instantiate (firstCase);
				newCase.transform.SetParent (GameObject.Find ("Background").transform);
				newCase.GetComponent<RectTransform> ().anchorMin = new Vector2 (i / 13f, j / 9f);
				newCase.GetComponent<RectTransform> ().anchorMax = new Vector2 ((i + 1) / 13f, (j + 1) / 9f);
				newCase.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				newCase.GetComponent<CaseHandler> ().hor = i;
				newCase.GetComponent<CaseHandler> ().ver = j;
				newCase.SetActive (true);
				myCases.Add (i + "" + j, newCase.GetComponent<CaseHandler>());
			}
		}
		SetNeighbours ();
			
		typeCase = GameObject.Find ("NameType").GetComponent<Text> ();
		caraCase = GameObject.Find ("NameCaracs").GetComponent<Text> ();
		timeText = GameObject.Find ("NameTime").GetComponent<Text> ();
		PrintTime ();
		StartCoroutine (Horloge ());

	}

	public void SetNeighbours(){

		for (int i = 0; i < 12; i++) {
			for (int j = 0; j < 8; j++) {
				myCases [i + "" + j].neighbours = new Dictionary<string, CaseHandler> ();
				for (int k = -1; k < 2; k ++)
					for (int m = -1; m < 2; m ++) {
						if ((k + m) % 2 != 0 && myCases.ContainsKey ((i + k).ToString () + (j + m).ToString ()))
							myCases [i + "" + j].neighbours.Add (k + "" + m, 
								myCases [(i + k).ToString () + (j + m).ToString ()]);
					}
			}
		}

	}

	public void NewAttribute(GameObject goal, string theType){
		GameObject newAttr = Instantiate (firstAttribute);
		newAttr.transform.SetParent (goal.transform);
		newAttr.transform.localPosition = Vector3.zero;
		newAttr.name = theType;
		switch (theType) {
		case "Cloud":
			newAttr.AddComponent<Cloud_Script> ();
			newAttr.layer = 10;
			break;
		case "Selected":
			newAttr.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Selected");
			break;
		case "Fire":
			newAttr.AddComponent<Fire_Script> ();
			newAttr.layer = 9;
			break;
		case "Grass":
			newAttr.name = "Grass";
			newAttr.layer = 8;
			newAttr.AddComponent<Grass_Script> ();
			break;
		default:
			break;
		}
			
	}

	public void TimeHandler(){
		hour++;

		if (isDay) {
			foreach (CaseHandler caseH in myCases.Values)
				if (caseH.specialProperties ["Day&NightEffects"])
					caseH.ChangeParam ("Heat", 1);
		} else
			foreach (CaseHandler caseH in myCases.Values)
				if (caseH.specialProperties ["Day&NightEffects"])
					caseH.ChangeParam ("Heat", -1);

		if (hour == 12) {
			hour = 0;
			isDay = !isDay;
		}
		PrintTime ();
	}

	public IEnumerator Horloge(){
		yield return new WaitForSeconds (50f);
		StartCoroutine(Horloge ());
		TimeHandler ();
	}

	public void PrintTime(){
		string msg = "SPEED * " + Time.timeScale + "\n";
		if(isDay)
			msg+="JOUR";
		else
			msg+="NUIT";
		msg+="\n"+hour+" H";
		timeText.text = msg;
	}

	public void PrintInfos(string type, string descr){
		typeCase.text = type;
		caraCase.text = descr;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T))
			foreach (CaseHandler caseH in myCases.Values)
				caseH.PrintHeat();
	}
}
