using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaseHandler : MonoBehaviour {

	public GameObject myHeat;
	public GameObject myHum;
	public GameObject myPres;
	public GameObject myVit;
	public GameObject myWind;

	public GeneralHandler myHandler { get; set;}
	public CursorHandler myCursor { get; set;}
	public Animator myAnim { get; set;}

	public string type { get; set;}
	public int hor{ get; set;} public int ver{ get; set;}
	public Dictionary<string,string> descriptionBonus = new Dictionary<string,string>();

	public float timeM { get; set;}
	public Dictionary<string,int> caracs = new Dictionary<string, int> ();
	public Dictionary<string,bool> specialProperties = new Dictionary<string, bool>();
	public Dictionary<string, CaseHandler> neighbours = new Dictionary<string, CaseHandler>();
	public Dictionary<string, CaseHandler> sqNeighbours = new Dictionary<string, CaseHandler>();

	public CaseHandler windGoal { get; set;}

	// Use this for initialization
	void Start () {
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
		myCursor = GameObject.Find ("MainHandler").GetComponent<CursorHandler> ();
		myAnim = gameObject.GetComponent<Animator> ();

		windGoal = null;

		type = "Void";

		caracs.Add ("Heat", 50);
		caracs.Add ("Humidity", 50);
		caracs.Add ("Pressure", 50);
		caracs.Add ("Grad_Heat", 0);
		caracs.Add ("Organisms", 0);
		caracs.Add ("Wind", -1);

		timeM = 1f;
		specialProperties.Add ("Cloud", false);
		specialProperties.Add ("Grass", false);
		specialProperties.Add ("Flammable", false);
		specialProperties.Add ("Fire", false);
		specialProperties.Add ("Organism", false);
		specialProperties.Add ("Mushroom", false);
		specialProperties.Add ("Solid", true);
		specialProperties.Add ("42", false);
		specialProperties.Add ("golden", false);
		specialProperties.Add ("Day&NightEffects", true);
		specialProperties.Add ("Paused", false);
		specialProperties.Add ("PointerOver", false);
		specialProperties.Add ("Selected", false); //to simplify

		StartCoroutine (AttributeTest ());
		StartCoroutine (HeatMovement ());
		StartCoroutine (PressureMovement ());
		StartCoroutine (RefreshWind ());
		if (myHandler.properties ["cataclysmsAuthorized"])
			StartCoroutine (DisasterTest ());
		StartCoroutine (SoGoldMuchWow ());
	}

	/*public void test(){
		foreach (CaseHandler acase in myHandler.myCases.Values) {
			acase.myAnim.CrossFade ("Water", 0f);
			acase.ChangeParam ("Heat", -5);
		}
		myHandler.NewAttribute (gameObject, "Organism");
	}*/

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

	public IEnumerator PressureMovement(){
		yield return new WaitForSeconds (timeM*5f+Random.Range(-1f,1f));
		StartCoroutine (PressureMovement ());

		if (!specialProperties ["Paused"] && !specialProperties["Solid"]) {

			Dictionary<CaseHandler,int> division = new Dictionary<CaseHandler, int> ();
			int sum = 0;
			foreach (CaseHandler neigh in neighbours.Values) { 
				int temp = Mathf.Max (caracs ["Pressure"] - neigh.caracs ["Pressure"], 0);
				if (temp > 0)
					division.Add (neigh, temp);
				sum += temp;
			}
			int loss = 0;
			if (sum > 0)
				loss = sum / 2 / division.Count;
			if (loss > 0) {
				foreach (CaseHandler neigh in division.Keys) {
					int gain = Mathf.Max (loss * division [neigh] / sum, 0);
					neigh.ChangeParam ("Pressure", gain);
					ChangeParam ("Pressure", -gain);
				}
			}
			SynchroParams ();

		}
	}

	public IEnumerator DisasterTest(){
		yield return new WaitForSeconds (50*timeM+Random.Range(-1f,1f));
		StartCoroutine( DisasterTest ());
		if (!specialProperties ["Paused"]) {
			
			if (Random.Range (0f, 1f) <= 0.0017f && new List<string>(){"Void","Water","Stone","Dirt"}.Contains(type)){
				int intensity = Random.Range (0, 3);
				List<CaseHandler> goals = myHandler.GetAllNeighbours (this, intensity);
				foreach (CaseHandler aCase in goals) {
					if (type == "Void")
						aCase.TransitionWithParams ("Steam", 90, 95);
					else if (type == "Water")
						aCase.TransitionWithParams ("Water", -1, -1);
					else if (type == "Stone" || type == "Dirt")
						aCase.TransitionWithParams ("Lava", -1, 98);
				}
			}

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

	public IEnumerator RefreshWind(){
		yield return new WaitForSeconds (1+Random.Range(-0.1f,0.1f));
		StartCoroutine( RefreshWind ());
		RecomputeWindDirection ();
	}

	//=======================MINOR FUNCTIONS====================================================

	public void ChangeParam(string param, int value){
		caracs [param] = Mathf.Max (Mathf.Min (caracs [param] + value, 100), 0);
		SynchroParams ();
		if (specialProperties["Flammable"])
			TestFire ();
		specialProperties ["42"] = (caracs ["Heat"] == 42 && caracs ["Humidity"] == 42 && caracs["Pressure"] == 42);
	}

	public void RecomputeWindDirection(){
		bool greater = false;
		bool singleMinimum = false;
		windGoal = this;
		foreach (CaseHandler neigh in sqNeighbours.Values) {
			if (neigh.caracs ["Pressure"] > caracs ["Pressure"])
				greater = true;
			if (neigh.caracs ["Pressure"] < windGoal.caracs ["Pressure"]) {
				windGoal = neigh;
				singleMinimum = true;
			} else if (neigh.caracs ["Pressure"] == windGoal.caracs ["Pressure"])
				singleMinimum = false;
				
		}
		if (greater && singleMinimum && windGoal != this) 
			caracs ["Wind"] = myHandler.neighboursAngle [(windGoal.hor - hor).ToString () +
				(windGoal.ver - ver).ToString ()];
		else
			caracs ["Wind"] = -1;
	}

	public IEnumerator SoGoldMuchWow(){
		yield return new WaitForSeconds (0.5f);
		StartCoroutine (SoGoldMuchWow ());
		if (specialProperties ["42"]) {
			if (specialProperties ["golden"])
				gameObject.GetComponent<Image> ().color = new Color (212 / 255f, 175 / 255f, 55 / 255f);
			else
				gameObject.GetComponent<Image> ().color = new Color (255f, 255f, 255f);
			specialProperties ["golden"] = !specialProperties ["golden"];
		} else gameObject.GetComponent<Image> ().color = new Color (255f, 255f, 255f);
	}

	public void TransitionWithParams(string typeGoal, int hum, int heat){
		if (hum != -1)
			ChangeParam ("Humidity", hum - caracs ["Humidity"]);
		if (heat != -1)
			ChangeParam ("Heat", heat - caracs ["Heat"]);
		myAnim.CrossFade (typeGoal, 0f);
	}

	public void TestFire(){
		if (caracs ["Humidity"] == 0 && caracs ["Heat"] == 100 && !specialProperties["Fire"])
			myHandler.NewAttribute (gameObject, "Fire");
	}

	public void BeingScrolled(){
		if (new List<string>{ "Heat", "Humidity", "Pressure" }.Contains (myCursor.cursorState))
			Invoke("BeigClicked",0.01f);
	}

	public void BeigClicked(){

		List<CaseHandler> goals = new List<CaseHandler> ();
		if (specialProperties ["Selected"] && !(myCursor.cursorState == "none"))
			goals.AddRange (myCursor.allSelected);
		else
			goals.Add (this);

		foreach (CaseHandler goal in goals) {
			if (new List<string>{ "Heat", "Humidity", "Pressure" }.Contains (myCursor.cursorState)) {
				if (myCursor.isLeft)
					goal.ChangeParam (myCursor.cursorState, myHandler.scrollValue);
				else
					goal.ChangeParam (myCursor.cursorState, -myHandler.scrollValue);
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
		myHandler.encyclopedia.GetComponent<Encyclopedia> ().CheckDiscovered(type);
	}

	public void PrintCarac(string carac){
		if(carac!="Heat")
			myHeat.SetActive (false);
		else
			myHeat.SetActive (!myHeat.activeSelf);

		if(carac!="Humidity")
			myHum.SetActive (false);
		else
			myHum.SetActive (!myHum.activeSelf);

		if(carac!="Speed")
			myVit.SetActive (false);
		else
			myVit.SetActive (!myVit.activeSelf);

		if(carac!="Pressure")
			myPres.SetActive (false);
		else
			myPres.SetActive (!myPres.activeSelf);

		if(carac!="Wind")
			myWind.SetActive (false);
		else
			myWind.SetActive (!myWind.activeSelf);

		SynchroParams ();
	}

	public void ReturnDescription(){
		string typeDescr = type;
		string descr = "Heat : " + caracs ["Heat"] + "\nHumidity : " +
			caracs ["Humidity"]+ "\nPressure : " + caracs ["Pressure"];
		if (specialProperties ["Paused"])
			descr += "\nSpeed : Paused";
		else
			descr += "\nSpeed : " + (1f / timeM).ToString ();
		foreach (string toAdd in descriptionBonus.Keys)
			typeDescr += "\n" + toAdd + " : " + caracs[descriptionBonus[toAdd]];
		myHandler.PrintInfos (typeDescr, descr);
	}

	public void SynchroParams(){
		foreach (string param in new List<string>{"Heat","Humidity", "Pressure"})
			myAnim.SetInteger (param, caracs [param]);

		if (myHeat.activeSelf) {
			float rate = caracs ["Heat"] / 100f;
			float R = Mathf.Min (1, 2 * rate);
			float G = 1 - 2 * Mathf.Abs (0.5f - rate);
			float B = Mathf.Min (1, 2 - 2 * rate);
			transform.Find ("Heat").Find ("Text").gameObject.GetComponent<Text> ().text = caracs ["Heat"].ToString ();
			transform.Find ("Heat").gameObject.GetComponent<Image> ().color = new Color (R, G, B);
		} else if (myHum.activeSelf) {
			float rate = caracs ["Humidity"] / 100f;
			transform.Find ("Hum").Find ("Text").gameObject.GetComponent<Text> ().text = caracs ["Humidity"].ToString ();
			transform.Find ("Hum").gameObject.GetComponent<Image> ().color = new Color (1 - rate, 1 - rate, 1);
		} else if (myPres.activeSelf) {
			float rate = caracs ["Pressure"] / 100f;
			transform.Find ("Pres").Find ("Text").gameObject.GetComponent<Text> ().text = caracs ["Pressure"].ToString ();
			transform.Find ("Pres").gameObject.GetComponent<Image> ().color = new Color (1, 1 - rate, 0);
		} else if (myVit.activeSelf) {
			if (specialProperties ["Paused"])
				transform.Find ("Vit").Find ("Text").gameObject.GetComponent<Text> ().text = "0";
			else
				transform.Find ("Vit").Find ("Text").gameObject.GetComponent<Text> ().text = (1f / timeM).ToString ();
		} else if (myWind.activeSelf) {
			if (caracs ["Wind"] != -1) {
				transform.Find ("Wind").GetComponent<Image> ().enabled = true;
				transform.Find ("Wind").GetComponent<Transform> ().eulerAngles = new Vector3 (0f, 0f, caracs["Wind"]);
			}
			else
				transform.Find ("Wind").GetComponent<Image> ().enabled = false;
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
