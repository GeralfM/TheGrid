using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaseHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}

	public string type { get; set;}
	public int hor{ get; set;} public int ver{ get; set;}
	public List<string> supertype = new List<string>();

	public Dictionary<string,int> caracs = new Dictionary<string, int> ();
	public Dictionary<string, CaseHandler> neighbours = new Dictionary<string, CaseHandler>();

	// Use this for initialization
	void Start () {
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();

		type = "Void";

		caracs.Add ("Heat", 50);
		caracs.Add ("Humidity", 50);
		caracs.Add ("Grad_Heat", 0);
	}

	public void SetSourceHeat(int value){
		/*supertype.Add ("Heat");
		caracs ["Heat"] = 80;
		foreach (CaseHandler neigh in neighbours.Values)
			neigh.RecomputeHeat ();*/
	}

	public void RecomputeHeat(){
		if (!supertype.Contains ("Heat")) {
			int result = 0;
			foreach (CaseHandler neigh in neighbours.Values) {
				int val = neigh.caracs ["Heat"];
				if (val >= 50)
					result += Mathf.Max (val - neigh.caracs ["Grad_Heat"], 50);
				else
					result += Mathf.Min (val + neigh.caracs ["Grad_Heat"], 50);
			}
			result = result / neighbours.Count;
			if (result != caracs ["Heat"]) {
				caracs ["Heat"] = result;
				foreach (CaseHandler neigh in neighbours.Values)
					neigh.RecomputeHeat ();
			}
		}
	}

	public void SetType(string newType){
		type = newType;
		gameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/" + newType);
	}

	public void ReturnDescription(){
		myHandler.PrintInfos (type, "Heat : " + caracs ["Heat"] + "\nHumidity : " + caracs ["Humidity"]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
