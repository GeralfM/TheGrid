using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaseHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}
	public CursorHandler myCursor { get; set;}
	public Animator myAnim { get; set;}
	public GroundHandler myGround { get; set;}

	public string type { get; set;}
	public int hor{ get; set;} public int ver{ get; set;}
	public Dictionary<string,string> descriptionBonus = new Dictionary<string,string>();
	public List<string> supertype = new List<string>();

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

		specialProperties.Add ("Cloud", false);
		specialProperties.Add ("Day&NightEffects", true);
		StartCoroutine (CloudTest ());
		StartCoroutine (HeatMovement ());
	}

	/*public void test(){
		foreach (CaseHandler ca in myHandler.myCases.Values)
			ca.ChangeParam ("Humidity", 30);
	}*/

	public IEnumerator HeatMovement(){
		yield return new WaitForSeconds (5);
		StartCoroutine (HeatMovement ());
		int loss = Mathf.RoundToInt ((caracs ["Heat"] - 50) / 2f);
		int neighGain = Mathf.RoundToInt ((float)loss / neighbours.Count);
		int realGain = 0;
		foreach (CaseHandler neigh in neighbours.Values) {
			realGain = (int)Mathf.Sign (neighGain) * Mathf.Max ((Mathf.Abs (neighGain) - neigh.caracs ["Grad_Heat"]), 0);
			neigh.caracs ["Heat"] += realGain;
			caracs ["Heat"] -= realGain;
			neigh.SynchroParams ();
		}
		SynchroParams ();
	}

	public IEnumerator CloudTest(){
		yield return new WaitForSeconds (50);
		StartCoroutine( CloudTest ());
		if (caracs ["Humidity"] >= 80 && Random.Range (1, 101) <= 50 && !specialProperties ["Cloud"]) 
			myHandler.NewAttribute (gameObject, "Cloud");
	}

	public void ChangeParam(string param, int value){
		caracs [param] = Mathf.Max (Mathf.Min (caracs [param] + value, 100), 0);
		SynchroParams ();
	}

	public void BeigClicked(){
		List<string> inter = new List<string>{ "Heat", "Humidity" };
		if (inter.Contains (myCursor.cursorState)) {
			if (myCursor.isLeft)
				ChangeParam (myCursor.cursorState, 5);
			else
				ChangeParam (myCursor.cursorState, -5);

			SynchroParams ();
		}
	}

	public void SetType(string newType){
		type = newType;
		gameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/" + newType);
	}

	public void ReturnDescription(){
		string descr = "Heat : " + caracs ["Heat"] + "\nHumidity : " + caracs ["Humidity"];
		foreach (string toAdd in descriptionBonus.Keys)
			descr += "\n" + toAdd + " : " + caracs[descriptionBonus[toAdd]];
		myHandler.PrintInfos (type, descr);
	}

	public void SynchroParams(){
		foreach (string param in new List<string>{"Heat","Humidity"}) {
			myAnim.SetInteger (param, caracs [param]);
			myGround.myAnim.SetInteger (param, caracs [param]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
