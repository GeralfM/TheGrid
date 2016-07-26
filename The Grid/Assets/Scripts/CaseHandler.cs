﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaseHandler : MonoBehaviour {

	public GeneralHandler myHandler { get; set;}
	public Animator myAnim { get; set;}

	public string type { get; set;}
	public int hor{ get; set;} public int ver{ get; set;}
	public Dictionary<string,string> descriptionBonus = new Dictionary<string,string>();
	public List<string> supertype = new List<string>();

	public Dictionary<string,int> caracs = new Dictionary<string, int> ();
	public Dictionary<string, CaseHandler> neighbours = new Dictionary<string, CaseHandler>();

	// Use this for initialization
	void Start () {
		myHandler = GameObject.Find ("MainHandler").GetComponent<GeneralHandler> ();
		myAnim = gameObject.GetComponent<Animator> ();

		type = "Void";

		caracs.Add ("Heat", 50);
		caracs.Add ("Humidity", 50);
		caracs.Add ("Grad_Heat", 0);
	}

	/*public void Test(){
		if (type != "Stone")
			myAnim.CrossFade ("Stone", 0f);
		else {
			myAnim.CrossFade ("Water", 0f);
		}
	}*/
		
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
		SynchroParams ();
	}

	public void ChangeParam(string param, int value){
		caracs [param] = Mathf.Max (Mathf.Min (caracs [param] + value, 100), 0);
		SynchroParams ();
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
		foreach (string param in new List<string>{"Heat","Humidity"})
			myAnim.SetInteger (param, caracs [param]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
