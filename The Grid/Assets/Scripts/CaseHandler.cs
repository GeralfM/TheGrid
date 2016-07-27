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
	}
		
	public void RecomputeHeat(){
		if (!supertype.Contains ("Heat")) {
			int result = 0;
			foreach (CaseHandler neigh in neighbours.Values) {
				int val = neigh.caracs ["Heat"];
				if (val >= 50)
					result += Mathf.Max (val - caracs ["Grad_Heat"], 50);
				else
					result += Mathf.Min (val + caracs ["Grad_Heat"], 50);
			}
			result = Mathf.RoundToInt ( result*1f / neighbours.Count );
			if (result != caracs ["Heat"]) {
				caracs ["Heat"] = result;
				foreach (CaseHandler neigh in neighbours.Values)
					neigh.RecomputeHeat ();
			}
		}
		SynchroParams ();
	}

	public void ChangeParam(string param, int value){
		caracs [param] = Mathf.Max (Mathf.Min (caracs [param] + value, 100), 0);
		SynchroParams ();
	}

	public void BeigClicked(){
		List<string> inter = new List<string>{ "Heat", "Humidity" };
		if (inter.Contains (myCursor.cursorState)) {
			if (myCursor.isLeft)
				caracs [myCursor.cursorState] += 5;
			else
				caracs [myCursor.cursorState] -= 5;
			
			supertype.Add ("Heat"); // WOWOWO... Clic droit température baisse toute la grille au même seuil !
			foreach (CaseHandler neigh in neighbours.Values)
				neigh.RecomputeHeat ();
			supertype.Remove ("Heat");

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
