using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaseHandler : MonoBehaviour {

	public GameObject myHeat;
	public GameObject myHum;
	public GameObject myVit;

	public GeneralHandler myHandler { get; set;}
	public CursorHandler myCursor { get; set;}
	public Animator myAnim { get; set;}
	public GroundHandler myGround { get; set;}

	public string type { get; set;}
	public int hor{ get; set;} public int ver{ get; set;}
	public Dictionary<string,string> descriptionBonus = new Dictionary<string,string>();

	public float timeM { get; set;}
	public Dictionary<string,int> caracs = new Dictionary<string, int> ();
	public Dictionary<string,bool> specialProperties = new Dictionary<string, bool>();
	public Dictionary<string, CaseHandler> neighbours = new Dictionary<string, CaseHandler>();

	// Use this for initialization
	void Start () {
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
		myCursor = GameObject.Find ("MainHandler").GetComponent<CursorHandler> ();
		myAnim = gameObject.GetComponent<Animator> ();
		myGround = gameObject.GetComponentInChildren <GroundHandler>();

		type = "Void";

		caracs.Add ("Heat", 50);
		caracs.Add ("Humidity", 50);
		caracs.Add ("Grad_Heat", 0);

		timeM = 1f;
		specialProperties.Add ("Cloud", false);
		specialProperties.Add ("Grass", false);
		specialProperties.Add ("Flammable", false);
		specialProperties.Add ("Fire", false);
		specialProperties.Add ("Day&NightEffects", true);
		specialProperties.Add ("Paused", false);
		specialProperties.Add ("PointerOver", false);
		specialProperties.Add ("Selected", false); //to simplify

		StartCoroutine (AttributeTest ());
		StartCoroutine (HeatMovement ());
	}

	public IEnumerator HeatMovement(){
		yield return new WaitForSeconds (timeM*5f+Random.Range(-1f,1f));
		StartCoroutine (HeatMovement ());

		if (!specialProperties ["Paused"]) {

			Dictionary<CaseHandler,int> division = new Dictionary<CaseHandler, int> ();
			int sum = 0;
			foreach (CaseHandler neigh in neighbours.Values) { 
				int temp = Mathf.Max (caracs ["Heat"] - neigh.caracs ["Heat"], 0);
				if (temp > 0)
					division.Add (neigh, temp);
				sum += temp;
			}
			int loss = 0;
			if (sum > 0)
				loss = sum / 2 / division.Count;
			if (loss > 0) {
				foreach (CaseHandler neigh in division.Keys) {
					int gain = Mathf.Max (loss * division [neigh] / sum - neigh.caracs ["Grad_Heat"] / division.Count, 0);
					neigh.ChangeParam ("Heat", gain);
					ChangeParam ("Heat", -gain);
				}
			}
			SynchroParams ();

		}
	}

	public IEnumerator AttributeTest(){
		yield return new WaitForSeconds (50*timeM);
		StartCoroutine( AttributeTest ());
		if (!specialProperties ["Paused"]) {
			
			if (caracs ["Humidity"] >= 80 && Random.Range (1, 101) <= 50 && !specialProperties ["Cloud"])
				myHandler.NewAttribute (gameObject, "Cloud");
			
		}
	}

	//===============================================================================================

	public void ChangeParam(string param, int value){
		caracs [param] = Mathf.Max (Mathf.Min (caracs [param] + value, 100), 0);
		SynchroParams ();
		if (specialProperties["Flammable"])
			TestFire ();
	}

	public void TestFire(){
		if (caracs ["Humidity"] == 0 && caracs ["Heat"] == 100 && !specialProperties["Fire"])
			myHandler.NewAttribute (gameObject, "Fire");
	}

	public void BeingScrolled(){
		if (new List<string>{ "Heat", "Humidity" }.Contains (myCursor.cursorState))
			BeigClicked ();
	}

	public void BeigClicked(){

		List<CaseHandler> goals = new List<CaseHandler> ();
		if (specialProperties ["Selected"] && !(myCursor.cursorState == "Select"))
			goals.AddRange (myCursor.allSelected);
		else
			goals.Add (this);

		foreach (CaseHandler goal in goals) {
			if (new List<string>{ "Heat", "Humidity" }.Contains (myCursor.cursorState)) {
				if (myCursor.isLeft)
					goal.ChangeParam (myCursor.cursorState, 5);
				else
					goal.ChangeParam (myCursor.cursorState, -5);
				SynchroParams ();
			} else if (myCursor.cursorState == "Switch")
				myCursor.SwitchCases (gameObject);
			else if (myCursor.cursorState == "Copy")
				myCursor.CopyCases (gameObject);
		}
	}

	public void SetType(string newType){
		type = newType;
		gameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/" + newType);
	}

	public void PrintCarac(string carac){
		switch(carac)
		{
		case "Heat":
			myHeat.SetActive (!myHeat.activeSelf);
			break;
		case "Humidity":
			myHum.SetActive (!myHum.activeSelf);
			break;
		case "Speed":
			myVit.SetActive (!myVit.activeSelf);
			break;
		default:
			break;
		}
		SynchroParams ();
	}

	public void ReturnDescription(){
		string descr = "Heat : " + caracs ["Heat"] + "\nHumidity : " + caracs ["Humidity"];
		if (specialProperties ["Paused"])
			descr += "\nSpeed : Paused";
		else
			descr += "\nSpeed : " + (1f / timeM).ToString ();
		foreach (string toAdd in descriptionBonus.Keys)
			descr += "\n" + toAdd + " : " + caracs[descriptionBonus[toAdd]];
		myHandler.PrintInfos (type, descr);
	}

	public void SynchroParams(){
		foreach (string param in new List<string>{"Heat","Humidity"}) {
			myAnim.SetInteger (param, caracs [param]);
			myGround.myAnim.SetInteger (param, caracs [param]);
		}
		if(myHeat.activeSelf)
			transform.Find("HeatText").gameObject.GetComponent<Text> ().text = caracs ["Heat"].ToString();
		if(myHum.activeSelf)
			transform.Find("HumText").gameObject.GetComponent<Text> ().text = caracs ["Humidity"].ToString();
		if (myVit.activeSelf) {
			if (specialProperties ["Paused"])
				transform.Find ("VitText").gameObject.GetComponent<Text> ().text = "0";
			else
				transform.Find ("VitText").gameObject.GetComponent<Text> ().text = (1f / timeM).ToString ();
		}
	}

	public void PointerStatus(bool isOver){
		specialProperties ["PointerOver"] = isOver;
	}
	
	// Update is called once per frame
	void Update () {
		if (specialProperties ["PointerOver"])
			ReturnDescription ();
	}
}
