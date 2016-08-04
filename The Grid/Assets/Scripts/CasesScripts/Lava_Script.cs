using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lava_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		myCase.specialProperties ["Solid"] = false;
		StartCoroutine (Routine ());
		StartCoroutine (Drain ());
	}

	public IEnumerator Routine(){
		yield return new WaitForSeconds (150*myCase.timeM+Random.Range(-1f,1f));
		StartCoroutine (Routine ());
		if (!myCase.specialProperties ["Paused"]) {

			Dictionary<int, CaseHandler> candidates = new Dictionary<int, CaseHandler> ();
			int i = 0;
			foreach (CaseHandler neigh in myCase.neighbours.Values) {
				if (neigh.specialProperties["Flammable"] && !neigh.specialProperties ["Fire"]) {
					i++;
					candidates.Add (i, neigh);
				}
			}
			if (i > 0) {
				int goal = Random.Range (1, i + 1);
				candidates [goal].myHandler.NewAttribute (candidates [goal].gameObject, "Fire");
			}

		}
	}

	public IEnumerator Drain(){
		yield return new WaitForSeconds (50*myCase.timeM);
		StartCoroutine (Drain ());
		if (!myCase.specialProperties ["Paused"]) {

			foreach (CaseHandler neigh in myCase.neighbours.Values) {
				neigh.ChangeParam ("Heat", 20);
				neigh.ChangeParam ("Humidity", -20);
			}

		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
