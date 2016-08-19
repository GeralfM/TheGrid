using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GeneralHandler : MonoBehaviour {

	public GameObject menu;
	public GameObject encyclopedia;
	public GameObject firstCase;
	public GameObject firstAttribute;

	public Text typeCase{ get; set;}
	public Text caraCase{ get; set;}
	public Text timeText{ get; set;}
	public Dictionary<string,bool> properties = new Dictionary<string, bool> ();
	public bool copyAuthorized{ get; set;}
	public Dictionary<string,CaseHandler> myCases = new Dictionary<string, CaseHandler>(); 

	private Dictionary<string,int> orderDisplayed = new Dictionary<string,int>();
	public Dictionary<string,int> neighboursAngle = new Dictionary<string,int>();

	public bool isDay { get; set;}
	public int hour { get; set;}

	void Awake () {
		properties.Add ("copyAuthorized", false);
		properties.Add ("cataclysmsAuthorized", true);

		orderDisplayed.Add("Grass",1);
		orderDisplayed.Add("Organism",1);
		orderDisplayed.Add("Mushroom",2);
		orderDisplayed.Add("Fire",3);
		orderDisplayed.Add("Cloud",4);
		foreach(string element in new List<string>{"Selected","Heat","Hum","Vit","Pres", "Wind"})
			orderDisplayed.Add(element,5);

		neighboursAngle.Add ("01", 0);
		neighboursAngle.Add ("11", 315);
		neighboursAngle.Add ("10", 270);
		neighboursAngle.Add ("1-1", 225);
		neighboursAngle.Add ("0-1", 180);
		neighboursAngle.Add ("-1-1", 135);
		neighboursAngle.Add ("-10", 90);
		neighboursAngle.Add ("-11", 45);

		CreateNewGrid ();
		PrintTime ();
		StartCoroutine (Horloge ());
	}

	public void StartAgain(){
		foreach (CaseHandler aCase in myCases.Values)
			Destroy (aCase.gameObject);
		myCases = new Dictionary<string, CaseHandler>();
		CreateNewGrid ();
		GameObject.Find ("ButtonCopy").GetComponent<Image> ().enabled = properties["copyAuthorized"];

		foreach (string str in 
			new List<string>{"ButtonHeat","ButtonHumidity","ButtonSwitch","ButtonCopy"})
			GameObject.Find (str).GetComponent<ButtonHandler> ().SetSelected (false);
		gameObject.GetComponent<CursorHandler> ().SetState ("none");

		isDay = true;
		hour = 0;
		PrintTime ();
	}

	public void CreateNewGrid(){

		isDay = true; hour = 0;

		for (int i = 0; i < 12; i++) {
			for (int j = 0; j < 8; j++) {
				GameObject newCase = Instantiate (firstCase);
				newCase.transform.SetParent (GameObject.Find ("Background").transform);
				newCase.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, 0);
				newCase.GetComponent<RectTransform> ().anchorMin = new Vector2 (i / 13f, j / 9f);
				newCase.GetComponent<RectTransform> ().anchorMax = new Vector2 ((i + 1) / 13f, (j + 1) / 9f);
				newCase.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
				newCase.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
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
		foreach (CaseHandler aCase in myCases.Values)
			aCase.sqNeighbours = GetAllSquareNeighbours (aCase);

	}

	public void NewAttribute(GameObject goal, string theType){
		List<string> test = new List<string> ();
		foreach (Transform child in goal.transform)
			test.Add (child.gameObject.name);
		if (!test.Contains (theType) || new List<string>{ "Organism" }.Contains (theType)) {
			GameObject newAttr = Instantiate (firstAttribute);
			newAttr.transform.SetParent (goal.transform);
			newAttr.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
			newAttr.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
			newAttr.GetComponent<RectTransform> ().localScale = Vector3.one;
			newAttr.GetComponent<Transform> ().localPosition = Vector3.zero;
			newAttr.name = theType;
			if (!new List<string>{ "Selected" }.Contains (theType)) {
				encyclopedia.GetComponent<Encyclopedia> ().allTypes [theType] = true;
				newAttr.AddComponent (System.Type.GetType (theType + "_Script"));
			}
			else {
				goal.GetComponent<CaseHandler> ().specialProperties ["Selected"] = true;
				newAttr.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/Selected");
			}
		}
		OrganizeAttributes (goal);
	}

	public void OrganizeAttributes(GameObject goal){
		for (int i = 5; i >= 1; i--)
			foreach (Transform child in goal.transform)
				if (orderDisplayed [child.gameObject.name] == i)
					child.SetAsFirstSibling ();
	}

	//=====================================================================================

	public void RandomizeAll(){
		foreach (CaseHandler aCase in myCases.Values) {
			aCase.caracs ["Heat"] = Random.Range (0, 101);
			aCase.caracs ["Humidity"] = Random.Range (0, 101);
			aCase.caracs ["Pressure"] = Random.Range (0, 101);
			aCase.TestFire ();
			aCase.SynchroParams ();
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

	public List<CaseHandler> GetAllNeighbours(CaseHandler me, int range){
		List<CaseHandler> answer = new List<CaseHandler> ();
		List<CaseHandler> answerInter;
		answer.Add (me);
		for (int i = 0; i < range; i++) {
			answerInter = new List<CaseHandler> ();
			foreach (CaseHandler aCase in answer) {
				answerInter.Add (aCase);
				foreach (CaseHandler neigh in aCase.neighbours.Values)
					if (!answerInter.Contains (neigh))
						answerInter.Add (neigh);
			}
			foreach (CaseHandler aCase in answerInter)
				if (!answer.Contains (aCase))
					answer.Add (aCase);
		}
		return answer;
	}

	public Dictionary<string,CaseHandler> GetAllSquareNeighbours(CaseHandler me){
		Dictionary<string,CaseHandler> answer = new Dictionary<string,CaseHandler> ();
		for (int i = me.hor - 1; i <= me.hor + 1; i++)
			for (int j = me.ver - 1; j <= me.ver + 1; j++)
				if (myCases.ContainsKey (i + "" + j) && !(i == me.hor && j == me.ver))
					answer.Add (i + "" + j, myCases [i + "" + j]);
		return answer;
	}

	public void PrintInfos(string type, string descr){
		typeCase.text = type;
		caraCase.text = descr;
	}

	public void DisplayMenu(GameObject cible){
		if (encyclopedia.activeSelf)
			encyclopedia.SetActive (false);
		cible.SetActive (!cible.activeSelf);

		if (cible.name == "Encyclopedia")
			encyclopedia.GetComponent<Encyclopedia> ().MajIcons ();
	}

	public void ChangeConfiguration(string param){
		properties[param] = !properties[param];
	}

	public void Quit(){
		Application.Quit ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T))
			foreach (CaseHandler caseH in myCases.Values)
				caseH.PrintCarac("Heat");
		if (Input.GetKeyDown (KeyCode.H))
			foreach (CaseHandler caseH in myCases.Values)
				caseH.PrintCarac ("Humidity");
		if (Input.GetKeyDown (KeyCode.V))
			foreach (CaseHandler caseH in myCases.Values)
				caseH.PrintCarac("Speed");
		if (Input.GetKeyDown (KeyCode.P))
			foreach (CaseHandler caseH in myCases.Values)
				caseH.PrintCarac("Pressure");
		if (Input.GetKeyDown (KeyCode.W))
			foreach (CaseHandler caseH in myCases.Values)
				caseH.PrintCarac("Wind");
		if (Input.GetKeyDown (KeyCode.Escape)) {
			DisplayMenu (menu);
			if (encyclopedia.activeSelf)
				encyclopedia.SetActive (false);
		}
	}
}
